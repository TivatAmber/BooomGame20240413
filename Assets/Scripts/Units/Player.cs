using GlobalSystem;
using TMPro;
using Units.Components;
using Units.SubPrefabs;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public sealed class Player : Entity
    {
        #region Modules
        private ViewChaserModule _viewChaserModule;
        private InputModule _inputModule;
        private PlayerTransformModule _transformModule;
        private PlayerWeaponModule _weaponModule;
        private DistanceModule _distanceModule;
        private UIModule _uiModule;
        #endregion
        
        #region ViewChaserConfigure
        [Header("ViewChaser")]
        [Range(0, 90)]
        [SerializeField] internal float viewDegree;
        [Range(0, 20)]
        [SerializeField] internal float viewDistance;
        #endregion

        #region PlayerConfigure
        #region PlayerMoveConfigure
        [Header("PlayerMoveConfigure")]
        [SerializeField] internal float drivingForce;
        [SerializeField] internal float fasterDrivingForce;
        [SerializeField] internal float fasterDrivingMaxSpeed;
        [SerializeField] internal float slowRotationSpeed;
        [SerializeField] internal float drivingCostPerSec;
        [SerializeField] internal float fasterDrivingCostPerSec;
        [SerializeField] internal float rotationCostPerSec;
        #endregion
        #region PlayerWeaponConfigure
        [Header("PlayerWeaponConfigure")]
        [SerializeField] internal WeaponPort WeaponPort;
        #endregion
        #region UICOnfigure
        [Header("PlayerUI")]
        [SerializeField] internal TextMeshProUGUI nowSpeedUI;
        [SerializeField] internal TextMeshProUGUI orbitalSpeedUI;
        [SerializeField] internal TextMeshProUGUI heightUI;
        [SerializeField] internal TextMeshProUGUI resourceUI;
        [SerializeField] internal Image healthUI;
        [SerializeField] internal Image energyUI;
        #endregion
        #endregion

        #region PlayerInput
        internal bool RotationOrder;
        internal bool DriveOrder;
        internal bool FasterDriveOrder;
        [SerializeField] internal bool WeaponAttackDownOrder;
        [SerializeField] internal bool WeaponAttackUpOrder;
        internal bool WeaponAttackOrder;
        internal bool WeaponChangeOrder;
        
        internal float WeaponAttackOrderDuringTime;
        #endregion

        #region PlayerState
        internal bool cantBeHurt;
        [SerializeField] internal float nowSpeedModule;
        [SerializeField] internal float orbitalSpeedModule;

        public bool CantBeHurt => cantBeHurt;
        public float NowSpeedModule => nowSpeedModule;
        public float OrbitalSpeedModule => orbitalSpeedModule;
        #endregion
        private new void Start()
        {
            base.Start();
            _distanceModule = new DistanceModule();
            _viewChaserModule = new ViewChaserModule();
            _inputModule = new InputModule();
            _transformModule = new PlayerTransformModule();
            _weaponModule = new PlayerWeaponModule();
            _uiModule = new UIModule();
            
            _transformModule.Init(this);
            _weaponModule.Init();
            health = healthLimit;
        }
        private void Update()
        {
            NowForce = Vector3.zero;
            _inputModule.Update(this);
            _distanceModule.Update(this);
            _forceModule.Update(this);
            _transformModule.Update(this);
            _weaponModule.Update(this);
            _viewChaserModule.Update(this);

            nowSpeedModule = NowSpeed.magnitude;
            orbitalSpeedModule = GlobalConfigure.Instance.GetCircularVelocity(this).magnitude;
            _uiModule.UIUpdate(this);
        }
        #region ChangeViewDegree

        public void ChangeViewDegreeTo(float degree)
        {
            viewDegree = degree;
        }

        public void ChangeViewDegreeByMul(float degree)
        {
            viewDegree *= degree;
        }

        public void ChangeViewDegreeByAdd(float degree)
        {
            viewDegree += degree;
        }

        #endregion
        #region ChangeViewDistance

        public void ChangeViewDistanceTo(float distance)
        {
            viewDistance = distance;
        }

        public void ChangeViewDistanceByMul(float distance)
        {
            viewDistance *= distance;
        }

        public void ChangeViewDistanceByAdd(float distance)
        {
            viewDistance += distance;
        }

        #endregion
    }
}