using GlobalSystem;
using Units.Enemies;
using UnityEngine;

namespace Units.Components.Enemies
{
    internal class DestroyerSearchModule
    {
        public void Update(Destroyer entity)
        {
            if (entity.NowState == Destroyer.DestroyerState.Sleeping)
            {
                entity.NowForce += entity.accelerate * (GlobalConfigure.Instance.GetCircularVelocity(entity) - entity.NowSpeed);
                return;
            }
            
            if (entity.SearchTarget is null) return;
            Vector3 SumForce = Vector3.zero;
            Vector3 targetForward = (entity.transform.position - entity.SearchTarget.transform.position).normalized;
            Vector3 targetPos = entity.SearchTarget.transform.position + entity.followingRadius * targetForward;
            
            SumForce += (targetPos - entity.transform.position).normalized;
            if (Vector3.Distance(entity.transform.position, GlobalConfigure.Planet.PlanetTransform.position) <
                GlobalConfigure.Energy.R1)
            {
                Vector3 outerForward = (entity.transform.position - GlobalConfigure.Planet.PlanetTransform.position)
                    .normalized;
                SumForce += outerForward;
            }
            if (entity.NowState is Destroyer.DestroyerState.Following)
                entity.NowForce += SumForce.normalized * entity.followingAccelerate;
            else if (entity.laserAttacking)
                entity.NowForce += SumForce.normalized * entity.laserAccelerate;
            else
                entity.NowForce += SumForce.normalized * entity.accelerate;
        }
    }
}