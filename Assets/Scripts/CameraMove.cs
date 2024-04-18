using System;
using Units;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 0.8f)] private float ratio;
    [SerializeField] private Entity target;
    [Range(-5, -15)]
    [SerializeField] private float Height;

    private void ChangeTransform()
    {
        Vector3 pos = target.transform.position;
        pos.z = Height;
        transform.position = pos;
    }

    private void ChangeRotation()
    {
        Vector3 targetForward = target.NowSpeed.normalized;
        // Vector3 targetUp = Vector3.Cross(Vector3.forward, targetForward);
        // Debug.DrawLine(target.transform.position, target.transform.position + targetUp, Color.red);
        Vector3 nowForward = transform.right;
        // float deg = Mathf.Sign(Vector3.Dot(targetUp, nowForward));
        // float angle = Vector3.Angle(nowForward, targetForward);
        // Debug.Log(angle + " " + deg + " " + nowForward + " " + targetForward);
        // transform.Rotate(0, 0, -deg * angle);
        if (Vector3.Angle(nowForward, targetForward) > 90f) targetForward = -targetForward;
        Vector3 targetUp = Vector3.Cross(Vector3.forward, targetForward);
        float deg = Mathf.Sign(Vector3.Dot(targetUp, nowForward));
        deg *= Vector3.Angle(nowForward, targetForward);
        transform.Rotate(0, 0, -deg * ratio);
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
        ChangeRotation();
    }
}
