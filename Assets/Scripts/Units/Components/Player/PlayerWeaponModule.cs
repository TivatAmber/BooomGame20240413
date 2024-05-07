using UnityEngine;

namespace Units.Components
{
    internal class PlayerWeaponModule : WeaponModule
    {
        private const float DeltaTime = 0.2f;
        private float _nowTime;
        private bool _lstLongAttack;
        private float _lstDuringTime;

        public void Init()
        {
            _nowTime = 0f;
            _lstLongAttack = false;
            _lstDuringTime = 0f;
        }
        public override void Update(Entity entity)
        {
            switch (entity)
            {
                case Player player:
                    PlayerUpdate(player);
                    break;
                default:
                    base.Update(entity);
                    break;
            }
            if (_nowTime < DeltaTime)
                _nowTime += Time.deltaTime;
        }

        private void PlayerUpdate(Player entity)
        {
            if (entity.WeaponChangeOrder && _nowTime >= DeltaTime)
            {
                entity.WeaponPort.ChangeNextWeapon();
                _nowTime -= DeltaTime;
                return;
            }
            
            if (entity.WeaponAttackUpOrder)
            {
                entity.WeaponPort.ShortUpAttack();
            } 
            else if (entity.WeaponAttackDownOrder)
            {
                entity.WeaponPort.ShortDownAttack();
            }
            else if (entity.WeaponAttackOrderDuringTime > entity.WeaponPort.NowWeaponLongAndShortInterval)
            {
                entity.WeaponPort.LongAttack(entity.WeaponAttackOrderDuringTime);
            }
            else
            {
                entity.WeaponPort.ResetWeapon();
            }
        }
    }
}