using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ParkourObstacleTesting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EdgeObstacleDetection edgeObstacleDetection;
    [SerializeField] private ParkourScriptableObject curParkourScriptableObject;
    [SerializeField] Transform startCastPoint;
    [Range(0.01f,1)]
    public float sphereCastRaduis;
    [Range(0,1)]
    public float sphereDistanceDifference;
    private void Awake()
    {
        edgeObstacleDetection = new EdgeObstacleDetection();
    }
    private void OnDrawGizmos()
    {
        if (edgeObstacleDetection == null)
            return;

        if(curParkourScriptableObject is VaultingParkourScriptableObject vaultingParkourScriptableObject)
        {
            DrawGizmosVaultingParkour(vaultingParkourScriptableObject);
        }
        else
        {
            DrawGizmosParkourClimb(curParkourScriptableObject);
        }
    }



    private void DrawGizmosVaultingParkour(VaultingParkourScriptableObject vaultingParkourScriptableObject)
    {
        List<Vector3> cts = new List<Vector3>();

        if (edgeObstacleDetection.GetEdgeObstaclePos(
              sphereCastRaduis
              , startCastPoint.forward
              , startCastPoint.position
              , startCastPoint.position + (Vector3.up * vaultingParkourScriptableObject.hieght)
              , sphereDistanceDifference
              , out Vector3 edgePos1
              , out List<Vector3> spheresH
              ))
        {

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(edgePos1, sphereCastRaduis * 1.5f);

            Vector3 startPos = startCastPoint.position;
            Vector3 ct1
                = edgePos1
                + startCastPoint.forward * curParkourScriptableObject.forWardControlPoint_1_offset
                + startCastPoint.up * curParkourScriptableObject.upWardControlPoint_1_offset;


            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(startPos, 0.15f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(ct1, 0.15f);
           
            cts.Add(ct1);

            if (spheresH.Count > 0)
            {
                Gizmos.color = Color.blue;
                foreach (Vector3 spherePos in spheresH)
                    Gizmos.DrawSphere(spherePos, sphereCastRaduis);
            }
        }
        else
            return;

        Vector3 startCastDown = edgePos1 + (Vector3.up * 2);
        Vector3 destinationCaswDown = startCastDown + startCastPoint.forward * vaultingParkourScriptableObject.vaultingLenght;
        Vector3 exitPos;

        if (edgeObstacleDetection.GetEdgeObstaclePos(
            sphereCastRaduis
            , Vector3.down
            , startCastDown
            , destinationCaswDown
            , sphereDistanceDifference
            , out Vector3 edgePos2
            , out List<Vector3> sphereLine))
        {
            Vector3 ct2
                   = edgePos2
                   + startCastPoint.forward * vaultingParkourScriptableObject.forwardControlPoint_2_offset
                   + startCastPoint.up * vaultingParkourScriptableObject.upWardControlPoint_2_offset;

            cts.Add(ct2);

            exitPos
                = ct2
                + startCastPoint.forward * vaultingParkourScriptableObject.forwardExitPoint_offset
                + startCastPoint.up * vaultingParkourScriptableObject.upWardExitPoint_offset;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(edgePos2, 0.15f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(ct2, 0.15f);

            Gizmos.color = Color.blue;
            if (sphereLine.Count > 0)
            {
                foreach (Vector3 spherePos in sphereLine)
                    Gizmos.DrawSphere(spherePos, sphereCastRaduis);
            }
        }
        else
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(exitPos, .15f);

        DrawBezierCurve(startCastPoint.position,cts,exitPos);
    }
    private void DrawGizmosParkourClimb(ParkourScriptableObject parkourScriptableObject)
    {
        if (edgeObstacleDetection.GetEdgeObstaclePos(
              sphereCastRaduis
              , startCastPoint.forward
              , startCastPoint.position
              , startCastPoint.position + (Vector3.up * parkourScriptableObject.hieght)
              , sphereDistanceDifference
              , out Vector3 edgePos
              , out List<Vector3> spheres
              ))
        {

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(edgePos, sphereCastRaduis * 1.5f);

            Vector3 startPos = startCastPoint.position;
            Vector3 ct1
                = edgePos
                + startCastPoint.forward * parkourScriptableObject.forWardControlPoint_1_offset
                + startCastPoint.up * parkourScriptableObject.upWardControlPoint_1_offset;
            Vector3 exit
                = edgePos
                + startCastPoint.forward * parkourScriptableObject.forwardExitPoint_offset
                + startCastPoint.up * parkourScriptableObject.upWardExitPoint_offset;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(startPos, 0.15f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(ct1, 0.15f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(exit, 0.15f);

            List<Vector3> cts = new List<Vector3>();
            cts.Add(ct1);
            DrawBezierCurve(startPos, cts, exit);
        }
        if (spheres.Count > 0)
        {
            Gizmos.color = Color.blue;
            foreach (Vector3 spherePos in spheres)
                Gizmos.DrawSphere(spherePos, sphereCastRaduis);
        }

    }
    private void DrawBezierCurve(Vector3 startPos,List<Vector3> controlPoints , Vector3 endPos)
    {
        float curveResolution = 30;

        if (controlPoints == null || controlPoints.Count == 0)
            return;

        Vector3 previousPoint = startPos;

        for (int i = 1; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 pointOnCurve = CalculateBezierPoint(t, startPos, controlPoints, endPos);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(previousPoint, pointOnCurve);
            previousPoint = pointOnCurve;
        }

        // Draw control points for debugging
        Gizmos.color = Color.yellow;
        foreach (var p in controlPoints)
        {
            Gizmos.DrawSphere(p, 0.05f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPos, 0.05f);
        Gizmos.DrawSphere(endPos, 0.05f);
    }
    Vector3 CalculateBezierPoint(float t, Vector3 start, List<Vector3> controls, Vector3 end)
    {
        List<Vector3> points = new List<Vector3> { start };
        points.AddRange(controls);
        points.Add(end);

        // Iteratively reduce the points list until we get the point on the curve
        while (points.Count > 1)
        {
            List<Vector3> newPoints = new List<Vector3>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 interpolated = Vector3.Lerp(points[i], points[i + 1], t);
                newPoints.Add(interpolated);
            }
            points = newPoints;
        }

        return points[0];
    }
}
