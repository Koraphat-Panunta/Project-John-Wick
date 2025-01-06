using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprintStateNode : EnemyStateLeafNode
{
    Animator animator;
    RotateObjectToward objectToward;
    public EnemySprintStateNode(Enemy enemy) : base(enemy)
    {
        animator = enemy.animator;
        objectToward = new RotateObjectToward();
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        animator.SetBool("IsSprinting", true);
        animator.speed = 1f;

        base.Enter();
    }

    public override void Exit()
    {
        animator.SetBool("IsSprinting", false);
        animator.speed = 1f;

        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(enemy.isPainTrigger)
            return true;

        if(enemy.isSprint == false)
            return true;

        return false;

    }

    public override bool PreCondition()
    {
        if(enemy.isSprint)
            return true;

        return false;
    }

    public override void Update()
    {

        objectToward.RotateToward(enemy.lookRotation, enemy.gameObject, enemy.rotateSpeed);

        base.Update();
    }
}
