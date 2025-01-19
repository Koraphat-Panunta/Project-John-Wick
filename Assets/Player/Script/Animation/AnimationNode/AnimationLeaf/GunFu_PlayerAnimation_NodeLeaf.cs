using UnityEngine;

public class GunFu_PlayerAnimation_NodeLeaf : PlayerAnimationNodeLeaf
{
    private Player player;

    private string hit1 = "Hit";
    private string hit2 = "Hit2GunFuNode";
    private string knockDown = "KnockDown";
    public GunFu_PlayerAnimation_NodeLeaf(PlayerAnimationManager playerAnimationManager, Animator animator) : base(playerAnimationManager, animator)
    {
        player = playerAnimationManager.player;
    }
    public override void Enter()
    {
        animator.SetLayerWeight(1, 0);
        //if (player.curPlayerActionNode == player.Hit1gunFuNode)
            animator.CrossFadeInFixedTime(hit1, 0.05f, 0, 0);
        base.Enter();
    }

    public override void Exit()
    {
        animator.SetLayerWeight(1, 1);
        base.Exit();
    }

    public override void FixedUpdate()
    {
        player.playerMovement.FreezingCharacter();
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (player.curPlayerActionNode != player.Hit1gunFuNode)
            return true;

        return false;
    }

   

    public override bool PreCondition()
    {
        if(player.curPlayerActionNode == player.Hit1gunFuNode)
            return true;

        return false;
    }

    public override void Update()
    {
        base.Update();
    }
}
