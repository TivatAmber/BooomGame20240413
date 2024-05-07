using System;
using System.Collections;
using System.Collections.Generic;
using GlobalSystem;
using Units.Components;
using Units.Components.Enemies;
using Units.Components.Enemies.Interface;
using UnityEngine;

namespace Units.Enemies
{
    public class Destroyer: Entity, ISearchAndBroadcast
    {
        private static readonly List<Destroyer> Destroyers = new List<Destroyer>();
        #region Modules
        private DestroyerTransformModule _transformModule;
        private DestroyerSearchModule _searchModule;
        [SerializeField] private DestroyerLaserModule _laserModule;
        private DistanceModule _distanceModule;
        private SearchAndBroadcastModule _searchAndBroadcastModule;
        // TODO
        #endregion
        
        #region DestroyerConfigure

        [Header("DestroyerConfigure")]
        [SerializeField] private Animator _animator;
        [SerializeField] internal float costOfEnergy;
        [SerializeField] internal float accelerate;
        [SerializeField] internal float followingAccelerate;
        [SerializeField] internal float searchRadius;
        [SerializeField] internal float followingRadius;
        #region Laser

        [SerializeField] internal bool laserAttacking;
        [SerializeField] internal float laserRadius;
        [SerializeField] internal float laserPrepareTime;
        [SerializeField] internal float laserAttackTime;
        [SerializeField] internal float laserRotationSpeed;
        [SerializeField] internal float laserAccelerate;
        [SerializeField] internal float laserDamage;
        [SerializeField] internal float laserSpeed;
        [SerializeField] internal float laserRecycleTime;
        [SerializeField] internal bool laserPenetrating;
        [SerializeField] internal float laserAttackInterval;
        [SerializeField] internal GameObject laserPreShoot;
        #endregion
        [SerializeField] internal bool canBroadcast;
        [SerializeField] internal float broadcastRadius;
        internal bool canPrepareAttack;
        private Entity _target;
        private bool _fallenDown = false;
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

        internal enum DestroyerState
        {
            Idle,
            Chasing,
            Following,
            Sleeping,
        }

        private DestroyerState _nowState;
        internal DestroyerState NowState => _nowState;
        #endregion
        
        #region PoolFunctions

        internal static Destroyer Create(Vector3 pos)
        {
            Destroyer ret = null;
            if (GlobalConfigure.Enemies.Destroyer.NowNum >= GlobalConfigure.Enemies.Destroyer.Limit)
            {
                return ret;
            }
            if (Destroyers.Count > 0)
            {
                ret = Destroyers[0];
                Destroyers.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.EnemiesPrefabs.DestroyerPrefab).GetComponent<Destroyer>();
                ret.transform.SetParent(GlobalConfigure.EnemiesPrefabs.EnemiesPool);
            }
            GlobalConfigure.Instance.SummonDestroyer();
            ret.gameObject.SetActive(true);
            ret.Init(pos);
            return ret;
        }

        private static void Recycle(Destroyer target)
        {
            target.gameObject.SetActive(false);
            Destroyers.Add(target);
        }
        #endregion
        private new void Start()
        {
            base.Start();
            _transformModule ??= new DestroyerTransformModule();
            _distanceModule = new DistanceModule();
            _searchAndBroadcastModule = new SearchAndBroadcastModule();
            _searchModule = new DestroyerSearchModule();
            
            _transformModule.Init(this);
            _fallenDown = false;
            _nowState = DestroyerState.Idle;
        }

        private void Update()
        {
            if (died) EndLife();
            ChangeState();
            NowForce = Vector3.zero;
            
            _searchAndBroadcastModule.Update(this);
            
            _searchModule.Update(this);
            _forceModule.Update(this);

            _laserModule.MyUpdate(this);
            _transformModule.Update(this);
            _distanceModule.Update(this);
        }

