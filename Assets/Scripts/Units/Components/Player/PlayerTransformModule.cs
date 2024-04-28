using GlobalSystem;
using UnityEngine;

namespace Units.Components
{
    internal class PlayerTransformModule: TransformModule
    {
        private Transform _entityTransform;
        private Vector3 _mousePosition;
        private Vector3 _forward;
        public override void Init(Entity entity)
        {
            base.Init(entity);
            entity.NowSpeed = -entity.NowSpeed;
        }

        public override void Update(Entity entity)
        {
            switch (entity)
            {
                case Player player:
                    PlayerUpdate(player);
                    break;
                default:
                    base.Update(entity);
                    break;
            }
        }

        private void CalcCache(Player entity)
        {
            _entityTransform = entity.transform;
            _mousePosition = GlobalConfigure.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.Set(_mousePosition.x, _mousePosition.y, _entityTransform.position.z);
            _forward = (_mousePosition - _entityTransform.position).normalized;
        }
        private void PlayerUpdate(Player entity)
        {
            CalcCache(entity);
            
            // Rotate
            if (entity.RotationOrder) ChangeRotation(entity);
            else if (entity.canChangeRotationByPlanet) PlayerChangeRotationByPlanet(entity);
            
            // Transform
            if (entity.CanChangeTransform)
            {
                if (entity.FasterDriveOrder && entity.energy >= entity.fasterDrivingCostPerSec * Time.deltaTime)
                {
                    if (entity.NowSpeed.magnitude < entity.fasterDrivingMaxSpeed)
                    {
                        entity.NowForce += _forward * entity.fasterDrivingForce;
                        entity.energy -= entity.fasterDrivingCostPerSec * Time.deltaTime;
                    }
                } 
                else if (entity.DriveOrder && entity.energy >= entity.drivingCostPerSec * Time.deltaTime)
                {
                    if (entity.NowSpeed.magnitude < entity.maxSpeed)
                    {
                        entity.NowForce += _forward * entity.drivingForce;
                        entity.energy -= entity.drivingCostPerSec * Time.deltaTime;
                    }
                }
                ChangeTransform(entity);
            }
            
            // Debug
            Debug.DrawLine(entity.transform.position, GlobalConfigure.Planet.PlanetTransform.position);
        }
        protected void PlayerChangeRotationByPlanet(Entity entity)
        {
            Vector3 relativePosition = (entity.transform.position - GlobalConfigure.Planet.PlanetTransform.position)
                .normalized;
            float delta = Vector3.SignedAngle(entity.transform.up, relativePosition, Vector3.forward);
            entity.transform.Rotate(0, 0, delta);
            // float degree = Tools.GetAngleByDeg(relativePosition, GlobalConfigure.Planet.PlanetTransform.up);
            // bool flag = Vector3.Dot(relativePosition, GlobalConfigure.Planet.PlanetTransform.right) >= 0;
            // float lstRotation = entity.transform.rotation.eulerAngles.z;
            // degree = flag ? 360 - degree : degree;
            // entity.transform.Rotate(0, 0,  degree - lstRotation);
        }
        private void ChangeRotation(Player entity)
        {
            if (entity.canChangeRotationByPlanet)
                entity.LeaveOrbit();
            
            float speed = Mathf.Sign(Vector3.Dot(_entityTransform.up, _forward));
            if (entity.energy >= entity.rotationCostPerSec * Time.deltaTime)
            {
                entity.energy -= entity.rotationCostPerSec * Time.deltaTime;
                speed *= entity.rotationSpeed;
            }
            else
            {
                speed *= entity.slowRotationSpeed;
            }
            float maxAngle = Vector3.SignedAngle(_entityTransform.right, _forward, Vector3.forward);
            entity.transform.Rotate(0, 0, Tools.GetCloseToZero(speed * Time.deltaTime, maxAngle));
        }
    }
}