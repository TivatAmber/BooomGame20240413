using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace GlobalSystem.Systems
{
    public class ViewEntityManager
    {
        private readonly List<Entity> _entities = new List<Entity>();
        public List<Entity> Entities => _entities;

        public ViewEntityManager()
        {
            Entity[] initialEntities = 
                Object.FindObjectsByType<Entity>(FindObjectsSortMode.InstanceID);
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