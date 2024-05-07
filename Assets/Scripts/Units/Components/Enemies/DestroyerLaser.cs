using Units.SubPrefabs.Weapons.WeaponBullets;
using UnityEngine;

namespace Units.Components.Enemies
{
    internal class DestroyerLaser: BaseBullet
    {
        protected override void Update()
        {
            base.Update();
            transform.localPosition += Speed * Time.deltaTime;
            if (NowTime >= RecycleTime && gameObject.activeSelf) DestroyerLaserModule.Recycle(this);
        }

        public override void Init(Vector3 position, Vector3 forward, Quaternion degree, 
            float speed, float damage, float recycleTime,
            bool penetrating)
        {
            base.Init(position, forward, degree, speed, damage, recycleTime, penetrating);
            Speed = Vector3.right * speed;
            transform.rotation = degree;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Entity entity = other.gameObject.GetComponent<Entity>();
                entity.ChangeHealth(-Damage);
                if (!Penetrating) RecycleTime = 0f;
            }
        }
    }
}