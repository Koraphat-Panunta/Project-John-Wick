
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveCurvePath
{
    public List<Vector3> _markPoint = new List<Vector3>();
    public Queue<Vector3> _curvePoint = new Queue<Vector3>();   
    public Vector3 targetPos;
    public Vector3 targetAnchor;
    public Vector3 _curPos;

    private float minCurve;
    private float maxCurve;
    public EnemyMoveCurvePath(float minCurve, float maxCurve)
    {
        this.minCurve = minCurve;
        this.maxCurve = maxCurve;
    }
    public void AutoRegenaratePath(Vector3 tarPos,Vector3 CurPos,float distanceRePath)
    {
        targetPos = tarPos;
        _curPos = CurPos;

        if (Vector3.Distance(targetAnchor, tarPos) > 2)
        {
            RegenaratePath(tarPos,CurPos);
        }
    }
    public void GenaratePath(Vector3 target, Vector3 curPos)
    {
        targetPos = target;
        targetAnchor = target;
        _curPos = curPos;
        //
        float distance = Vector3.Distance(target, curPos);
       
        Vector3 dir = (target - curPos).normalized;
        dir = Quaternion.AngleAxis(90,Vector3.up)*dir;

        Vector3[] cp = new Vector3[3];
        if(Random.Range(0,1) > 0)
            dir = dir * Random.Range(minCurve, maxCurve);
        else
            dir = dir * Random.Range(-minCurve,-maxCurve);

        cp[0] = Vector3.Lerp(curPos, target, 0) + dir;
        if (Random.Range(0, 1) > 0) 
        {
            dir = dir * Random.Range(minCurve, maxCurve);
            cp[1] = Vector3.Lerp(curPos, target, 0.5f) + dir;
            dir = dir * Random.Range(minCurve, maxCurve);
            cp[2] = Vector3.Lerp(curPos, target, 1) + dir;
        }

        else
        {
            dir = dir * Random.Range(-minCurve, -maxCurve);
            cp[1] = Vector3.Lerp(curPos, target, 0.5f) + dir;
            dir = dir * Random.Range(-minCurve, -maxCurve);
            cp[2] = Vector3.Lerp(curPos, target, 1) + dir;
        }
       

        
        for (float T = 0; T <= 1; T = T + 0.2f)
        {
            Vector3 markPos;

            markPos = BezierurveBehavior.GetPointOnBezierCurve(_curPos, cp.ToList<Vector3>(), target, T);

            IsPositionOnNavMesh(markPos, 2f);
        }
    } 
   
    public void ResetPath()
    {
        if (_markPoint.Count > 0)
        {
            _curvePoint.Clear();
            _markPoint.Clear();
        }
    }
    public void RegenaratePath(Vector3 targetPos,Vector3 _curPos)
    {
        ResetPath();
        GenaratePath(targetPos, _curPos);
    }
    private void IsPositionOnNavMesh(Vector3 position, float maxDistance)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
        {
            _markPoint.Add(hit.position);
            _curvePoint.Enqueue(hit.position);
        }
    }
       

}
