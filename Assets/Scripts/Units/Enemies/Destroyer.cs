using System.Collections.Generic;
using GlobalSystem;
using Units.Components;
using UnityEngine;

namespace Units.Enemies
{
    public class Destroyer: Entity
    {
        private static readonly List<Destroyer> Destroyers = new List<Destroyer>();
        #region Modules
        private TransformModule _transformModule;
        // TODO
        #endregion
        
        #region DestroyerConfigure
        // [Header("DestroyerConfigure")]
        // TODO
        private bool _fallenDown = false;
        #endregion
        
        #region PoolFunctions

        internal static Destroyer Create(Vector3 pos)
        {
            Destroyer ret;
            if (Destroyers.Count > 0)
            {
                ret = Destroyers[0];
                Destroyers.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.EnemiesPrefabs.DestroyerPrefab).GetComponent<Destroyer>();
                ret.transform.SetParent(GlobalConfigure.EnemiesPrefabs.EnemiesPool);
            }
            ret.gameObject.SetActive(true);
            ret.Init(pos);
            return ret;
        }

        private static void Recycle(Destroyer target)
        {
            target.gameObject.SetActive(false);
            Destroyers.Add(target);
        }
        #endregion

        private new void Start()
        {
            base.Start();
            _transformModule ??= new TransformModule();
            
            _transformModule.Init(this);
            // TODO
            _fallenDown = false;
        }

        protected void Init(Vector3 pos)
        {
            // TODO BaseInit
        }
    }
}