using System;
using System.Collections.Generic;
using UnityEngine;

public class Idle_and_Aim : EnemyActionLeafNode
{
    private RotateObjectToward rotateObject;

    public Idle_and_Aim(EnemyCommandAPI enemyController) : base(enemyController)
    {
        rotateObject = new RotateObjectToward();
    }

    public Idle_and_Aim(
        EnemyCommandAPI enemyController, 
        Func<bool> preCondition,
        Func<bool> isReset) 
        : base(enemyController, preCondition, isReset)
    {

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
    public override void Update()
    {
        enemyController.Freez();

        enemyController.AimDownSight();

        enemyController.RotateToPosition(enemy.targetKnewPos,7);

        base.Update();
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
}
