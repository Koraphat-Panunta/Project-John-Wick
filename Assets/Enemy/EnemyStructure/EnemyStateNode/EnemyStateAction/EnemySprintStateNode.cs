using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprintStateNode : EnemyStateLeafNode
{
    Animator animator;
    public EnemySprintStateNode(Enemy enemy) : base(enemy)
    {
        animator = enemy.animator;
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

        enemy.transform.rotation = enemy.rotating;
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
        base.Update();
    }
}
