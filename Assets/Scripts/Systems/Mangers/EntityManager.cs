using System.Collections.Generic;
using Units;
using Units.Components.Enemies.Interface;

namespace GlobalSystem.Systems
{
    public class EntityManager
    {
        private readonly PlayerEntityManager _playerEntityManager = new();
        private readonly ViewEntityManager _viewEntityManager = new();
        private readonly EnemyEntityManager _enemyEntityManager = new();

        public List<Entity> PlayerEntity => _playerEntityManager.Entities;
        public List<Entity> ViewEntity => _viewEntityManager.Entities;
        public List<Entity> EnemyEntity => _enemyEntityManager.Entities;

        public void Add(Entity entity)
        {
            if (entity.CompareTag("Enemies"))
            {
                _enemyEntityManager.Add(entity);
            } 
            if (entity.CompareTag("Player"))
            {
                _playerEntityManager.Add(entity);
            }

            if (!entity.AlwaysInViewField)
            {
                _viewEntityManager.Add(entity);
            }
        }

        public void Remove(Entity entity)
        {
            if (entity.CompareTag("Enemies"))
            {
                _enemyEntityManager.Remove(entity);
            }

            if (entity.CompareTag("Player"))
            {
                _playerEntityManager.Remove(entity);
            }

            if (!entity.AlwaysInViewField)
            {
                _viewEntityManager.Remove(entity);
            }
        }
    }
}