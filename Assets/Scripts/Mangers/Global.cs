using Mangers.Systems;
using Units;
using UnityEngine;

namespace Mangers
{
    public class Global : Singleton<Global>
    {
        #region PlanetConfigure
        [Header("Planet")] 
        [SerializeField] private Transform planetTransform;

        [SerializeField] private float gm;
        [SerializeField] private float inverseGm;
        [SerializeField] private float maxInverseGm;
        [SerializeField] private float dragRatio;
        [SerializeField] private float dragConstant;

        public static class Planet
        {
            public static Transform PlanetTransform => Instance.planetTransform;
            public static float DragRatio => Instance.dragRatio;
            public static float DragConstant => Instance.dragConstant;
        }
        public float GravityFunc(Entity entity)
        {
            float distance = (entity.transform.position - planetTransform.position).magnitude;
            if (distance < r0) return - Mathf.Min(inverseGm / (distance * distance), maxInverseGm);
            return gm / (distance * distance);
        }

        public Vector3 GetCircularVelocity(Entity entity)
        {
            Vector3 vec = planetTransform.transform.position - entity.transform.position;
            Vector3 forward = Vector3.Cross(vec.normalized, Vector3.forward);
            return forward * Mathf.Sqrt(gm / vec.magnitude);
        }
        #endregion

        #region EnergyConfigure

        [SerializeField] private float r0;
        [SerializeField] private float r1;
        [SerializeField] private float r2;
        [SerializeField] private float r3;
        [SerializeField] private float r4;

        public static class Energy
        {
            public static float R0 => Instance.r0;
            public static float R1 => Instance.r1;
            public static float R2 => Instance.r2;
            public static float R3 => Instance.r3;
            public static float R4 => Instance.r4;
        }
        #endregion

        #region InputConfigure

        [Header("InputConfigure")]
        [SerializeField] private KeyCode rotation = KeyCode.Mouse0;
        [SerializeField] private KeyCode drive = KeyCode.J;
        [SerializeField] private KeyCode fasterDrive = KeyCode.K;
        public static class Orders
        {
            public static KeyCode Rotation => Instance.rotation;
            public static KeyCode Drive => Instance.drive;
            public static KeyCode FasterDrive => Instance.fasterDrive;
        }
        #endregion

        #region Camera
        [SerializeField] private Camera mainCamera;
        public Camera MainCamera => mainCamera;
        #endregion

        #region Manager
        private EntityManager _entityManager;
        public EntityManager EntityManager => _entityManager;
        #endregion
        private void Start()
        {
            _entityManager = new EntityManager();
            if (mainCamera == null) mainCamera = Camera.main;
        }
    }
}