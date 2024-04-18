using UnityEngine;
public static class Tools
{
    public static float GetAngleByDeg(Vector3 lhs, Vector3 rhs)
    {
        float cos_val = Vector3.Dot(lhs.normalized, rhs.normalized);
        float deg = Mathf.Acos(cos_val) * 180.0f / Mathf.PI;
        return deg;
    }

    public static float GetCloseToZero(float a, float b)
    {
        return Mathf.Abs(a) > Mathf.Abs(b) ? b : a;
    }
}