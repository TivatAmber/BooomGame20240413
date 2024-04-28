using System;
using Units;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float rotationRatio;
    [SerializeField] [Range(0f, 1f)] private float transformRatio;
    [SerializeField] private Entity target;
    [SerializeField] [Range(-5, -15)] private float Height;

    private void ChangeTransform()
    {
        Vector3 pos = target.transform.position;
        pos.z = Height;
        transform.position = Vector3.Lerp(transform.position, pos, transformRatio);
    }

    private void ChangeRotation()
    {
        Vector3 targetForward = target.transform.right;
        Vector3 nowForward = transform.right;
        if (Vector3.Angle(nowForward, targetForward) > 90f) targetForward = -targetForward;
        Vector3 targetUp = Vector3.Cross(Vector3.forward, targetForward);
        float deg = Mathf.Sign(Vector3.Dot(targetUp, nowForward));
        deg *= Vector3.Angle(nowForward, targetForward);
        transform.Rotate(0, 0, -deg * rotationRatio);
    }
    private void Start()
    {
        Vector3 pos = target.transform.position;
        pos.z = Height;
        transform.position = pos;
    }

    private void Update()
    {
        ChangeTransform();
        // ChangeRotation();
    }
}
