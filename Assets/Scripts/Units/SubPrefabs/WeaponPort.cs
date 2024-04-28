using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Units.SubPrefabs.Weapons;
using UnityEngine;

namespace Units.SubPrefabs
{
    internal class WeaponPort: MonoBehaviour
    {
        private int _nowWeapon;
        private int _numOfWeapon;
        [SerializeField] private List<BaseWeapon> weaponList;
        [SerializeField] private Player owner;

        public float NowWeaponLongAndShortInterval => weaponList[_nowWeapon].LongAndShortInterval;

        void Start()
        {
            _nowWeapon = 0;
            weaponList ??= new List<BaseWeapon>();
            _numOfWeapon = weaponList.Count;
        }
        public string GetWeaponName()
        {
            return weaponList[_nowWeapon].WeaponName;
        }

        public void ShortDownAttack()
        {
            if (weaponList[_nowWeapon] is not null)
            {
                weaponList[_nowWeapon].ShortDownAttack(owner);
            }
        }

        public void ShortUpAttack()
        {
            if (weaponList[_nowWeapon] is not null)
            {
                weaponList[_nowWeapon].ShortUpAttack(owner);
            }
        }

        public void LongAttack(float duringTime)
        {
            if (weaponList[_nowWeapon] is not null)
            {
                weaponList[_nowWeapon].LongAttack(owner, duringTime);
            }
        }

        public void LongUpReset(float duringTime)
        {
            if (weaponList[_nowWeapon] is not null)
            {
                weaponList[_nowWeapon].LongUpReset(owner, duringTime);
            }
        }

        public void ChangeNextWeapon()
        {
            weaponList[_nowWeapon].ChangeEffect(owner);
            _nowWeapon = (_nowWeapon + 1) % _numOfWeapon;
            weaponList[_nowWeapon].UndoEffect(owner);
            
            Debug.Log(weaponList[_nowWeapon].WeaponName);
        }

        public void ChangeLastWeapon()
        {
            weaponList[_nowWeapon].ChangeEffect(owner);
            _nowWeapon = (_nowWeapon - 1 + _numOfWeapon) % _numOfWeapon;
            weaponList[_nowWeapon].UndoEffect(owner);
            
        }
    }
}