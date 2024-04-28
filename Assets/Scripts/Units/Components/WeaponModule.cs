using System;
using UnityEngine;

namespace Units.Components
{
    internal class WeaponModule
    {
        public virtual void Update(Entity entity)
        {
            switch (entity)
            {
                case Entity:
                    DefaultUpdate(entity);
                    break;
                default:
                    throw new Exception("Please Use Right Class");
            }
        }

        private void DefaultUpdate(Entity entity)
        {
            // TODO
            Debug.LogError("Not Implement");
        }
    }
}