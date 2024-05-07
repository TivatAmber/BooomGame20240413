using System.Collections.Generic;
using GlobalSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Units.Enemies
{
    public class Occultator: MonoBehaviour
    {
        private List<OccultatorUnit> nowOccultator = new List<OccultatorUnit>();
        #region PoolFunction
        static readonly List<OccultatorUnit> OccultatorUnits = new List<OccultatorUnit>();
        
        #endregion
    }
}