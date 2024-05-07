using UnityEngine;

namespace Units.SubPrefabs.Weapons
{
    internal abstract class BaseWeapon: MonoBehaviour
    {
        [Header("BaseConfigure")]
        [SerializeField] protected string weaponName; 
        [SerializeField] protected float bulletDamage;
        [SerializeField] protected float shootInterval;
        [SerializeField] protected float costOfEnergy;
        [SerializeField] protected float bulletSpeed;
        [SerializeField] protected float longAndShortInterval;
        [SerializeField] protected float bulletRecycleTime;
        [SerializeField] protected bool penetrating;
        [SerializeField] protected GameObject weaponUI;
        protected float NowInterval;

        public string WeaponName => weaponName;
        public float BulletDamage => bulletDamage;
        public float ShootInterval => shootInterval;
        public float CostOfEnergy => costOfEnergy;
        public float BulletSpeed => bulletSpeed;
        public float LongAndShortInterval => longAndShortInterval;
        public float BulletRecycleTime => bulletRecycleTime;
        public bool Penetrating => penetrating;

        protected virtual void Start()
        {
            NowInterval = 0f;
        }

        public virtual void ShortDownAttack(Entity entity)
        {
        }

        public virtual void ShortUpAttack(Entity entity)
        {
        }
        public virtual void LongAttack(Entity entity, float duringTime)
        {
        }

        public virtual void LongUpReset(Entity entity, float duringTime)
        {
        }

        public virtual void ResetWeapon()
        {
            
        }

        public virtual void ChangeEffect(Player entity)
        {
            
        }

        public virtual void UndoEffect(Player entity)
        {
            
        }

        public virtual void ShowUI()
        {
            weaponUI.SetActive(true);
        }

        public virtual void CloseUI()
        {
            weaponUI.SetActive(false);
        }
    }
}