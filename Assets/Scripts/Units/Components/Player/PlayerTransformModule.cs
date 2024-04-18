using Mangers;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Units.Components
{
    internal class PlayerTransformModule: TransformModule
    {
        private Transform _entityTransform;
        private Vector3 _mousePosition;
        private Vector3 _forward;
        public PlayerTransformModule(Entity entity) : base(entity)
        {
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
            _mousePosition = Global.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.Set(_mousePosition.x, _mousePosition.y, _entityTransform.position.z);
            _forward = (_mousePosition - _entityTransform.position).normalized;
        }
        private void PlayerUpdate(Player entity)
        {
            CalcCache(entity);
            
            // Rotate
            if(entity.RotationOrder) ChangeRotation(entity);
            else if (entity.canChangeRotationByPlanet) ChangeRotationByPlanet(entity);
            
            // Transform
            if (entity.CanChangeTransform)
            {
                if (entity.FasterDriveOrder && entity.energy >= entity.fasterDrivingCostPerSec * Time.deltaTime)
                {
                    entity.NowForce += _forward * entity.fasterDrivingForce;
                } 
                else if (entity.DriveOrder && entity.energy >= entity.drivingCostPerSec * Time.deltaTime)
                {
                    entity.NowForce += _forward * entity.drivingForce;
                }
                ChangeTransform(entity);
            }
            
            // Debug
            Debug.DrawLine(entity.transform.position, Global.Instance.PlanetTransform.position);
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
            
            entity.transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}