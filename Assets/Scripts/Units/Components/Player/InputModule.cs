using GlobalSystem;
using UnityEngine;

namespace Units.Components
{
    internal class InputModule
    {
        // TODO
        /*
         * Need to Change Input Key
         * Transform Input Key
         * 
         */
        public void Update(Player entity)
        {
            if (entity.WeaponAttackOrder) entity.WeaponAttackOrderDuringTime += Time.deltaTime;
            else entity.WeaponAttackOrderDuringTime = 0f;
            #region ReadKey
            entity.RotationOrder = Input.GetKey(GlobalConfigure.Orders.Rotation);
            entity.DriveOrder = Input.GetKey(GlobalConfigure.Orders.Drive);
            entity.FasterDriveOrder = Input.GetKey(GlobalConfigure.Orders.FasterDrive);
            
            #region WeaponAttackOrder
            entity.WeaponAttackDownOrder = entity.WeaponAttackOrder == false;
            entity.WeaponAttackUpOrder = entity.WeaponAttackOrder;
            entity.WeaponAttackOrder = Input.GetKey(GlobalConfigure.Orders.WeaponAttack);
            entity.WeaponAttackDownOrder &= entity.WeaponAttackOrder;
            entity.WeaponAttackUpOrder &= entity.WeaponAttackOrder == false;
            #endregion
            
            entity.WeaponChangeOrder = Input.GetKey(GlobalConfigure.Orders.WeaponChange);
            #endregion
            Vector3 mousePosition = GlobalConfigure.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(entity.transform.position, mousePosition);
        }
    }
}