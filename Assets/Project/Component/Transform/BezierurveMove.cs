using System.Collections.Generic;
using UnityEngine;

public class BezierurveMove 
{
    public static Vector3 GetPointOnBezierCurve(Vector3 startPos, List<Vector3> controlPoints, Vector3 endPos, float t)
    {
        List<Vector3> points = new List<Vector3> { startPos };
        points.AddRange(controlPoints);
        points.Add(endPos);

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
    public static void MoveTransformOnBezierCurve(Transform moveTransform,Vector3 startPos, List<Vector3> controlPoints, Vector3 endPos, float t)
    {
        moveTransform.position = GetPointOnBezierCurve(startPos, controlPoints, endPos, t);
    }
}
