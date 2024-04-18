using Mangers;
using UnityEngine;

namespace Units.Components
{
    public class GravityModule
    {
        public void Update(Entity entity)
        {
            float g = Global.Instance.GravityFunc(entity);
            Vector3 forward = (Global.Instance.PlanetTransform.position - entity.transform.position).normalized;
            entity.NowForce += forward * g;
        }
    }
}