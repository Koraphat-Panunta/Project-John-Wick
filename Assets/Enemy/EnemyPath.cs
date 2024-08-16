
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPath
{
    public List<Vector3> _markPoint = new List<Vector3>();
    public Vector3 targetPos;
    public Vector3 targetAnchor;
    public Vector3 _curPos;
    public NavMeshAgent agent;
    public EnemyPath(NavMeshAgent agent)
    {
        this.agent = agent;
    }
    public void UpdateTargetPos(Vector3 tarPos,Vector3 CurPos)
    {
        targetPos = tarPos;
        _curPos = CurPos;
        EnemyDebuger._markPos = _markPoint;
        SetNavDestinationNext();
        if (Vector3.Distance(targetAnchor, targetPos) > 6 && _markPoint.Count>0)
        {
            RegenaratePath();
        }
    }
    public void GenaratePath(Vector3 target, Vector3 curPos)
    {
        targetAnchor = target;
        UpdateTargetPos(target,curPos);
        float distance = Vector3.Distance(target, curPos);
        Vector3 _conP_1 = Vector3.Lerp(curPos, target, 0.5f);
        Vector3 dir = (target - curPos).normalized;
        dir = Quaternion.AngleAxis(90,Vector3.up)*dir;
        dir = dir * Random.Range(-6, 6);
        _conP_1 = _conP_1 + dir;
        for (float T = 0; T <= 1; T = T + 0.2f)
        {
            Vector3 markPos;
            Vector3 A = Vector3.Lerp(curPos, _conP_1, T);
            Vector3 B = Vector3.Lerp(_conP_1, target, T);
            EnemyDebuger.cp1 = _conP_1;
            markPos = Vector3.Lerp(A, B, T);
            IsPositionOnNavMesh(markPos, 2f);
        }
        SetNavDestinationNext();
        Debug.Log("GenPath");
    }
    public void ResetPath()
    {
        _markPoint.Clear();
        agent.ResetPath();
        
    }
    private void SetNavDestinationNext()
    {
        if (Vector3.Distance(_curPos, agent.destination) <= agent.radius)
        {
            if (_markPoint.Count > 0)
            {
                Vector3 nextDestination = _markPoint[0];
                agent.destination = nextDestination;
                _markPoint.RemoveAt(0);
            }
            else if (agent.hasPath)
            {
                _markPoint.Clear();
                agent.ResetPath();
            }
        }
       
    }
    public void RegenaratePath()
    {
        ResetPath();
        GenaratePath(targetPos,this._curPos);
        SetNavDestinationNext();
    }
    private void IsPositionOnNavMesh(Vector3 position, float maxDistance)
    {
        NavMeshHit hit;
        // Check if the position is on the NavMesh within the specified maxDistance
        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
        {
            // If hit.position is within the maxDistance range and hit is valid, return true
            _markPoint.Add(hit.position);
        }
    }
       

}
