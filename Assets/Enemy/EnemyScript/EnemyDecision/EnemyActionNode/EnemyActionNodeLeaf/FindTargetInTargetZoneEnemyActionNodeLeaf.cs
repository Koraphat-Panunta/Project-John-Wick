using System;
using UnityEngine;
using UnityEngine.AI;

public class FindTargetInTargetZoneEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private ZoneDefine targetZone;

    public Vector3 destinate { get; private set; }
    public readonly float waitTime = 4;
    public float elapseWaitTime { get; private set; }

    public FindTargetInTargetZoneEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager,ZoneDefine targetZone) 
        : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
        this.targetZone = targetZone;
    }
    public override void Enter()
    {
        elapseWaitTime = 0;
        RandomNewPos();
        base.Enter();
    }
    public override void UpdateNode()
    {
        if (enemyCommandAPI.MoveToPosition(destinate, enemy.moveMaxSpeed, true, 0.25f))
        {
            elapseWaitTime += Time.deltaTime;
            if (elapseWaitTime >= waitTime)
            {
                RandomNewPos();
                elapseWaitTime = 0;
            }
        }

        base.UpdateNode();
    }

    public void RandomNewPos()
    {
        if (NavMesh.SamplePosition(targetZone.GetRandomPositionInZone(), out NavMeshHit des, 100, NavMesh.AllAreas))
            destinate = des.position;
        else
            destinate = enemy.transform.position;
    }
}
