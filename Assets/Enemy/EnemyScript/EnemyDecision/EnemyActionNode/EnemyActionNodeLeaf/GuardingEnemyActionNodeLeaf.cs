using System;
using UnityEngine;
using UnityEngine.AI;

public class GuardingEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    public ZoneDefine guardingZone { get; private set; }
    public Vector3 destinate { get; private set; }
    public float waitiming { get; private set; } 
    public GuardingEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
        guardingZone = new ZoneDefine(enemy.transform.position, 3);
    }
    public override void Enter()
    {
       RandomNewPos();
        waitiming = 3;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
       
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override void UpdateNode()
    {
        if (enemyCommandAPI.MoveToPosition(destinate, enemy.moveMaxSpeed, true, 0.25f))
        {
            waitiming -= Time.deltaTime;
            if (waitiming <= 0)
            {
                RandomNewPos();
                waitiming = 3;
            }
        }

        base.UpdateNode();
    }

    public void RandomNewPos()
    {
        if (NavMesh.SamplePosition(guardingZone.GetRandomPositionInZone(), out NavMeshHit des, 100, NavMesh.AllAreas))
            destinate = des.position;
        else
            destinate = enemy.transform.position;
    }
}
