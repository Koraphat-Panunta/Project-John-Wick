
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
        for(int i = 0; i < _markPoint.Count-1; i++)
        {
            Debug.DrawLine(_markPoint[i],_markPoint[i+1]);
        }
    }
    public void GenaratePath(Vector3 target, Vector3 curPos)
    {
        UpdateTargetPos(target);
        float distance = Vector3.Distance(target, curPos);
        Vector3 _conP_1 = new Vector3(target.x + Random.Range(-distance * 0.5f, distance * 0.5f), target.y, target.z + Random.Range(-distance * 0.5f, distance * 0.5f));
        for (float T = 0; T <= 1; T = T + 0.2f)
        {
            Vector3 markPos;
            Vector3 A = Vector3.Lerp(curPos, _conP_1, T);
            Vector3 B = Vector3.Lerp(_conP_1, target, T);
            markPos = Vector3.Lerp(A, B, T);
            if (IsPositionOnNavMesh(markPos, 0.5f))
            {
                _markPoint.Add(markPos);
            }
        }
        targetAnchor = target;
    }
    public void ResetPath()
    {
        _markPoint.Clear();
    }
    public void SetNavDestinationNext(NavMeshAgent agent)
    {
        if (!agent.hasPath || agent.remainingDistance < agent.stoppingDistance)
        {
            if (_markPoint.Count > 0)
            {
                Vector3 nextDestination = _markPoint[0];
                if (agent.SetDestination(nextDestination))
                {
                    _markPoint.RemoveAt(0);
                }
            }
            else
            {
                agent.SetDestination(targetPos);
            }
        }
    }
    public void RegenaratePath(Vector3 target, Vector3 curPos, NavMeshAgent agent)
    {
        ResetPath();
        GenaratePath(target, curPos);
        SetNavDestinationNext(agent);
    }
    private bool IsPositionOnNavMesh(Vector3 position, float maxDistance)
    {
        NavMeshHit hit;
        // Check if the position is on the NavMesh within the specified maxDistance
        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
        {
            // If hit.position is within the maxDistance range and hit is valid, return true
            return hit.position == position || Vector3.Distance(hit.position, position) <= maxDistance;
        }
        return false; // If not on NavMesh
    }

}
