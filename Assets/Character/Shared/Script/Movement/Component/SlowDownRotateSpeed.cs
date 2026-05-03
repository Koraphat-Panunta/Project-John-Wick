using UnityEngine;

public static class SlowDownRotateSpeed 
{
    public static float GetSlowDownRotateSpeedOnNearlyTargetRotation(Vector3 curDir,Vector3 targetDir,float slowOnAngleEualer,float rotateSpeed)
    {
        float returnRotateSpeed = rotateSpeed;

        float deltaEuler = Vector3.Angle(curDir, targetDir);

        if(deltaEuler < slowOnAngleEualer)
        {
            returnRotateSpeed = rotateSpeed * (Mathf.Abs(deltaEuler) / Mathf.Abs(slowOnAngleEualer));
        }

        return returnRotateSpeed;
    }
}