        protected override void ChangeState()
        {
            switch (_nowState)
            {
                case DestroyerState.Idle when _target is not null:
                {
                    _nowState = DestroyerState.Chasing;
                    _animator.Play("Chasing");
                    LeaveOrbit();
                    break;
                }
                case DestroyerState.Chasing when _target is null:
                {
                    _nowState = DestroyerState.Sleeping;
                    _animator.Play("Idle");
                    break;
                }
                case DestroyerState.Chasing when Vector3.Distance(transform.position, _target.transform.position) <
                    followingRadius:
                {
                    _nowState = DestroyerState.Following;
                    _animator.Play("Chasing");
                    break;
                }
                case DestroyerState.Following when Vector3.Distance(transform.position, _target.transform.position) >
                                                   followingRadius:
                {
                    _nowState = DestroyerState.Chasing;
                    _animator.Play("Chasing");
                    break;
                }
                case DestroyerState.Sleeping when _target is not null:
                {
                    _nowState = DestroyerState.Chasing;
                    _animator.Play("Chasing");
                    break;
                }
                case DestroyerState.Sleeping
                    when Vector3.Distance(NowSpeed, GlobalConfigure.Instance.GetCircularVelocity(this)) < 0.1f:
                {
                    EnterOrbit();
                    _nowState = DestroyerState.Idle;
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

        protected void Init(Vector3 pos)
        {
            base.BaseInit(GlobalConfigure.Enemies.Destroyer.Health, GlobalConfigure.Enemies.Destroyer.MaxSpeed,
                GlobalConfigure.Enemies.Destroyer.RotationSpeed, GlobalConfigure.Enemies.Destroyer.Energy,
                GlobalConfigure.Enemies.Destroyer.Resources);
            transform.position = pos;
            searchRadius = GlobalConfigure.Enemies.Destroyer.SearchRadius;
            followingRadius = GlobalConfigure.Enemies.Destroyer.FollowingRadius;
            followingAccelerate = GlobalConfigure.Enemies.Destroyer.FollowingAccelerate;
            laserRadius = GlobalConfigure.Enemies.Destroyer.LaserRadius;
            laserPrepareTime = GlobalConfigure.Enemies.Destroyer.LaserPrepareTime;
            laserAttackTime = GlobalConfigure.Enemies.Destroyer.LaserAttackTime;
            laserAttackInterval = GlobalConfigure.Enemies.Destroyer.LaserAttackInterval;
            laserRotationSpeed = GlobalConfigure.Enemies.Destroyer.LaserRotationSpeed;
            laserAccelerate = GlobalConfigure.Enemies.Destroyer.LaserAccelerate;
            laserDamage = GlobalConfigure.Enemies.Destroyer.LaserDamage;
            laserSpeed = GlobalConfigure.Enemies.Destroyer.LaserSpeed;
            laserRecycleTime = GlobalConfigure.Enemies.Destroyer.LaserRecycleTime;
            laserPenetrating = GlobalConfigure.Enemies.Destroyer.LaserPenetrating;
            costOfEnergy = GlobalConfigure.Enemies.Destroyer.CostOfEnergy;
            accelerate = GlobalConfigure.Enemies.Destroyer.Accelerate;
            canBroadcast = GlobalConfigure.Enemies.Destroyer.CanBroadcast;
            broadcastRadius = GlobalConfigure.Enemies.Destroyer.BroadcastRadius;

            _transformModule ??= new DestroyerTransformModule();
            _transformModule.Init(this);
            _fallenDown = false;
            
            StartCoroutine(InitAnimation(GlobalConfigure.Enemies.Destroyer.InitAnimationWaitTime));
        }
        protected override IEnumerator InitAnimation(float waitTime = 1f)
        {
            StartInitAnimation();
            _animator.Play("InitAnimation");
            yield return new WaitForSeconds(waitTime);
            EndInitAnimation();
        }
        
        protected override void EndLife()
        {
            GlobalLevelUp.AllDoneInfo.DestroyerKilledAdd();
            GlobalConfigure.Instance.RecycleDestroyer();
            Recycle(this);
            base.EndLife();
        }
    }
}