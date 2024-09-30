using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWarping 
{
    public MovementWarping()
    {

    }
    public void Warping(GameObject obj,Vector3 DesPos,Vector3 offset,float lerpNum)
    {
        Debug.Log("warping " +lerpNum);
        Vector3 finalDes = DesPos+ offset;
        obj.transform.position = Vector3.Lerp(obj.transform.position, finalDes, lerpNum);
    }
    public IEnumerator WarpingByCoroutine(GameObject obj,Vector3 DesPos,Vector3 offset,float speedLerp) 
    {
        float i = 0;
        Vector3 finalDes = DesPos + offset;
        while (i <= 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, finalDes,i);
            i += Time.deltaTime * speedLerp;
            yield return null;
        }
    }
}
