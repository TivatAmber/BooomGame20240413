using Mangers;
using UnityEngine;

namespace Units.Components
{
    internal class ViewChaserModule
    {
        private void DebugLines(Player entity)
        {
            Vector3 pos = entity.transform.position;
            Vector3 lb = pos + Quaternion.Euler(0, 0, entity.viewDegree) * entity.transform.right * entity.viewDistance;
            Vector3 rb = pos + Quaternion.Euler(0, 0, -entity.viewDegree) * entity.transform.right * entity.viewDistance;
            Debug.DrawLine(pos, lb);
            Debug.DrawLine(pos, rb);
        }

        public void Update(Player entity)
        {
            DebugLines(entity);
            Vector3 nowPosition = entity.transform.position;
            foreach (var otherEntity in Global.Instance.EntityManager.Entities)
            {
                Vector3 otherPosition = otherEntity.transform.position;
                float distance = (otherPosition - nowPosition).magnitude;
                float degree = Tools.GetAngleByDeg(otherPosition - nowPosition,
                    entity.transform.right);
                bool lstStatus = otherEntity.canBeSee;

                if (-entity.viewDegree < degree 
                    && degree < entity.viewDegree 
                    && distance < entity.viewDistance) otherEntity.canBeSee = true;
                else otherEntity.canBeSee = false;
                
                if (lstStatus ^ entity.canBeSee) otherEntity.ChangeActive();
            }
        }
    }
}