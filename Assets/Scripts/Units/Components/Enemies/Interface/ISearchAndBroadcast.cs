using UnityEngine;

namespace Units.Components.Enemies.Interface
{
    public interface ISearchAndBroadcast
    {
        public Entity SearchTarget
        {
            get;
            set;
        }

        public float SearchRadius
        {
            get;
        }

        public Vector3 Position
        {
            get;
        }

        public bool CanBroadcast
        {
            get;
        }
        public float BroadcastRadius
        {
            get;
        }
    }
}