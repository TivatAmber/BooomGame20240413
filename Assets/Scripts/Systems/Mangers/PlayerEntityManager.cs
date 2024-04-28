using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace GlobalSystem.Systems
{
    public class PlayerEntityManager
    {
        private readonly List<Entity> _entities = new List<Entity>();
        public List<Entity> Entities => _entities;

        public PlayerEntityManager()
        {
            Entity[] initialEntities = 
                Object.FindObjectsByType<Entity>(FindObjectsSortMode.InstanceID);
            foreach (Entity entity in initialEntities)
            {
                if (entity.gameObject.CompareTag("Player"))
                {
                    Debug.Log(entity.gameObject.name);
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