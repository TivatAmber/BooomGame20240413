using System.Collections.Generic;
using Units;
using UnityEngine;

namespace GlobalSystem.Systems
{
    public class EnemyEntityManager
    {
        private readonly List<Entity> _entities = new List<Entity>();
        public List<Entity> Entities => _entities;

        public EnemyEntityManager()
        {
            Entity[] initialEntities = 
                Object.FindObjectsByType<Entity>(FindObjectsSortMode.InstanceID);
            foreach (Entity entity in initialEntities)
            {
                if (entity.gameObject.CompareTag("Enemies"))
                {
                    _entities.Add(entity);
                }
            }
        }

        public void Add(Entity entity)
        {
            _entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            _entities.Remove(entity);
        }
    }
}