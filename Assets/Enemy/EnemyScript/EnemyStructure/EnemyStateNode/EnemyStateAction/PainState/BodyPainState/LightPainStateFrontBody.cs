using System;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class LightPainStateFrontBody : EnemyStateLeafNode
{
    private bool animationIsPerformded = false;
    public LightPainStateFrontBody(Enemy enemy) : base(enemy)
    {
    }
    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.animator.SetTrigger("BodyHitNormalReaction");
        if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("BodyHitNormalReaction") == false)
        {
            animationIsPerformded = false;
        }
        base.Enter();
    }

    public override void Exit()
    {
       enemy._painPart = IPainState.PainPart.None;
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (enemy.isDead)
            return true;

        if (animationIsPerformded == true)
        {
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("BodyHitNormalReaction") == false 
                && enemy.animator.GetAnimatorTransitionInfo(0).IsName("Enter->" + "BodyHitNormalReaction") == false)
            {
                return true;
            }
        }
        return false;
    }

    public override bool PreCondition()
    {
        if (enemy._painPart == IPainState.PainPart.BodyFornt)
            return true;

        return false;
    }

    public override void Update()
    {
        if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("BodyHitNormalReaction"))
        {
            animationIsPerformded = true;
        }
        base.Update();
    }
}
