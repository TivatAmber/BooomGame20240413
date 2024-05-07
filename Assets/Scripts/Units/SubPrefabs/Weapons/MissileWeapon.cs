using System;
using System.Collections.Generic;
using GlobalSystem;
using TMPro;
using Units.SubPrefabs.Weapons.WeaponBullets;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Units.SubPrefabs.Weapons
{
    internal sealed class MissileWeapon : BaseWeapon
    {
        /*
         * Fuck Hnh.
         */
        private static readonly List<Missile> Missiles = new List<Missile>();
        [Header("MissileConfigure")]
        [SerializeField] private float reloadTime;
        [SerializeField] private int maxMagazine;
        [SerializeField] private float viewFieldEffectByDeg;
        [SerializeField] private float attackRadius;
        [SerializeField] private Image loadingBar;
        [SerializeField] private TextMeshProUGUI magazineText;
        private float _nowReloadTime;
        private int _magazine;
        private bool _loading;
        private float _nowLoadingTime;

        #region PoolFunctions
        private static Missile Get()
        {
            Missile ret;
            if (Missiles.Count > 0)
            {
                ret = Missiles[0];
                Missiles.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.BulletPrefabs.MissilePrefab).GetComponent<Missile>();
                ret.transform.SetParent(GlobalConfigure.BulletPrefabs.BulletBulletPool);
            }
            ret.gameObject.SetActive(true);
            return ret;
        }

        public static void Recycle(Missile target)
        {
            target.gameObject.SetActive(false);
            Missiles.Add(target);
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            _magazine = maxMagazine;
            ResetWeapon();
        }

        private void Update()
        {
            if (NowInterval < shootInterval)
                NowInterval += Time.deltaTime;
            if (!_loading) return;
            if (_nowLoadingTime < reloadTime)
                _nowLoadingTime += Time.deltaTime;
        }

        #region AttackFunctions
        public override void ShortUpAttack(Entity entity)
        {
            if (_loading)
            {
                _loading = false;
                return;
            }
            
            if (NowInterval >= shootInterval && _magazine > 0)
            {
                NowInterval -= shootInterval;
                _magazine -= 1;
                #region UpdateUI
                magazineText.SetText("Magazine: " + _magazine);
                #endregion
                Missile missile = Get();
                missile.Init(transform.position, entity.Forward, entity.transform.rotation, 
                    bulletSpeed * entity.Forward + entity.NowSpeed, bulletDamage, bulletRecycleTime,
                    penetrating);
                missile.SetAttackRadius(attackRadius);
            }
        }

        public override void LongAttack(Entity entity, float duringTime)
        {
            if (_magazine == maxMagazine) return;
            if (entity.energy < costOfEnergy * Time.deltaTime) return;
            _loading = true;
            entity.energy -= costOfEnergy * Time.deltaTime;
            #region UpdateUI
            loadingBar.fillAmount = _nowLoadingTime / reloadTime;
            #endregion
            
            if (_nowLoadingTime <= reloadTime) return;
            _nowLoadingTime -= reloadTime;
            _magazine += 1;
            
            #region UpdateUI
            magazineText.SetText("Magazine: " + _magazine);
            #endregion
        }

        public override void ResetWeapon()
        {
            _loading = false;
            _nowLoadingTime = 0f;
            loadingBar.fillAmount = 0f;
            magazineText.SetText("Magazine: " + _magazine);
        }

        #endregion

        #region Effect

        public override void ChangeEffect(Player entity)
        {
            entity.ChangeViewDegreeByAdd(viewFieldEffectByDeg);
        }

        public override void UndoEffect(Player entity)
        {
            entity.ChangeViewDegreeByAdd(-viewFieldEffectByDeg);
        }
        #endregion
    }
}