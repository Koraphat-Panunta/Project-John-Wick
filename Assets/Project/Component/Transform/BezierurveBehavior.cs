using System.Collections.Generic;
using UnityEngine;

public class BezierurveBehavior 
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
    public static Vector3 GetPointOnBezierCurve(Vector3 startPos, Vector3[] controlPoints, Vector3 endPos, float t)
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
    public static Vector3 GetPointOnBezierCurve(List<Vector3> controlPoints, float t)
    {
        
        while (controlPoints.Count > 1)
        {
            List<Vector3> newPoints = new List<Vector3>();
            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                Vector3 interpolated = Vector3.Lerp(controlPoints[i], controlPoints[i + 1], t);
                newPoints.Add(interpolated);
            }
            controlPoints = newPoints;
        }

        return controlPoints[0];
    }
    public static void DrawBezierCurve(Vector3 startPos, List<Vector3> controlPoints, Vector3 endPos)
    {
        float curveResolution = 30;

        if (controlPoints == null || controlPoints.Count == 0)
            return;

        Vector3 previousPoint = startPos;

        for (int i = 1; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 pointOnCurve = GetPointOnBezierCurve( startPos, controlPoints, endPos,t);

            Debug.DrawLine(previousPoint, pointOnCurve,Color.green);
            previousPoint = pointOnCurve;
        }

       
    }
    public static void DrawBezierCurve(Vector3 startPos, List<Vector3> controlPoints, Vector3 endPos, float time)
    {
        float curveResolution = 30;

        if (controlPoints == null || controlPoints.Count == 0)
            return;

        Vector3 previousPoint = startPos;

        for (int i = 1; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 pointOnCurve = GetPointOnBezierCurve(startPos, controlPoints, endPos, t);

            Debug.DrawLine(previousPoint, pointOnCurve, Color.green,time);
            previousPoint = pointOnCurve;
        }


    }
}
