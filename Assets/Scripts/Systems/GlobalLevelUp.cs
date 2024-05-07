using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalSystem
{
    public class GlobalLevelUp: Singleton<GlobalLevelUp>
    {
        [SerializeField] private float allGetResources;
        [SerializeField] private int allCollectorKilled;
        [SerializeField] private int allWatcherKilled;
        [SerializeField] private int allDestroyerKilled;
        [SerializeField] private int allOcculatorKilled;

        public static class AllDoneInfo
        {
            public static float GetResources => Instance.allGetResources;
            public static float CollectorKilled => Instance.allCollectorKilled;
            public static float WatcherKilled => Instance.allWatcherKilled;
            public static float DestroyerKilled => Instance.allDestroyerKilled;
            public static float OcculatorKilled => Instance.allOcculatorKilled;
            public static void ResourceAdd(float value)
            {
                Instance.allGetResources += value;
            }
            public static void CollectorKilledAdd()
            {
                Instance.allCollectorKilled++;
            }
            public static void WatcherKilledAdd()
            {
                Instance.allWatcherKilled++;
            }
            public static void DestroyerKilledAdd()
            {
                Instance.allDestroyerKilled++;
            }
            public static void OcculatorKilledAdd()
            {
                Instance.allOcculatorKilled++;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            SceneManager.LoadScene(2);
        }
    }
}