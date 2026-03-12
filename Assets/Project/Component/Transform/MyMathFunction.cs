using UnityEngine;

public static class MyMathFunction 
{
    public static Vector3 LerpClampDistance(
    Vector3 start,
    Vector3 end,
    float deltaDistance,
    float t)
    {
        // Normal lerp
        Vector3 result = Vector3.Lerp(start, end, t);

        // Direction from start
        Vector3 dir = result - start;
        float dist = dir.magnitude;

        // Clamp distance
        if (dist > deltaDistance)
        {
            dir = dir / dist; // normalize
            result = start + dir * deltaDistance;
        }

        return result;
    }

    public static Quaternion LerpClampAngle(
    Quaternion start,
    Quaternion end,
    float maxDeltaAngle,
    float t)
    {
        // Normal interpolation
        Quaternion result = Quaternion.Lerp(start, end, t);

        // Angle from start to result
        float angle = Quaternion.Angle(start, result);

        // Clamp if exceeding max angle
        if (angle > maxDeltaAngle)
        {
            float ratio = maxDeltaAngle / angle;
            result = Quaternion.Slerp(start, result, ratio);
        }

        return result;
    }
}
