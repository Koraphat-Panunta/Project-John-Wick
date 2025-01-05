using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandIdleStateNode : EnemyStateLeafNode
{
    public EnemyStandIdleStateNode(Enemy enemy) : base(enemy)
    {
    }

    public EnemyStandIdleStateNode(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
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
        Animator animator = enemy.animator;

        animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), 0, 2 * Time.deltaTime));
        animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), 0, 2 * Time.deltaTime));

        enemy.transform.rotation = enemy.rotating;
        base.Update();
    }
}
