using System;
using System.Collections.Generic;
using UnityEngine;

public class Sprint_PlayerAnimationNodeLeaf : PlayerAnimationNodeLeaf
{
    private string stateName = "Sprint";
    private int stateLayer = 0;
    public Sprint_PlayerAnimationNodeLeaf(PlayerAnimationManager playerAnimationManager, Animator animator, Func<bool> preCondition) : base(playerAnimationManager, animator, preCondition)
    {
    }

   
    public override void Enter()
    {
        animator.CrossFade(stateName, 0.3f, stateLayer,0);
    }

    public override void Exit()
    {

    }
    public override bool IsReset()
    {
        

        return false;
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

   
}
