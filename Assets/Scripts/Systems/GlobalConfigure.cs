using System.Collections;
using System.Collections.Generic;
using GlobalSystem.Systems;
using Units;
using Units.Components.Enemies.Interface;
using Units.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace GlobalSystem
{
    public class GlobalConfigure : Singleton<GlobalConfigure>
    {
        
        private void Start()
        {
            _entityManager = new EntityManager();
            
            if (mainCamera == null) mainCamera = Camera.main;
            _collectorInterval = Random.Range(collectorMinAppearanceInterval, collectorMaxAppearanceInterval);
            _occulatorInterval = Random.Range(occultatorMinAppearanceInterval, occultatorMaxAppearanceInterval);

            _nowCollectorInterval = 0f;
            _nowOccultatorInterval = 0f;
            
            StartCoroutine(Summon());
        }

        private void Update()
        {
            if (_nowCollectorInterval < _collectorInterval) _nowCollectorInterval += Time.deltaTime;
            if (_nowOccultatorInterval < _occulatorInterval) _nowOccultatorInterval += Time.deltaTime;
            #region Debug
            // Debug.Log(_entityManager.ViewEntity.Count);
            #endregion
        }

        private IEnumerator Summon()
        {
            while (true)
            {
                if (_nowCollectorInterval >= _collectorInterval)
                {
                    _entityManager.Add(Collector.Create(GlobalConfigure.Planet.PlanetTransform.position
                        , Random.insideUnitCircle.normalized));
                    _nowCollectorInterval -= _collectorInterval;
                }
                if (_nowOccultatorInterval >= _occulatorInterval)
                {
                    // TODO
                    _nowOccultatorInterval -= _occulatorInterval;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        #region Manager

        private EntityManager _entityManager;
        public static class Manager
        {
            public static List<Entity> ViewEntity => Instance._entityManager.ViewEntity;
            public static List<Entity> EnemyEntity => Instance._entityManager.EnemyEntity;
            public static List<Entity> PlayerEntity => Instance._entityManager.PlayerEntity;
        }
        #endregion
        #region PlanetConfigure
        [Header("PlanetConfigure")] 
        [SerializeField] private Transform planetTransform;

        [SerializeField] private float gm;
        [SerializeField] private float inverseGm;
        [SerializeField] private float maxInverseGm;
        [SerializeField] private float dragRatio;
        [SerializeField] private float dragConstant;

        public static class Planet
        {
            public static Transform PlanetTransform => Instance.planetTransform;
            public static float DragRatio => Instance.dragRatio;
            public static float DragConstant => Instance.dragConstant;
        }
        public float GravityFunc(Entity entity)
        {
            float distance = (entity.transform.position - planetTransform.position).magnitude;
            if (distance < r0) return - Mathf.Min(inverseGm / (distance * distance), maxInverseGm);
            return gm / (distance * distance);
        }

        public Vector3 GetCircularVelocity(Entity entity)
        {
            Vector3 vec = planetTransform.transform.position - entity.transform.position;
            Vector3 forward = Vector3.Cross(vec.normalized, Vector3.forward);
            return forward * Mathf.Sqrt(gm / vec.magnitude);
        }
        #endregion
        #region EnergyConfigure
        [Header("EnergyConfigure")]
        [SerializeField] private float r0;
        [SerializeField] private float e0;
        [SerializeField] private float r1;
        [SerializeField] private float e1;
        [SerializeField] private float r2;
        [SerializeField] private float e2;
        [SerializeField] private float r3;
        [SerializeField] private float e3;
        [SerializeField] private float r4;
        [SerializeField] private float e4;

        public static class Energy
        {
            public static float R0 => Instance.r0;
            public static float E0 => Instance.e0;
            public static float R1 => Instance.r1;
            public static float E1 => Instance.e1;
            public static float R2 => Instance.r2;
            public static float E2 => Instance.e2;
            public static float R3 => Instance.r3;
            public static float E3 => Instance.e3;
            public static float R4 => Instance.r4;
            public static float E4 => Instance.e4;
        }
        #endregion
        #region InputConfigure

        [Header("InputConfigure")]
        [SerializeField] private KeyCode rotation = KeyCode.Mouse0;
        [SerializeField] private KeyCode drive = KeyCode.J;
        [SerializeField] private KeyCode fasterDrive = KeyCode.K;
        [SerializeField] private KeyCode weaponAttack = KeyCode.Z;
        [SerializeField] private KeyCode weaponChange = KeyCode.X;
        
        public static class Orders
        {
            public static KeyCode Rotation => Instance.rotation;
            public static KeyCode Drive => Instance.drive;
            public static KeyCode FasterDrive => Instance.fasterDrive;
            public static KeyCode WeaponAttack => Instance.weaponAttack;
            public static KeyCode WeaponChange => Instance.weaponChange;
        }
        #endregion
        #region CameraConfigure
        [Header("CameraConfigure")]
        [SerializeField] private Camera mainCamera;
        public Camera MainCamera => mainCamera;
        #endregion
        #region EnemiesConfigure
        [Header("EnemiesConfigure")]
        #region AppearanceInterval
        [Header("AppearanceInterval")] 
        [SerializeField] private float collectorMinAppearanceInterval;
        [SerializeField] private float collectorMaxAppearanceInterval;
        private float _collectorInterval;
        private float _nowCollectorInterval;
        [SerializeField] private float occultatorMinAppearanceInterval;
        [SerializeField] private float occultatorMaxAppearanceInterval;
        private float _occulatorInterval;
        private float _nowOccultatorInterval;
        #endregion
        #region CollectorConfigure
        [Header("CollectorConfigure")]
        [SerializeField] private GameObject collectorPrefab;
        [SerializeField] private float collectorHealth;
        [SerializeField] private float collectorMaxSpeed;
        [SerializeField] private float collectorRotationSpeed;
        [SerializeField] private float collectorMinEnergy;
        [SerializeField] private float collectorMaxEnergy;
        [SerializeField] private float collectorMinHeight;
        [SerializeField] private float collectorMaxHeight;
        [SerializeField] private float collectorMinResources;
        [SerializeField] private float collectorMaxResources;
        [SerializeField] private float collectorSummonDestroyerInterval;
        [SerializeField] private float collectorSummonDestroyerDistance;
        [SerializeField] private int collectorSummonDestroyerNum;
        [SerializeField] private float collectorSummonWatcherInterval;
        [SerializeField] private int collectorSummonWatcherNum;
        [SerializeField] private float collectorSummonWatcherDistance;
        #endregion
        #region WatcherConfigure
        [Header("WatcherConfigure")] 
        [SerializeField] private GameObject watcherPrefab;
        [SerializeField] private float watcherHealth;
        [SerializeField] private float watcherMaxSpeed;
        [SerializeField] private float watcherRotationSpeed;
        [SerializeField] private float watcherMinEnergy;
        [SerializeField] private float watcherMaxEnergy;
        [SerializeField] private float watcherMinResources;
        [SerializeField] private float watcherMaxResources;
        [SerializeField] private float watcherCostOfEnergy;
        [SerializeField] private float watcherSearchRadius;
        [SerializeField] private float watcherAccelerate;
        [SerializeField] private float watcherSelfDestructRadius;
        [SerializeField] private float watcherSelfDestructRadiusBias;
        [SerializeField] private float watcherSelfDestructDamage;
        [SerializeField] private bool watcherCanBroadcast;
        [SerializeField] private float watcherBroadcastRadius;
        #endregion
        #region DestroyerConfigure
        [SerializeField] private GameObject destroyerPrefab;
        // TODO
        #endregion
        #region Prefabs
        [Header("EnemiesPools")]
        [SerializeField] private Transform enemiesPool;
        public static class EnemiesPrefabs
        {
            public static Transform EnemiesPool => Instance.enemiesPool;
            public static GameObject CollectorPrefab => Instance.collectorPrefab;
            public static GameObject WatcherPrefab => Instance.watcherPrefab;
            public static GameObject DestroyerPrefab => Instance.destroyerPrefab;
        }
        #endregion
        public static class Enemies
        {
            #region Collector
            public static class Collector
            {
                public static float Health => Instance.collectorHealth;
                public static float Energy => Random.Range(Instance.collectorMinEnergy, Instance.collectorMaxEnergy);
                public static float Height => Random.Range(Instance.collectorMinHeight, Instance.collectorMaxHeight);
                public static float MaxSpeed => Instance.collectorMaxSpeed;
                public static float RotationSpeed => Instance.collectorRotationSpeed;
                public static float Resources =>
                    Random.Range(Instance.collectorMinResources, Instance.collectorMaxResources);
                
                public static float SummonWatcherInterval => Instance.collectorSummonWatcherInterval;
                public static float SummonDestroyerInterval => Instance.collectorSummonDestroyerInterval;
                public static int SummonDestroyerNum => Instance.collectorSummonDestroyerNum;
                public static int SummonWatcherNum => Instance.collectorSummonWatcherNum;
                public static float SummonDestroyerDistance => Instance.collectorSummonDestroyerDistance;
                public static float SummonWatcherDistance => Instance.collectorSummonWatcherDistance;
            }
            #endregion
            #region Watcher
            public static class Watcher
            {
                public static float Health => Instance.watcherHealth;
                public static float Energy => Random.Range(Instance.watcherMinEnergy, Instance.watcherMaxEnergy);
                public static float MaxSpeed => Instance.watcherMaxSpeed;
                public static float RotationSpeed => Instance.watcherRotationSpeed;
                public static float Resources =>
                    Random.Range(Instance.watcherMinResources, Instance.watcherMaxResources);
                public static float CostOfEnergy => Instance.watcherCostOfEnergy;
                public static float SearchRadius => Instance.watcherSearchRadius;
                public static float Accelerate => Instance.watcherAccelerate;
                public static float SelfDestructRadius => Instance.watcherSelfDestructRadius;
                public static float SelfDestructRadiusBias => Instance.watcherSelfDestructRadiusBias;
                public static float SelfDestructDamage => Instance.watcherSelfDestructDamage;
                public static bool CanBroadcast => Instance.watcherCanBroadcast;
                public static float BroadcastRadius => Instance.watcherBroadcastRadius;
            }
            #endregion
            #region Destroyer
            public static class Destroyer
            {
                // TODO
            }
            #endregion
        }
        #endregion
        #region BulletConfigure
        [Header("BulletConfigure")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private Transform bulletPool;

        public static class BulletPrefabs
        {
            public static Transform BulletBulletPool => Instance.bulletPool;
            public static GameObject BulletPrefab => Instance.bulletPrefab;
            public static GameObject MissilePrefab => Instance.missilePrefab;
            public static GameObject LaserPrefab => Instance.laserPrefab;
        }
        #endregion
    }
}