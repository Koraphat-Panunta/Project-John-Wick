using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTheZoneEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    public ZoneDefine assignZone;
    private Vector3 destinateInZone;
    public MoveToTheZoneEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager, ZoneDefine assignZone) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
        this.assignZone = assignZone;
    }
    public override void Enter()
    {
        if(NavMesh.SamplePosition(this.assignZone.GetRandomPositionInZoneForwardZoneToPosition(enemy.transform.position,180),out NavMeshHit des,100,NavMesh.AllAreas))
            this.destinateInZone = des.position;
        else
            this.destinateInZone = assignZone.zonePosition;
            
        base.Enter();
    }
    public override void UpdateNode()
    {
        Vector3 enemyToZonePos = this.assignZone.zonePosition - enemy.transform.position;
        Vector3 enemyToDes = this.destinateInZone - enemy.transform.position;

        if (Vector3.Dot(enemyToZonePos, enemyToDes) > 5)
        {
            enemyCommandAPI.SprintToPosition(this.destinateInZone, enemy.sprintRotateSpeed);
            enemyCommandAPI.LowReady();
        }
        else
        {
            enemyCommandAPI.MoveToPosition(this.destinateInZone, enemy.moveMaxSpeed);
            enemyCommandAPI.AimDownSight(enemy.targetKnewPos, enemy.aimingRotateSpeed);
            enemyCommandAPI.NormalFiringPattern.Performing();
        }

        base.UpdateNode();
    }
}
