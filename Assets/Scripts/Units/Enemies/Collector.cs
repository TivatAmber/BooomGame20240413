using System;
using System.Collections;
using System.Collections.Generic;
using GlobalSystem;
using Units.Components;
using Units.Components.Enemies;
using UnityEngine;

namespace Units.Enemies
{
    public sealed class Collector : Entity
    {
        private static readonly List<Collector> Collectors = new List<Collector>();
        #region Modules
        private TransformModule _transformModule;
        private List<SummonModule> _summonModules;
        #endregion

        #region CollectorConfigure

        [Header("CollectorConfigure")]
        [SerializeField] private Animator _topAnimator;
        [SerializeField] private Animator _bottomAnimator;
        [SerializeField] private Animator _mainAnimator;
        [SerializeField] internal float summonDestroyerInterval;
        [SerializeField] internal int summonDestroyerNum;
        [SerializeField] internal float summonDestroyerDistance;
        [SerializeField] internal float summonWatcherInterval;
        [SerializeField] internal int summonWatcherNum;
        [SerializeField] internal float summonWatcherDistance;
        #endregion
        
        #region PoolFunctions
        internal static Collector Create(Vector3 origin, Vector3 forward)
        {
            Collector ret = null;
            if (GlobalConfigure.Enemies.Collector.NowNum >= GlobalConfigure.Enemies.Collector.Limit)
            {
                return ret;
            }
            if (Collectors.Count > 0)
            {
                ret = Collectors[0];
                Collectors.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.EnemiesPrefabs.CollectorPrefab).GetComponent<Collector>();
                ret.transform.SetParent(GlobalConfigure.EnemiesPrefabs.EnemiesPool);
            }
            GlobalConfigure.Instance.SummonCollector();
            ret.gameObject.SetActive(true);
            ret.transform.position = origin;
            ret.Init(forward);
            return ret;
        }

        private void Init(Vector3 forward)
        {
            BaseInit(GlobalConfigure.Enemies.Collector.Health, GlobalConfigure.Enemies.Collector.MaxSpeed,
                GlobalConfigure.Enemies.Collector.RotationSpeed, GlobalConfigure.Enemies.Collector.Energy,
                GlobalConfigure.Enemies.Collector.Resources);
            
            transform.position += forward * GlobalConfigure.Enemies.Collector.Height;
            summonDestroyerInterval = GlobalConfigure.Enemies.Collector.SummonDestroyerInterval;
            summonWatcherInterval = GlobalConfigure.Enemies.Collector.SummonWatcherInterval;
            summonDestroyerNum = GlobalConfigure.Enemies.Collector.SummonDestroyerNum;
            summonWatcherNum = GlobalConfigure.Enemies.Collector.SummonWatcherNum;
            summonDestroyerDistance = GlobalConfigure.Enemies.Collector.SummonDestroyerDistance;
            summonWatcherDistance = GlobalConfigure.Enemies.Collector.SummonWatcherDistance;
            
            _transformModule ??= new TransformModule();
            _transformModule?.Init(this);

            _summonModules ??= new List<SummonModule>
            {
                new(summonWatcherInterval, summonWatcherDistance,
                    summonWatcherNum, GlobalConfigure.EnemiesPrefabs.WatcherPrefab),
                new(summonDestroyerInterval, summonDestroyerDistance,
                    summonDestroyerNum, GlobalConfigure.EnemiesPrefabs.DestroyerPrefab),
            };
            
            foreach (SummonModule summonModule in _summonModules) summonModule.Reset();

            StartCoroutine(InitAnimation(GlobalConfigure.Enemies.Collector.InitAnimationWaitTime));
        }
        
        protected override IEnumerator InitAnimation(float waitTime = 1f)
        {
            StartInitAnimation();
            _topAnimator.Play("TopInit");
            _bottomAnimator.Play("BottomInit");
            _mainAnimator.Play("MainInit");
            yield return new WaitForSeconds(waitTime);
            EndInitAnimation();
        }

        private static void Recycle(Collector target)
        {
            target.gameObject.SetActive(false);
            Collectors.Add(target);
        }
        #endregion
        private new void Start()
        {
            base.Start();
            _transformModule ??= new TransformModule();
            _transformModule.Init(this);
            
            _summonModules ??= new List<SummonModule>
            {
                new(summonWatcherInterval, summonWatcherDistance, 
                    summonWatcherNum, GlobalConfigure.EnemiesPrefabs.WatcherPrefab)
            };
        }

        private void Update()
        {
            if (died) EndLife();
            ChangeState();
            NowForce = Vector3.zero;
            _forceModule.Update(this);
            _transformModule.Update(this);
            foreach (SummonModule summonModule in _summonModules)
            {
                summonModule.Update(this);
            }
        }

        protected override void ChangeState()
        {
            if ((transform.position - GlobalConfigure.Planet.PlanetTransform.position).magnitude <
                GlobalConfigure.Energy.R0)
            {
                died = true;
            }
        }

        protected override void EndLife()
        {
            GlobalLevelUp.AllDoneInfo.CollectorKilledAdd();
            GlobalConfigure.Instance.RecycleCollector();
            Recycle(this);
            base.EndLife();
        }
    }
}