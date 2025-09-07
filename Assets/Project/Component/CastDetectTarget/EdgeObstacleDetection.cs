using System.Collections.Generic;
using UnityEngine;

public static class EdgeObstacleDetection 
{
    public static bool GetEdgeObstaclePos(float sphereRaduis,float distance, Vector3 castDir, Vector3 startPos, Vector3 destinatePos, float difDistance, out Vector3 edgePos,out List<Vector3> sphere)
    {
        sphere = new List<Vector3>();
        List<Vector3> sphereCast = ObstacleDetectionSurface.GetSphereCast(sphereRaduis,distance, castDir, startPos, destinatePos);
        sphere = sphereCast;

        edgePos = new Vector3();

        for (int i = 0; i < sphereCast.Count; i++)
        {
            if (i >= sphereCast.Count - 1) 
                return false;

            if (Vector3.Distance(sphereCast[i], sphereCast[i + 1]) >= difDistance)
            {
                Vector3 desStartDir = (destinatePos - startPos).normalized;
                Vector3 secondCastCheck = sphereCast[i] + (desStartDir * sphereRaduis * 2) + (castDir * sphereRaduis);
                if (Physics.SphereCast(secondCastCheck, sphereRaduis, desStartDir * -1, out RaycastHit hit, sphereRaduis * 2.5f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
                    edgePos = hit.point;
                else
                    edgePos = sphereCast[i];
                return true;
            }
        }
        return true;

    }
    public static bool GetEdgeObstaclePos(float sphereRaduis,float distance, Vector3 castDir, Vector3 startPos, Vector3 destinatePos, float difDistance, out Vector3 edgePos)
    {
       return GetEdgeObstaclePos(sphereRaduis, distance, castDir, startPos,destinatePos, difDistance, out edgePos,out List<Vector3> sphere);

    }
    public static bool GetEdgeObstaclePos(
        float sphereRadius,
        float distance,
        Vector3 castDir,
        Vector3 startPos,
        Vector3 destinatePos,
        float difDistance,
        bool onlyForward,
        out Vector3 edgePos,
        out List<Vector3> sphere
        )
    {
        sphere = new List<Vector3>();
        List<Vector3> sphereCast = ObstacleDetectionSurface.GetSphereCast(sphereRadius, distance, castDir, startPos, destinatePos);
        sphere = sphereCast;

        edgePos = new Vector3();

        for (int i = 0; i < sphereCast.Count - 1; i++)
        {
            float gap = Vector3.Distance(sphereCast[i], sphereCast[i + 1]);
            if (gap >= difDistance)
            {
                Vector3 edgeDirection = (sphereCast[i + 1] - sphereCast[i]).normalized;
                float dot = Vector3.Dot(edgeDirection, castDir.normalized);

                bool isForward = dot > 0;

                if ((onlyForward && isForward) || (!onlyForward && !isForward))
                {
                    Vector3 desStartDir = (destinatePos - startPos).normalized;
                    Vector3 secondCastCheck = sphereCast[i] + (desStartDir * sphereRadius * 2) + (castDir.normalized * sphereRadius);
                    Debug.DrawRay(secondCastCheck, desStartDir * -1, Color.red);
                    if (Physics.SphereCast(secondCastCheck,sphereRadius, desStartDir * -1, out RaycastHit hit, sphereRadius * 2.5f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
                        edgePos = hit.point;
                    else
                        edgePos = sphereCast[i];
                    return true;
                }
            }
        }

        return false;
    }
    public static bool GetEdgeObstaclePos(
       float sphereRadius,
       float distance,
       Vector3 castDir,
       Vector3 startPos,
       Vector3 destinatePos,
       float difDistance,
       bool onlyForward,
       out Vector3 edgePos
       )
    {
        return GetEdgeObstaclePos(sphereRadius, distance, castDir, startPos, destinatePos, difDistance, onlyForward, out edgePos, out List<Vector3> sphere);
    }
}
