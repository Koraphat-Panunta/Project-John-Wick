using System.Collections.Generic;
using UnityEngine;

public class EdgeObstacleDetection 
{
    private ObstacleDetection obstacleDetection;
    public EdgeObstacleDetection()
    {
        obstacleDetection = new ObstacleDetection();
    }
    public bool GetEdgeObstaclePos(float sphereRaduis, Vector3 castDir, Vector3 startPos, Vector3 destinatePos, float difDistance, out Vector3 edgePos,out List<Vector3> sphere)
    {
        sphere = new List<Vector3>();
        List<Vector3> sphereCast = obstacleDetection.GetSphereCast(sphereRaduis, castDir, startPos, destinatePos);
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
    public bool GetEdgeObstaclePos(float sphereRaduis, Vector3 castDir, Vector3 startPos, Vector3 destinatePos, float difDistance, out Vector3 edgePos)
    {
        List<Vector3> sphereCast = obstacleDetection.GetSphereCast(sphereRaduis, castDir, startPos, destinatePos);

        edgePos = new Vector3();

        for (int i = 0; i < sphereCast.Count; i++)
        {
            if (i >= sphereCast.Count - 1) return false;

            if (Vector3.Distance(sphereCast[i], sphereCast[i + 1]) >= difDistance)
            {
                edgePos = sphereCast[i];
                break;
            }
        }
        return true;

    }
}
