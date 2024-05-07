using System.Collections.Generic;
using GlobalSystem;
using Units.SubPrefabs.Weapons.WeaponBullets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.SubPrefabs.Weapons
{
    internal sealed class BulletWeapon: BaseWeapon
    {
        private static readonly List<Bullet> Bullets = new List<Bullet>();
        [Header("BulletConfigure")] 
        [Range(0f, 90f)] [SerializeField] private float scatterAngle;

        #region PoolFunctions
        private static Bullet Get()
        {
            Bullet ret;
            if (Bullets.Count > 0)
            {
                ret = Bullets[0];
                Bullets.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.BulletPrefabs.BulletPrefab).GetComponent<Bullet>();
                ret.transform.SetParent(GlobalConfigure.BulletPrefabs.BulletBulletPool);
            }
            ret.gameObject.SetActive(true);
            return ret;
        }

        public static void Recycle(Bullet target)
        {
            target.gameObject.SetActive(false);
            Bullets.Add(target);
        }
        #endregion
        private void Update()
        {
            if (NowInterval < shootInterval)
                NowInterval += Time.deltaTime;
        }

        #region AttackFunctions
        public override void ShortDownAttack(Entity entity)
        {
            if (NowInterval >= shootInterval && entity.energy > costOfEnergy)
            {
                NowInterval -= shootInterval;
                entity.energy -= costOfEnergy;
                Bullet bullet = Get();
                bullet.Init(transform.position, entity.Forward, entity.transform.rotation, 
                    bulletSpeed * entity.Forward + entity.NowSpeed, bulletDamage, bulletRecycleTime,
                    penetrating);
            }
        }

        public override void LongAttack(Entity entity, float duringTime)
        {
            if (NowInterval >= shootInterval && entity.energy > costOfEnergy)
            {
                NowInterval -= shootInterval;
                entity.energy -= costOfEnergy;
                Bullet bullet = Get();
                Quaternion randomAng = Quaternion.Euler(0, 0, Random.Range(-scatterAngle, scatterAngle));
                Quaternion degree = randomAng;
                Vector3 forward = randomAng * entity.Forward;
                bullet.Init(transform.position, forward, degree, 
                    bulletSpeed * entity.Forward + entity.NowSpeed, bulletDamage, bulletRecycleTime, 
                    penetrating);
            }
        }
        #endregion
    }
}