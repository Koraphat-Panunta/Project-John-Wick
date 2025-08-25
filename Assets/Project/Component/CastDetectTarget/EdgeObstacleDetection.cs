using System.Collections.Generic;
using UnityEngine;

public static class EdgeObstacleDetection 
{
    public static bool GetEdgeObstaclePos(float sphereRaduis,float distance, Vector3 castDir, Vector3 startPos, Vector3 destinatePos, float difDistance, out Vector3 edgePos,out List<Vector3> sphere)
    {
        sphere = new List<Vector3>();
        List<Vector3> sphereCast = ObstacleDetection.GetSphereCast(sphereRaduis,distance, castDir, startPos, destinatePos);
        sphere = sphereCast;

        edgePos = new Vector3();

        for (int i = 0; i < sphereCast.Count; i++)
        {
            if (i >= sphereCast.Count - 1) 
                return false;

            if (Vector3.Distance(sphereCast[i], sphereCast[i + 1]) >= difDistance)
            {
                edgePos = sphereCast[i];
                break;
            }
        }
        return true;

    }
    public static bool GetEdgeObstaclePos(float sphereRaduis,float distance, Vector3 castDir, Vector3 startPos, Vector3 destinatePos, float difDistance, out Vector3 edgePos)
    {
       return GetEdgeObstaclePos(sphereRaduis, distance, castDir, startPos,destinatePos, difDistance, out edgePos,out List<Vector3> sphere);

    }
}
