using Units.Components;
using UnityEngine;

namespace Units
{
    public class Player : Entity
    {
        #region Modules
        private ViewChaserModule _viewChaserModule;
        private InputModule _inputModule;
        private PlayerTransformModule _transformModule;
        private DistanceModule _distanceModule;
        #endregion
        
        #region ViewChaserConfigure
        [Header("ViewChaser")]
        [Range(0, 90)]
        [SerializeField] internal float viewDegree;
        [Range(0, 20)]
        [SerializeField] internal float viewDistance;
        #endregion

        #region PlayerConfigure
        [Header("PlayerConfigure")]
        [SerializeField] internal float drivingForce;
        [SerializeField] internal float fasterDrivingForce;
        [SerializeField] internal float rotationSpeed;
        [SerializeField] internal float slowRotationSpeed;
        [SerializeField] internal float drivingCostPerSec;
        [SerializeField] internal float fasterDrivingCostPerSec;
        [SerializeField] internal float rotationCostPerSec;
        [SerializeField] internal float energyLimit;
        #endregion

        #region PlayerInput
        internal bool RotationOrder;
        internal bool DriveOrder;
        internal bool FasterDriveOrder;
        #endregion
        private new void Start()
        {
            base.Start();
            _transformModule = new PlayerTransformModule(this);
            _distanceModule = new DistanceModule();
            _viewChaserModule = new ViewChaserModule();
            _inputModule = new InputModule();
        }
        private void Update()
        {
            NowForce = Vector3.zero;
            _inputModule.Update(this);
            _distanceModule.Update(this);
            _gravityModule.Update(this);
            _transformModule.Update(this);
            _viewChaserModule.Update(this);
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