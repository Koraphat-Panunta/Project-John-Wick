
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPath
{
    public List<Vector3> _markPoint = new List<Vector3>();
    public Vector3 targetPos;
    public Vector3 targetAnchor;
    public EnemyPath()
    {

    }
    public void UpdateTargetPos(Vector3 tarPos)
    {
        targetPos = tarPos;
        EnemyDebuger._markPos = _markPoint;
    }
    public void GenaratePath(Vector3 target, Vector3 curPos)
    {
        UpdateTargetPos(target);
        float distance = Vector3.Distance(target, curPos);
        Vector3 _conP_1 = new Vector3(target.x + Random.Range(-distance * 0.7f, distance * 0.7f), target.y, target.z + Random.Range(-distance * 0.7f, distance * 0.7f));
        for (float T = 0; T <= 1; T = T + 0.2f)
        {
            Vector3 markPos;
            Vector3 A = Vector3.Lerp(curPos, _conP_1, T);
            Vector3 B = Vector3.Lerp(_conP_1, target, T);
            EnemyDebuger.cp1 = _conP_1;
            markPos = Vector3.Lerp(A, B, T);
            IsPositionOnNavMesh(markPos, 2f);
        }
        targetAnchor = target;
        _markPoint.Add(targetAnchor);
        Debug.Log("GenPath");
    }
    public void ResetPath()
    {
        _markPoint.Clear();
    }
    public void SetNavDestinationNext(NavMeshAgent agent)
    {
        if (_markPoint.Count > 0)
        {
            Debug.Log("Set agent");
            Vector3 nextDestination = _markPoint[0];
            agent.destination = nextDestination;
            _markPoint.RemoveAt(0);
        }
        else
        {
            Debug.Log("Clear agent");
            _markPoint.Clear();
            agent.ResetPath();
            Debug.Log("Agent still has path" + agent.destination == null);
        }
       
    }
    public void RegenaratePath(Vector3 target, Vector3 curPos, NavMeshAgent agent)
    {
        ResetPath();
        GenaratePath(target, curPos);
        SetNavDestinationNext(agent);
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
