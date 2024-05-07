using System;
using UnityEngine;

namespace Units.SubPrefabs.Weapons.WeaponBullets
{
    internal class Bullet: BaseBullet
    {
        protected override void Update()
        {
            base.Update();
            transform.position += Speed * Time.deltaTime;
            if (NowTime >= RecycleTime && gameObject.activeSelf) BulletWeapon.Recycle(this);
        }

        public override void Init(Vector3 position, Vector3 forward, Quaternion degree, 
            Vector3 speed, float damage, float recycleTime,
            bool penetrating)
        {
            base.Init(position, forward, degree, speed, damage, recycleTime, penetrating);
            Speed = speed;
            transform.rotation = degree;
        }
    }
}