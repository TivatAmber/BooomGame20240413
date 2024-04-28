using GlobalSystem;
using UnityEngine;
using Units.Enemies;

namespace Units.Components.Enemies
{
    public class SummonModule
    {
        private float _interval;
        private float _distance;
        private float _nowInterval;
        private int _num;
        private GameObject _prefab;
        
        public SummonModule(float interval, float distance, int num, GameObject prefab)
        {
            Init(interval, distance, num, prefab);
        }
        private void Init(float interval, float distance, int num, GameObject prefab)
        {
            _interval = interval;
            _distance = distance;
            _num = num;
            _prefab = prefab;
        }

        public void Reset()
        {
            _nowInterval = 0f;
        }

        public void Update(Entity entity)
        {
            if (_nowInterval < _interval)
            {
                _nowInterval += Time.deltaTime;
                return;
            }

            _nowInterval -= _interval;
            for (int i = 0; i < _num; i++)
            {
                Vector3 forward = Random.insideUnitCircle;
                if (_prefab.TryGetComponent<Watcher>(out _))
                {
                    GlobalConfigure.Manager.EnemyEntity
                        .Add(Watcher.Create(entity.transform.position
                            + forward * _distance));
                } // TODO
            }
        }
    }
}