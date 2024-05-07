using System.Collections.Generic;
using GlobalSystem;
using Units.SubPrefabs.Weapons.WeaponBullets;
using UnityEngine;
using UnityEngine.UI;

namespace Units.SubPrefabs.Weapons
{
    internal sealed class LaserWeapon: BaseWeapon
    {
        private static readonly List<Laser> Lasers = new List<Laser>();
        [Header("LaserWeaponConfigure")] 
        [SerializeField] private float damageInterval;
        [SerializeField] private float loadingTime;
        [SerializeField] private float releaseTime;
        [SerializeField] private float viewFieldEffectByDeg;
        [SerializeField] private Image loadingBar;
        private float _nowDamageInterval;
        private float _nowLoadingTime;
        
        #region State
        private enum LaserWeaponState
        {
            Idle,
            Loading,
            Attacking
        }

        private LaserWeaponState _nowState;
        #endregion
        #region PoolFunctions
        private static Laser Get(LaserWeapon nowWeapon)
        {
            Laser ret;
            if (Lasers.Count > 0)
            {
                ret = Lasers[0];
                Lasers.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.BulletPrefabs.LaserPrefab).GetComponent<Laser>();
                ret.transform.SetParent(nowWeapon.transform);
            }
            ret.gameObject.SetActive(true);
            return ret;
        }

        public static void Recycle(Laser target)
        {
            target.gameObject.SetActive(false);
            Lasers.Add(target);
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            _nowDamageInterval = 0f;
        }

        private void Update()
        {
            if (_nowState is LaserWeaponState.Idle)
            {
                if (NowInterval < shootInterval) NowInterval += Time.deltaTime;
            }
            else if (_nowState is LaserWeaponState.Loading)
            {
                if (_nowLoadingTime < loadingTime) _nowLoadingTime += Time.deltaTime;
            }
            else if (_nowState is LaserWeaponState.Attacking)
            {
                PerLaser();
                float nowDeltaTime = Time.deltaTime;
                if (_nowDamageInterval < damageInterval) _nowDamageInterval += nowDeltaTime;
                
                if (NowInterval < releaseTime) NowInterval += nowDeltaTime;
                else ResetState();
            }
        }
        #region ChangeState
        private void ChangeState(Entity entity, float duringTime)
        {
            switch (_nowState)
            {
                case LaserWeaponState.Idle when NowInterval >= shootInterval:
                {
                    _nowState = LaserWeaponState.Loading;
                    _nowLoadingTime = 0f;
                    NowInterval = 0f;
                    break;
                }
                case LaserWeaponState.Loading when entity.energy < costOfEnergy * Time.deltaTime:
                {
                    ResetState();
                    return;
                }
                case LaserWeaponState.Loading when _nowLoadingTime >= loadingTime:
                {
                    _nowState = LaserWeaponState.Attacking;
                    _nowDamageInterval = 0f;
                    NowInterval = 0f;
                    break;
                }
                case LaserWeaponState.Attacking when NowInterval >= releaseTime:
                {
                    ResetState();
                    break;
                }
            }
        }

        private void ResetState()
        {
            _nowState = LaserWeaponState.Idle;
            NowInterval = 0f;
        }
        #endregion
        #region AttackFunctions

        public override void LongAttack(Entity entity, float duringTime)
        {
            ChangeState(entity, duringTime);
            switch (_nowState)
            {
                case LaserWeaponState.Loading:
                    #region UpdateUI

                    loadingBar.fillAmount = _nowLoadingTime / loadingTime;
                    #endregion
                    entity.energy -= costOfEnergy * Time.deltaTime;
                    break;
                case LaserWeaponState.Attacking:                
                    #region UpdateUI
                    loadingBar.fillAmount = 0f;
                    #endregion
                    break;
            }
        }

        private void PerLaser()
        {
            if (_nowDamageInterval > damageInterval)
            {
                _nowDamageInterval -= damageInterval;
                Laser laser = Get(this);
                laser.Init(transform.position, transform.right, transform.rotation, 
                    bulletSpeed * Vector3.right, bulletDamage, bulletRecycleTime, 
                    penetrating);
            }
        }

        public override void ResetWeapon()
        {
            _nowDamageInterval = 0f;
            _nowLoadingTime = 0f;
            loadingBar.fillAmount = 0f;
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