using GlobalSystem;
using UnityEngine;

namespace Units.Components
{
    public class ForceModule
    {
        public void Update(Entity entity)
        {
            float g = GlobalConfigure.Instance.GravityFunc(entity);
            Vector3 forward = (GlobalConfigure.Planet.PlanetTransform.position - entity.transform.position).normalized;
            entity.NowForce += forward * g;

            float distance = (entity.transform.position - GlobalConfigure.Planet.PlanetTransform.position).magnitude;
            if (distance < GlobalConfigure.Energy.R2) return;

            forward = -entity.NowSpeed;
            if (distance < GlobalConfigure.Energy.R3)
            {
                if (entity.NowSpeed.magnitude < entity.maxSpeed) return;
                float additionSpeed = entity.NowSpeed.magnitude - entity.maxSpeed;
                float ratio = GlobalConfigure.Planet.DragRatio * (distance - GlobalConfigure.Energy.R2) /
                        (GlobalConfigure.Energy.R3 - GlobalConfigure.Energy.R2);
                entity.NowForce += forward * (ratio * additionSpeed + GlobalConfigure.Planet.DragConstant);
            }
            else
            {
                float ratio = GlobalConfigure.Planet.DragRatio * (distance - GlobalConfigure.Energy.R2) /
                              (GlobalConfigure.Energy.R4 - GlobalConfigure.Energy.R2);
                entity.NowForce += forward * (ratio * entity.NowSpeed.magnitude + GlobalConfigure.Planet.DragConstant);
            }
        }
    }
}