using System;
using System.Collections.Generic;
using UnityEngine;

public class Wait : EnemyActionLeafNode
{

    private IPatrolComponent patroler;
    PatrolPoint myPatrolpoint;
    public float waitTime { get; private set; }
    public Wait(EnemyControllerAPI enemyController) : base(enemyController)
    {
    }

    public Wait(EnemyControllerAPI enemyController,IPatrolComponent patroler, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        this.patroler = patroler;
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        this.myPatrolpoint = patroler.patrolPoints[patroler.Index];
        waitTime = 0;
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
        waitTime += Time.deltaTime;
        base.Update();
    }
}
