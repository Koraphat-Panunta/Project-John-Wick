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
        animator.CrossFade(stateName, 0.5f, stateLayer, 0);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override bool IsReset()
    {
        if(playerAnimationManager.playerStance != Player.PlayerStance.stand)
            return true;

        if(playerAnimationManager.isSprint)
            return true;

        if(playerAnimationManager.isTriggerDodge)
            return true;

        if(playerAnimationManager.isTriggerGunFu)
            return true;

        if(playerAnimationManager.isTriggerMantle)
            return true;

        if(playerAnimationManager.isGround == false)
            return true;

        return false;
            
    }

    public override void Update()
    {
        base.Update();
    }
}
