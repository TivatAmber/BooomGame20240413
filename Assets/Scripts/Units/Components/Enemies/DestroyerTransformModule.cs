using GlobalSystem;
using Units.Enemies;
using UnityEngine;

namespace Units.Components.Enemies
{
    internal class DestroyerTransformModule: TransformModule
    {
        public override void Update(Entity entity)
        {
            switch (entity)
            {
                case Destroyer destroyer:
                    DestroyerUpdate(destroyer);
                    break;
                default:
                    base.Update(entity);
                    break;
            }
        }

        private void DestroyerUpdate(Destroyer entity)
        {
            if (entity.CanChangeTransform) ChangeTransform(entity);
            if (entity.SearchTarget is null) ChangeRotationBySpeed(entity);
            else ChangeRotationByTarget(entity);
            Debug.DrawLine(entity.transform.position, GlobalConfigure.Planet.PlanetTransform.position);
        }

        protected override void ChangeTransform(Entity entity)
        {
            Vector3 nowForce = entity.NowForce;
            Vector3 nowSpeed = entity.NowSpeed + nowForce * Time.deltaTime;
            
            entity.NowSpeed = nowSpeed;
            Debug.DrawLine(entity.transform.position, entity.transform.position + entity.NowSpeed.normalized,
                Color.yellow);
            entity.transform.position += nowSpeed * Time.deltaTime;
        }

        private void ChangeRotationByTarget(Destroyer entity)
        {
            Vector3 relativeForward = (entity.SearchTarget.transform.position - entity.transform.position)
                .normalized;
            float speed = Mathf.Sign(Vector3.Dot(entity.transform.up, relativeForward));
            speed = entity.laserAttacking ? speed * entity.laserRotationSpeed : speed * entity.rotationSpeed;
            float delta = Vector3.SignedAngle(entity.transform.right, relativeForward, Vector3.forward);
            
            entity.transform.Rotate(0, 0, Tools.GetCloseToZero(delta, speed));
        }
    }
}