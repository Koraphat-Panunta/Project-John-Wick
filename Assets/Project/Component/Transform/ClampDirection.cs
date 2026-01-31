using UnityEngine;

public static class ClampDirection 
{
    public static Vector3 GetClampDirection(
     Vector3 referenceDir,
     Vector3 direction,
     float maxHorizontalDeg,
     float maxVerticalDeg)
    {

        //if (Vector3.Distance(poitnPos, pointingPos) > .5f)
        //    trackRate = 0;

        // Normalize input
        Vector3 dirToPoint = direction;

        // Basis: forward, right, up
        Vector3 fwd = referenceDir.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, fwd).normalized;
        Vector3 up = Vector3.Cross(fwd, right).normalized;

        // Project onto local basis (dot products give angles)
        float horizontalAngle = Mathf.Atan2(Vector3.Dot(dirToPoint, right), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg;
        float verticalAngle = (Mathf.Atan2(Vector3.Dot(dirToPoint, up), Vector3.Dot(dirToPoint, new Vector3(dirToPoint.x, 0, dirToPoint.z))) * Mathf.Rad2Deg) * -1;


        // Clamp angles
        horizontalAngle = Mathf.Clamp(horizontalAngle, -maxHorizontalDeg, maxHorizontalDeg);
        verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalDeg, maxVerticalDeg);

        // Rebuild direction from clamped angles
        Quaternion rot = Quaternion.AngleAxis(horizontalAngle, Vector3.up) *
                         Quaternion.AngleAxis(verticalAngle, right);
        Vector3 clampedDir = rot * fwd;

        return clampedDir;
    }
}
