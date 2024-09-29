using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetection : MonoBehaviour
{
    public ObstacleDetection() { }
    public List<Vector3> GetSphereCast(float sphereRaduis, Vector3 castDir,Vector3 startPos,Vector3 destinatePos)
    {
        List<Vector3> sphereCast = new List<Vector3>();
        for (float i = 0; i <= 1; i = i + sphereRaduis * 2)
        {
            Vector3 castPos = Vector3.Lerp(startPos, destinatePos, i);
            if (Physics.SphereCast(castPos, sphereRaduis, castDir, out RaycastHit hit, 10, LayerMask.GetMask("Default")))
            {
                sphereCast.Add(hit.point);
            }
            else 
            {
                sphereCast.Add(castPos+(castDir*10));
            }
        }
        return sphereCast;
    }
}
