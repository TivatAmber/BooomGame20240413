using Mangers;
using UnityEngine;

namespace Units.Components
{
    public class GravityModule
    {
        public void Update(Entity entity)
        {
            float g = Global.Instance.GravityFunc(entity);
            Vector3 forward = (Global.Planet.PlanetTransform.position - entity.transform.position).normalized;
            entity.NowForce += forward * g;

            float distance = (entity.transform.position - Global.Planet.PlanetTransform.position).magnitude;
            if (distance < Global.Energy.R2) return;
            
            if (distance < Global.Energy.R3)
            {
                if (entity.NowSpeed.magnitude < entity.maxSpeed) return;
                float additionSpeed = entity.NowSpeed.magnitude - entity.maxSpeed;
                float ratio = Global.Planet.DragRatio * (distance - Global.Energy.R2) /
                        (Global.Energy.R3 - Global.Energy.R2);
                entity.NowForce += forward * (ratio * additionSpeed + Global.Planet.DragConstant);
            }
            else
            {
                float ratio = Global.Planet.DragRatio * (distance - Global.Energy.R2) /
                              (Global.Energy.R4 - Global.Energy.R2);
                entity.NowForce += forward * (ratio * entity.NowSpeed.magnitude + Global.Planet.DragConstant);
            }
        }
    }
}