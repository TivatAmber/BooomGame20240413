using System;
using Mangers;
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
            if (entity.canChangeRotationByPlanet) ChangeRotationByPlanet(entity);
            Debug.DrawLine(entity.transform.position, Global.Planet.PlanetTransform.position);
        }
        
        protected void ChangeTransform(Entity entity)
        {
            Vector3 nowForce = entity.NowForce;
            Vector3 nowSpeed = entity.NowSpeed + nowForce * Time.deltaTime;
            // if (nowSpeed.magnitude > entity.maxSpeed)
            // {
            //     nowSpeed = nowSpeed.normalized * entity.maxSpeed;
            // }
            entity.NowSpeed = nowSpeed;
            Debug.DrawLine(entity.transform.position, entity.transform.position + entity.NowForce.normalized, Color.yellow);
            entity.transform.position += nowSpeed * Time.deltaTime;
        }
        protected void ChangeRotationByPlanet(Entity entity)
        {
            Vector3 relativePosition = entity.transform.position - Global.Planet.PlanetTransform.position;
            float degree = Tools.GetAngleByDeg(relativePosition, Global.Planet.PlanetTransform.up);
            bool flag = Vector3.Dot(relativePosition, Global.Planet.PlanetTransform.right) >= 0;
            float lstRotation = entity.transform.rotation.eulerAngles.z;
            degree = flag ? 360 - degree : degree;
            entity.transform.Rotate(0, 0,  (degree - lstRotation) * 0.6f);
        }
        public TransformModule(Entity entity)
        {
            ChangeRotationByPlanet(entity);
            entity.NowSpeed = Global.Instance.GetCircularVelocity(entity);
        }
    }
}