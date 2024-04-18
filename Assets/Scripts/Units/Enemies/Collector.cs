using Units.Components;
using UnityEngine;

namespace Units.Enemies
{
    public class Collector : Entity
    {
        #region Modules
        private TransformModule _transformModule;
        #endregion
        [Header("CollectorConfigure")] 
        [SerializeField] internal float nearestDistance;
        [SerializeField] internal float farthestDistance;
        internal float MidDistance;

        private new void Start()
        {
            base.Start();
            _transformModule = new TransformModule(this);
        }

        private void Update()
        {
            NowForce = Vector3.zero;
            _gravityModule.Update(this);
            _transformModule.Update(this);
        }
    }
}