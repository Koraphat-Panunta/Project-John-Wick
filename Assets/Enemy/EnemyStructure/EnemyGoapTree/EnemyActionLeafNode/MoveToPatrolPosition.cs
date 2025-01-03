using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPatrolPosition : EnemyActionLeafNode
{
    private Dictionary<Transform, float> curPatrolPoint;
    public MoveToPatrolPosition(EnemyControllerAPI enemyController,Dictionary<Transform,float> curPatrolPoint, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        this.curPatrolPoint = curPatrolPoint;
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
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
        //Vector3 moveDir = curPatrolPoint.getKe
        //enemyController.Move()
        //base.Update();
    }
}
