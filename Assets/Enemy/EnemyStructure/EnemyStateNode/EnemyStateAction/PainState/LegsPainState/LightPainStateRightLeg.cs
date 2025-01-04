using System;
using System.Collections.Generic;
using UnityEngine;

public class LightPainStateRightLeg : EnemyStateLeafNode
{
    private Animator animator;
    private bool animationIsPerformded = false;
    public LightPainStateRightLeg(Enemy enemy) : base(enemy)
    {
        animator = enemy.animator;
    }
    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        animator.SetTrigger("LegHitNormalReaction");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LegHitNormalReaction") == false)
        {
            animationIsPerformded = false;
        }
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
        if (animationIsPerformded == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("LegHitNormalReaction") == false 
                && animator.GetAnimatorTransitionInfo(0).IsName("Enter->" + "LegHitNormalReaction") == false)
            {
                return true;
            }
        }
        return false;
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LegHitNormalReaction"))
        {
            animationIsPerformded = true;
        }
    }
}
