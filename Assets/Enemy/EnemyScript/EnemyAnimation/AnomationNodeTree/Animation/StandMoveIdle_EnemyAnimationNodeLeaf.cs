using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class StandMoveIdle_EnemyAnimationNodeLeaf : EnemyAnimationNodeLeaf
{
    private string stateName = "Move/Idle";
    private int stateLayer = 0;

    public StandMoveIdle_EnemyAnimationNodeLeaf(EnemyAnimationManager enemyAnimation) : base(enemyAnimation)
    {

    }

    public override List<EnemyAnimationNode> childNode { get ; set ; }
    protected override Func<bool> preCondidtion { get ; set ; }
    

    public override void FixedUpdate()
    {
    }
    public override void Enter()
    {
        animator.CrossFade(stateName, 0.5f, stateLayer, 0);
    }
    public override void Exit()
    {

    }
    public override bool IsReset()
    {
        if (enemyAnimation.enemyStance != IMovementCompoent.Stance.Stand)
            return true;

        if(enemyAnimation.isSprint)
            return true;

        if(enemyAnimation.enemy._isPainTrigger)
            return true;

        return false;   
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {

    }
}
