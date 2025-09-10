using System;
using UnityEngine;
using UnityEngine.AI;

public class FindTargetInTargetZoneEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private ZoneDefine targetZone;

    public Vector3 destinate { get; private set; }
    public readonly float waitTime = 4;
    public float elapseWaitTime { get; private set; }
    protected IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision;

    public FindTargetInTargetZoneEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyDecision enemyDecision,IEnemyActionNodeManagerImplementDecision enemyActionNodeManager,ZoneDefine targetZone) 
        : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.targetZone = targetZone;
        this.enemyActionNodeManagerImplementDecision = enemyActionNodeManager;
    }
    public override void Enter()
    {
        elapseWaitTime = 0;
        RandomNewPos();
        base.Enter();
    }
    public override void UpdateNode()
    {

        if(elapseWaitTime > 0)
        {
            elapseWaitTime -= Time.deltaTime;
            return;
        }


       
        if (enemyCommandAPI.MoveToPositionRotateToward(destinate, enemy.moveMaxSpeed, 1, 0.25f))
        {
            RandomNewPos();
            elapseWaitTime = waitTime;
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
