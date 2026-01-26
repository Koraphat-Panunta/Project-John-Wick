using UnityEngine;

public static class ClampDirection 
{
    public static Vector3 GetClampDirection(
     Vector3 referenceDir,
     Vector3 direction,
     float maxHorizontalDeg,
     float maxVerticalDeg,
     Vector3 worldUp)
    {
        if (referenceDir.sqrMagnitude < 0.0001f || direction.sqrMagnitude < 0.0001f)
            return referenceDir.normalized;

        referenceDir.Normalize();
        direction.Normalize();

        // --- Build stable basis ---
        Vector3 up = worldUp;

        // Prevent degenerate cross when referenceDir is vertical
        if (Mathf.Abs(Vector3.Dot(referenceDir, up)) > 0.99f)
            up = Vector3.forward;

        Vector3 right = Vector3.Cross(up, referenceDir).normalized;
        Vector3 trueUp = Vector3.Cross(referenceDir, right).normalized;

        // --- Convert to local angles ---
        float yaw = Mathf.Atan2(
            Vector3.Dot(direction, right),
            Vector3.Dot(direction, referenceDir)
        ) * Mathf.Rad2Deg;

        float pitch = Mathf.Atan2(
            Vector3.Dot(direction, trueUp),
            Vector3.Dot(direction, Vector3.ProjectOnPlane(direction, trueUp))
        ) * Mathf.Rad2Deg;

        // --- Clamp ---
        yaw = Mathf.Clamp(yaw, -maxHorizontalDeg, maxHorizontalDeg);
        pitch = Mathf.Clamp(pitch, -maxVerticalDeg, maxVerticalDeg);

        // --- Rebuild direction ---
        Quaternion yawRot = Quaternion.AngleAxis(yaw, trueUp);
        Quaternion pitchRot = Quaternion.AngleAxis(pitch, right);

        Vector3 clampedDir = yawRot * pitchRot * referenceDir;
        return clampedDir.normalized;
    }
}
