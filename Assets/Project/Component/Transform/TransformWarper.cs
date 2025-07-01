using UnityEngine;

public static class TransformWarper
{
    /// <summary>
    /// Warps a transform smoothly from enterPosition/enterRotation to targetPosition/targetRotation at interpolation t ∈ [0,1].
    /// </summary>
    public static void WarpTransform(
        Vector3 enterPosition,
        Quaternion enterRotation,
        Transform transform,
        Vector3 targetPosition,
        Quaternion targetRotation,
        float t)
    {
        // Interpolate position
        Vector3 warpedPosition = Vector3.Lerp(enterPosition, targetPosition, t);
        transform.position = warpedPosition;

        // Interpolate rotation
        Quaternion warpedRotation = Quaternion.Slerp(enterRotation, targetRotation, t);
        transform.rotation = warpedRotation;
    }
}
