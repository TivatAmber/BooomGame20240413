using Mangers;
using UnityEngine;
using Units.Components;

namespace Units
{
    public abstract class Entity : MonoBehaviour
    {
        #region Modules
        internal HealthModule _healthModule;
        internal GravityModule _gravityModule;
        #endregion
        
        #region TransformInformation
        [Header("TransformInformation")]
        [SerializeField] internal float maxSpeed;
        [SerializeField] internal bool canChangeRotationByPlanet = true;
        [SerializeField] internal bool canChangeTransform = true;
        [SerializeField] internal Vector3 NowForce;
        [SerializeField] internal Vector3 NowSpeed;
        #endregion
        
        #region Configure
        [Header("EntityBaseConfigure")]
        public bool canBeSee = true;
        [SerializeField] internal bool alwaysInViewField = false;
        [SerializeField] internal float health;
        [SerializeField] internal float energy;
        [SerializeField] internal float resource;

        public bool AlwaysInViewField => alwaysInViewField;
        public bool CanChangeRotation => canChangeRotationByPlanet;
        public bool CanChangeTransform => canChangeTransform;
        public float Health => health;
        public float Energy => energy;
        public float Resource => resource;
        #endregion

        [Header("Body")] 
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject detect;

        protected void Start()
        {
            _healthModule = new HealthModule();
            _gravityModule = new GravityModule();
            if (alwaysInViewField) canBeSee = true;
            ChangeActive();
        }

        public void ChangeActive()
        {
            body.SetActive(canBeSee);
            detect.SetActive(!canBeSee);
        }

        public void EnterOrbit()
        {
            canChangeRotationByPlanet = true;
        }

        public void LeaveOrbit()
        {
            canChangeRotationByPlanet = false;
        }
    }
}