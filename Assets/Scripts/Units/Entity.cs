using System;
using System.Collections;
using System.Collections.Generic;
using GlobalSystem;
using UnityEngine;
using Units.Components;
using UnityEngine.Serialization;

namespace Units
{
    public abstract class Entity : MonoBehaviour
    {
        #region Modules
        internal HealthModule _healthModule;
        internal ForceModule _forceModule;
        #endregion
        
        #region Body
        [Header("Body")] 
        [SerializeField] private List<SpriteRenderer> bodySpriteRenders;
        [SerializeField] private GameObject detect;
        #endregion
        
        #region TransformInformation
        [Header("TransformInformation")]
        [SerializeField] internal float maxSpeed;
        [SerializeField] internal float rotationSpeed;
        [SerializeField] internal bool canChangeRotationByPlanet;
        [SerializeField] internal bool canChangeTransform;
        internal Vector3 NowForce;
        internal Vector3 NowSpeed;
        public Vector3 Forward => transform.right;
        #endregion
        
        #region Configure
        [Header("EntityBaseConfigure")]
        public bool canBeSee = true;
        [SerializeField] internal bool alwaysInViewField;
        [FormerlySerializedAs("resource")] [SerializeField] internal float resources;
        [SerializeField] internal bool canBeHurt;
        [SerializeField] internal float energyLimit;
        [SerializeField] internal float healthLimit;
        [SerializeField] internal float health;
        [SerializeField] internal float energy;
        internal bool died;

        public bool AlwaysInViewField => alwaysInViewField;
        public bool CanChangeRotation => canChangeRotationByPlanet;
        public bool CanChangeTransform => canChangeTransform;
        public float Health => health;
        public float HealthLimit => healthLimit;
        public float Energy => energy;
        public float EnergyLimit => energyLimit;
        public float Resources => resources;
        #endregion

        protected void Start()
        {
            _healthModule = new HealthModule();
            _forceModule = new ForceModule();
            if (alwaysInViewField) canBeSee = true;
            ChangeActive();
        }

        protected void BaseInit(float health, float maxSpeed, float rotationSpeed, float energy, float resources)
        {
            died = false;
            canChangeTransform = true;
            canChangeRotationByPlanet = true;
            this.health = this.healthLimit = health;
            this.maxSpeed = maxSpeed;
            this.rotationSpeed = rotationSpeed;
            this.energy = this.energyLimit = energy;
            this.resources = resources;
        }

        public void ChangeActive()
        {
            // body.SetActive(canBeSee);
            foreach (var bodyRender in bodySpriteRenders)
            {
                bodyRender.enabled = canBeSee;
            }
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

        public virtual void ChangeHealth(float delta)
        {
            if (canBeHurt)
                _healthModule.ChangeHealth(this, delta);
        }

        protected virtual void EndLife()
        {
            if (!alwaysInViewField)
                GlobalConfigure.Manager.EntityManager.Remove(this);
            GlobalLevelUp.AllDoneInfo.ResourceAdd(resources);
        }

        protected virtual void ChangeState()
        {
            
        }
        

        protected void EndInitAnimation()
        {
            canBeHurt = true;
            canChangeTransform = true;
            canChangeRotationByPlanet = true;
        }
        protected void StartInitAnimation()
        {
            canBeHurt = false;
            canChangeTransform = false;
            canChangeRotationByPlanet = false;
        }

        protected virtual IEnumerator InitAnimation(float waitTime = 1f)
        {
            throw new NotImplementedException();
        }
    }
}