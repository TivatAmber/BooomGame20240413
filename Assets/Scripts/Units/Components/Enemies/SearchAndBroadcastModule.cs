using System.Linq;
using GlobalSystem;
using Units.Components.Enemies.Interface;
using Units.Enemies;
using UnityEngine;

namespace Units.Components.Enemies
{
    internal class SearchAndBroadcastModule
    {
        public void Update(ISearchAndBroadcast entity)
        {
            entity.SearchTarget = null;
            foreach (Entity target in GlobalConfigure.Manager.PlayerEntity
                         .Where(target => 
                             Vector3.Distance(entity.Position, target.transform.position) < entity.SearchRadius))
            {
                entity.SearchTarget = target;
            }
        }

        public void LateUpdate(ISearchAndBroadcast entity)
        {
            if (!entity.CanBroadcast) return;
            
            foreach (Entity target in GlobalConfigure.Manager.EnemyEntity
                         .Where(target => 
                             Vector3.Distance(entity.Position, target.transform.position) < entity.BroadcastRadius))
            {
                if (target is ISearchAndBroadcast searchAndBroadcast)
                {
                    searchAndBroadcast.SearchTarget = entity.SearchTarget;
                }
            }
        }
    }
}