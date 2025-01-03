using System;
using System.Collections.Generic;
using UnityEngine;

public class BackToCover : EnemyActionLeafNode
{
    private NormalFiringPattern firingPattern;
    public BackToCover(EnemyControllerAPI enemyController) : base(enemyController)
    {
    }

    public BackToCover(EnemyControllerAPI enemyController, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        firingPattern = new NormalFiringPattern(enemyController);
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
        enemyController.LowReady();

        base.Update();
    }
}
