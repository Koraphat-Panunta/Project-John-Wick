using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPatrolPosition : EnemyActionLeafNode
{
    List<PatrolPoint> patrolPoints;
    private PatrolPoint myPatrolPoint;
    private IPatrolComponent patroler;

    public MoveToPatrolPosition(EnemyControllerAPI enemyController, IPatrolComponent patroler, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        this.patroler = patroler;
        this.patrolPoints = patroler.patrolPoints;
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        myPatrolPoint = patrolPoints[patroler.Index];
        enemy.agent.SetDestination(myPatrolPoint.patrolTrans.position);
        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        Vector3 dir = enemy.agent.steeringTarget - enemy.transform.position;
        enemyController.Move(dir, 1);

        enemyController.RotateToPos(enemy.agent.steeringTarget,7);

        base.Update();
    }
}
