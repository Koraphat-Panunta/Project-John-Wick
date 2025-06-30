using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetection 
{
    public ObstacleDetection() { }
    public List<Vector3> GetSphereCast(float sphereRaduis, Vector3 castDir,Vector3 startPos,Vector3 destinatePos)
    {
        List<Vector3> sphereCast = new List<Vector3>();
        Vector3 curPos = startPos;
        for (float i = 0; i <= Vector3.Distance(startPos,destinatePos); i = i + sphereRaduis * 2)
        {
            Vector3 castPos = Vector3.MoveTowards(startPos, destinatePos, i);
            if (Physics.SphereCast(castPos, sphereRaduis, castDir, out RaycastHit hit, 10, LayerMask.GetMask("Default")))
            {
                sphereCast.Add(hit.point);
            }
            else 
            {
                sphereCast.Add(castPos+(castDir*10));
            }
            castPos = Vector3.MoveTowards(castPos, destinatePos, i);
        }
        return sphereCast;
    }
}
