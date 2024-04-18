using Mangers;
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
            entity.RotationOrder = Input.GetKey(Global.Orders.Rotation);
            entity.DriveOrder = Input.GetKey(Global.Orders.Drive);
            entity.FasterDriveOrder = Input.GetKey(Global.Orders.FasterDrive);

            Vector3 mousePosition = Global.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(entity.transform.position, mousePosition);
        }
    }
}