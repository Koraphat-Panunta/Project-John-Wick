
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CurvePath
{
    public List<Vector3> _markPoint = new List<Vector3>();
    public Queue<Vector3> _curvePoint = new Queue<Vector3>();   
    public Vector3 targetPos;
    public Vector3 targetAnchor;
    public Vector3 _curPos;

    public CurvePath()
    {

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
        Vector3 _conP_1 = Vector3.Lerp(curPos, target, 0.5f);
        Vector3 dir = (target - curPos).normalized;
        dir = Quaternion.AngleAxis(90,Vector3.up)*dir;
        dir = dir * Random.Range(Random.Range(-8,-5), Random.Range(5,8));
        _conP_1 = _conP_1 + dir;
        for (float T = 0; T <= 1; T = T + 0.2f)
        {
            Vector3 markPos;
            Vector3 A = Vector3.Lerp(curPos, _conP_1, T);
            Vector3 B = Vector3.Lerp(_conP_1, target, T);
            markPos = Vector3.Lerp(A, B, T);
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
