using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Mangers.Systems
{
    public class EntityManager
    {
        private List<Entity> _entities;
        public List<Entity> Entities => _entities;

        public EntityManager()
        {
            Entity[] initialEntities = 
                Object.FindObjectsByType<Entity>(FindObjectsSortMode.InstanceID);
            _entities = new List<Entity>();
            foreach (var nowEntity in initialEntities.Where(nowEntity => !nowEntity.AlwaysInViewField))
            {
                _entities.Add(nowEntity);
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