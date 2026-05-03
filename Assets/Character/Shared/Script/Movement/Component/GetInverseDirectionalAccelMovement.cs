using UnityEngine;

public static class GetInverseDirectionalAccelMovement 
{
    public static float GetInverseDirectionalAccel(Vector3 inputDir,Vector3 curDir,float changeDirAccelRate)
    {
        float returnRate = 1;

        float dot = Mathf.Abs(Mathf.Clamp(Vector3.Dot(inputDir, curDir),-1,0));

        returnRate = Mathf.Lerp(1, changeDirAccelRate,dot);

        return returnRate;
    }
}
