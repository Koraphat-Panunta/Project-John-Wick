using System;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPainStateBackBody : EnemyStateLeafNode
{
    Animator animator;
    bool animationIsPerformded;
    public HeavyPainStateBackBody(Enemy enemy) : base(enemy)
    {
        animator = enemy.animator;  
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        animator.SetTrigger("BodyHitNormalReaction");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BodyHitNormalReaction") == false)
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
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BodyHitNormalReaction"))
        {
            animationIsPerformded = true;
        }
        if (animationIsPerformded == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BodyHitNormalReaction") == false && animator.GetAnimatorTransitionInfo(0).IsName("Enter->" + this.GetType().Name) == false)
            {
                //End(enemyState);
            }
        }
        base.Update();
    }
}
