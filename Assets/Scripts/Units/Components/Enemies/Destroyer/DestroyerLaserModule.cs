using System.Collections.Generic;
using GlobalSystem;
using Units.Enemies;
using Units.SubPrefabs.Weapons;
using Units.SubPrefabs.Weapons.WeaponBullets;
using UnityEngine;

namespace Units.Components.Enemies
{
    public class DestroyerLaserModule: MonoBehaviour
    {
        private float _loadingTime;
        private float _nowInterval;
        private float _attackTime;
        private static readonly List<DestroyerLaser> Lasers = new List<DestroyerLaser>();

        #region PoolFunctions
        private static DestroyerLaser Get(DestroyerLaserModule nowWeapon)
        {
            DestroyerLaser ret;
            if (Lasers.Count > 0)
            {
                ret = Lasers[0];
                Lasers.Remove(ret);
            }
            else
            {
                ret = Instantiate(GlobalConfigure.BulletPrefabs.DestroyerLaserPrefab).GetComponent<DestroyerLaser>();
                ret.transform.SetParent(nowWeapon.transform);
            }
            ret.gameObject.SetActive(true);
            return ret;
        }

        internal static void Recycle(DestroyerLaser target)
        {
            target.gameObject.SetActive(false);
            Lasers.Add(target);
        }
        #endregion
        
        enum DestroyerLaserState
        {
            Idle,
            Preparing,
            Attacking
        }

        private DestroyerLaserState _nowState;

        void ChangeState(Destroyer destroyer)
        {
            if (destroyer.SearchTarget is null)
            {
                _nowState = DestroyerLaserState.Idle;
                _loadingTime = 0f;
                destroyer.laserAttacking = false;
                destroyer.laserPreShoot.SetActive(false);
                return;
            }

            destroyer.canPrepareAttack =
                Vector3.Distance(destroyer.transform.position, destroyer.SearchTarget.transform.position) <
                destroyer.laserRadius;

            if (!destroyer.canPrepareAttack)
            {
                _nowState = DestroyerLaserState.Idle;
                _loadingTime = 0f;
                destroyer.laserAttacking = false;
                destroyer.laserPreShoot.SetActive(false);
                return;
            }
            
            switch (_nowState)
            {
                case DestroyerLaserState.Idle:
                {
                    _nowState = DestroyerLaserState.Preparing;
                    destroyer.laserPreShoot.SetActive(true);
                    destroyer.laserAttacking = true;
                    _loadingTime = 0f;
                    break;
                }
                case DestroyerLaserState.Preparing when _loadingTime > destroyer.laserPrepareTime:
                {
                    _nowState = DestroyerLaserState.Attacking;
                    destroyer.laserAttacking = true;
                    destroyer.laserPreShoot.SetActive(false);
                    _loadingTime = 0f;
                    _attackTime = 0f;
                    break;
                }
                case DestroyerLaserState.Attacking when _attackTime > destroyer.laserAttackTime:
                {
                    _nowState = DestroyerLaserState.Idle;
                    destroyer.laserAttacking = false;
                    _attackTime = 0f;
                    break;
                }
            }
        }
        
        public void MyUpdate(Destroyer destroyer)
        {
            ChangeState(destroyer);
            if (_nowState == DestroyerLaserState.Preparing) _loadingTime += Time.deltaTime;
            else if (_nowState == DestroyerLaserState.Attacking)
            {
                _nowInterval += Time.deltaTime;
                if (_nowInterval >= destroyer.laserAttackInterval)
                {
                    _nowInterval -= destroyer.laserAttackInterval;
                    DestroyerLaser laser = Get(this);
                    laser.Init(transform.position, transform.right, transform.rotation, 
                        destroyer.laserSpeed * Vector3.right, destroyer.laserDamage, destroyer.laserRecycleTime, 
                        destroyer.laserPenetrating);
                }

                _attackTime += Time.deltaTime;
            }
        }
    }
}