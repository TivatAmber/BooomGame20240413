using System;
using GlobalSystem;
using UnityEngine;

namespace Units.Components
{
    internal class TransformModule
    {
        public virtual void Update(Entity entity)
        {
            switch (entity)
            {
                case Entity:
                    DefaultUpdate(entity);
                    break;
                default:
                    throw new Exception("Please Use Right Class");
            }
        }

        private void DefaultUpdate(Entity entity)
        {
            if (entity.CanChangeTransform) ChangeTransform(entity);
            // if (entity.canChangeRotationByPlanet) ChangeRotationByPlanet(entity);
            // else ChangeRotationBySpeed(entity);
            ChangeRotationBySpeed(entity);
            Debug.DrawLine(entity.transform.position, GlobalConfigure.Planet.PlanetTransform.position);
        }

        protected void ChangeRotationBySpeed(Entity entity)
        {
            float speed = Mathf.Sign(Vector3.Dot(entity.transform.up, entity.NowSpeed.normalized));
            speed *= entity.rotationSpeed;
            entity.transform.Rotate(0, 0, speed * Time.deltaTime);
        }
        
        protected void ChangeTransform(Entity entity)
        {
            Vector3 nowForce = entity.NowForce;
            Vector3 nowSpeed = entity.NowSpeed + nowForce * Time.deltaTime;
            
            entity.NowSpeed = nowSpeed;
            Debug.DrawLine(entity.transform.position, entity.transform.position + entity.NowSpeed.normalized,
                Color.yellow);
            entity.transform.position += nowSpeed * Time.deltaTime;
        }
        protected void ChangeRotationByPlanet(Entity entity)
        {
            Vector3 relativePosition = (GlobalConfigure.Planet.PlanetTransform.position - entity.transform.position)
                .normalized;
            float delta = Vector3.SignedAngle(entity.transform.up, relativePosition, Vector3.forward);
            entity.transform.Rotate(0, 0, delta);
            // float degree = Tools.GetAngleByDeg(relativePosition, GlobalConfigure.Planet.PlanetTransform.up);
            // bool flag = Vector3.Dot(relativePosition, GlobalConfigure.Planet.PlanetTransform.right) >= 0;
            // float lstRotation = entity.transform.rotation.eulerAngles.z;
            // degree = flag ? 360 - degree : degree;
            // entity.transform.Rotate(0, 0,  degree - lstRotation);
        }
        public virtual void Init(Entity entity)
        {
            ChangeRotationByPlanet(entity);
            entity.NowSpeed = GlobalConfigure.Instance.GetCircularVelocity(entity);
        }
    }
}