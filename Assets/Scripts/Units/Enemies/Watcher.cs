using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalSystem;
using UnityEngine;
using Units.Components;
using Units.Components.Enemies;
using Units.Components.Enemies.Interface;

namespace Units.Enemies
{
    internal sealed class Watcher : Entity, ISearchAndBroadcast
    {
        private static readonly List<Watcher> Watchers = new List<Watcher>();

        #region Modules

        private TransformModule _transformModule;
        private DistanceModule _distanceModule;
        private SearchAndBroadcastModule _searchAndBroadcastModule;
        
        #endregion

        #region WatcherConfigure

        [Header("WatcherConfigure")] [SerializeField]
        internal float costOfEnergy;

        [SerializeField] private Animator _animator;
        [SerializeField] internal float searchRadius;
        [SerializeField] internal float accelerate;
        [SerializeField] internal float selfDestructRadius;
        [SerializeField] internal float selfDestructRadiusBias;
        [SerializeField] internal float selfDestructDamage;
        [SerializeField] internal bool canBroadcast;
        [SerializeField] internal float broadcastRadius;
        private bool _fallenDown;
        [SerializeField] private Entity _target;

        #region ICanSearchInterface

        public Entity SearchTarget
        {
            get => _target;
            set => _target = value;
        }

        public float SearchRadius => searchRadius;
        public Vector3 Position => transform.position;
        public bool CanBroadcast => canBroadcast;
        public float BroadcastRadius => broadcastRadius;

        #endregion

        #endregion

        #region StateMachine

        private enum WatcherState
        {
            Idle,
            Chasing,
            Booming,
            Sleeping
        }

        [SerializeField] private WatcherState _nowState;

        #endregion

        #region PoolFunction

        public static Watcher Create(Vector3 pos)
        {
            Watcher ret = null;
            if (GlobalConfigure.Enemies.Watcher.NowNum >= GlobalConfigure.Enemies.Watcher.Limit)
            {
                return ret;
            }
            if (Watchers.Count > 0)
            {
                ret = Watchers[0];
                Watchers.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.EnemiesPrefabs.WatcherPrefab).GetComponent<Watcher>();
                ret.transform.SetParent(GlobalConfigure.EnemiesPrefabs.EnemiesPool);
            }
            GlobalConfigure.Instance.SummonWatcher();
            ret.gameObject.SetActive(true);
            ret.Init(pos);
            return ret;
        }

        private static void Recycle(Watcher target)
        {
            target.gameObject.SetActive(false);
            Watchers.Add(target);
        }

        #endregion

        private new void Start()
        {
            base.Start();
            _transformModule ??= new TransformModule();
            _distanceModule = new DistanceModule();
            _searchAndBroadcastModule = new SearchAndBroadcastModule();

            _transformModule.Init(this);
            _fallenDown = false;
            _nowState = WatcherState.Idle;
        }

        protected void Init(Vector3 pos)
        {
            BaseInit(GlobalConfigure.Enemies.Watcher.Health, GlobalConfigure.Enemies.Watcher.MaxSpeed,
                GlobalConfigure.Enemies.Watcher.RotationSpeed, GlobalConfigure.Enemies.Watcher.Energy,
                GlobalConfigure.Enemies.Watcher.Resources);
            
            transform.position = pos;
            costOfEnergy = GlobalConfigure.Enemies.Watcher.CostOfEnergy;
            searchRadius = GlobalConfigure.Enemies.Watcher.SearchRadius;
            accelerate = GlobalConfigure.Enemies.Watcher.Accelerate;
            selfDestructRadius = GlobalConfigure.Enemies.Watcher.SelfDestructRadius;
            selfDestructRadiusBias = GlobalConfigure.Enemies.Watcher.SelfDestructRadiusBias;
            selfDestructDamage = GlobalConfigure.Enemies.Watcher.SelfDestructDamage;
            canBroadcast = GlobalConfigure.Enemies.Watcher.CanBroadcast;
            broadcastRadius = GlobalConfigure.Enemies.Watcher.BroadcastRadius;
            
            _transformModule ??= new TransformModule();
            _transformModule?.Init(this);
            _fallenDown = false;
            
            StartCoroutine(InitAnimation(GlobalConfigure.Enemies.Collector.InitAnimationWaitTime));
        }
        protected override IEnumerator InitAnimation(float waitTime = 1f)
        {
            StartInitAnimation();
            _animator.Play("InitAnimation");
            yield return new WaitForSeconds(waitTime);
            EndInitAnimation();
        }

        private void Update()
        {
            if (died) EndLife();
            ChangeState();
            NowForce = Vector3.zero;
            if (_nowState == WatcherState.Chasing)
            {
                NowForce = accelerate * (_target.transform.position - transform.position).normalized;
                energy -= Time.deltaTime * costOfEnergy;
            }
            else if (_nowState == WatcherState.Sleeping)
            {
                NowForce = accelerate * (GlobalConfigure.Instance.GetCircularVelocity(this) - NowSpeed);
            }

            _forceModule.Update(this);
            _transformModule.Update(this);
            _searchAndBroadcastModule.Update(this);
            _distanceModule.Update(this);
        }

        private void LateUpdate()
        {
            _searchAndBroadcastModule.LateUpdate(this);
        }

        protected override void ChangeState()
        {
            switch (_nowState)
            {
                case WatcherState.Idle when _target is not null:
                {
                    _nowState = WatcherState.Chasing;
                    _animator.Play("Chasing");
                    LeaveOrbit();
                    break;
                }
                case WatcherState.Chasing when _target is null:
                {
                    _nowState = WatcherState.Sleeping;
                    _animator.Play("Idle");
                    break;
                }
                case WatcherState.Chasing
                    when Vector3.Distance(transform.position, _target.transform.position) < selfDestructRadius - selfDestructRadiusBias:
                {
                    _nowState = WatcherState.Booming;
                    died = true;
                    break;
                }
                case WatcherState.Sleeping when _target is not null:
                {
                    _nowState = WatcherState.Chasing;
                    _animator.Play("Chasing");
                    break;
                }
                case WatcherState.Sleeping 
                    when Vector3.Distance(NowSpeed,GlobalConfigure.Instance.GetCircularVelocity(this)) < 0.1f:
                {
                    EnterOrbit();
                    _nowState = WatcherState.Idle;
                    _animator.Play("Idle");
                    break;
                }
            }

            if (Vector3.Distance(transform.position , GlobalConfigure.Planet.PlanetTransform.position) <
                GlobalConfigure.Energy.R0)
            {
                _fallenDown = true;
                died = true;
            }
        }

        protected override void EndLife()
        {
            GlobalLevelUp.AllDoneInfo.WatcherKilledAdd();
            GlobalConfigure.Instance.RecycleWatcher();
            if (health <= 0)
            {
                IEnumerable<Entity> nowEntities = GlobalConfigure.Manager.EntityManager.EnemyEntity
                    .Where(entity =>
                                Vector3.Distance(transform.position, entity.transform.position) <
                        selfDestructRadius - selfDestructRadiusBias);
                foreach (Entity entity in nowEntities)
                {
                    entity.ChangeHealth(-selfDestructDamage);
                }
            }
            else if (health > 0 && _nowState == WatcherState.Booming)
            {
                IEnumerable<Entity> nowEntities = GlobalConfigure.Manager.EntityManager.PlayerEntity
                    .Where(entity =>
                        Vector3.Distance(transform.position, entity.transform.position) < selfDestructRadius);
                foreach (Entity entity in nowEntities)
                {
                    entity.ChangeHealth(-selfDestructDamage);
                }
            }
            Recycle(this);
            base.EndLife();
        }
    }
}