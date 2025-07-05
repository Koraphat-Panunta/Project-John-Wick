using UnityEngine;

public static class MovementWarper 
{
    /// <summary>
    /// Warps a transform smoothly from enterPosition/enterRotation to targetPosition/targetRotation at interpolation t ∈ [0,1].
    /// </summary>
    public static void WarpMovement(
        Vector3 enterPosition,
        Quaternion enterRotation,
        MovementCompoent movementCompoent,
        Vector3 targetPosition,
        Quaternion targetRotation,
        float t)
    {
        // Interpolate position
        Vector3 warpedPosition = Vector3.Lerp(enterPosition, targetPosition, t);
        movementCompoent.SetPosition(warpedPosition);
        // Interpolate rotation
        Quaternion warpedRotation = Quaternion.Slerp(enterRotation, targetRotation, t);
        movementCompoent.SetRotation(warpedRotation);
    }
}
