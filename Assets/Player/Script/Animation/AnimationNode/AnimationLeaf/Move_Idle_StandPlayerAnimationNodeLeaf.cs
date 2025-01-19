using System;
using UnityEngine;

public class Move_Idle_StandPlayerAnimationNodeLeaf : PlayerAnimationNodeLeaf
{

    private string stateName = "Move/Idle";
    private int stateLayer = 0;
    public Move_Idle_StandPlayerAnimationNodeLeaf(PlayerAnimationManager playerAnimationManager, Animator animator, Func<bool> preCondition) : base(playerAnimationManager, animator, preCondition)
    {
    }

    public override void Enter()
    {
        animator.CrossFade(stateName, 0.3f, stateLayer, 0,0);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override bool IsReset()
    {
        
        return false;
            
    }

    public override void Update()
    {
        base.Update();
    }
}
