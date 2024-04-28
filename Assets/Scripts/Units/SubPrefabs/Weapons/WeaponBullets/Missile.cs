using System.Collections.Generic;
using System.Linq;
using GlobalSystem;
using UnityEngine;

namespace Units.SubPrefabs.Weapons.WeaponBullets
{
    internal class Missile: BaseBullet
    {
        private float attackRadius;
        protected override void Update()
        {
            base.Update();
            transform.position += Speed * Time.deltaTime;
            if (NowTime >= RecycleTime && gameObject.activeSelf) MissileWeapon.Recycle(this);
        }

        public override void Init(Vector3 position, Vector3 forward, Quaternion degree, 
            float speed, float damage, float recycleTime,
            bool penetrating)
        {
            base.Init(position, forward, degree, speed, damage, recycleTime, penetrating);
            Speed = forward * speed;
            transform.rotation = degree;
        }

        public Missile SetAttackRadius(float attackRadius)
        {
            this.attackRadius = attackRadius;
            return this;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemies"))
            {
                IEnumerable<Entity> nowEntity = GlobalConfigure.Manager.EnemyEntity.Where(target =>
                    Vector3.Distance(transform.transform.position, transform.position) <= attackRadius);
                foreach (Entity entity in nowEntity)
                {
                    entity.ChangeHealth(-Damage);
                }

                if (!Penetrating) RecycleTime = 0f;
            }
        }
    }
}