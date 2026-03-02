using System;
using UnityEngine;

public class PlayerDolphinDiveStateNodeLeaf : PlayerStateNodeLeaf
{
    public bool isPassingJump;
    private float jumpOutTime = .1f;
    private float timer;

    private Vector3 jumpDir;

    protected PlayerMovement playerMovement => this.player.playerMovement;
    protected float jumpVelocuty = 5;
    protected float jumpVerticalVelocuty = 3f;
    public PlayerDolphinDiveStateNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {

        this.jumpDir = (this.player.inputMoveDir_World.normalized + this.playerMovement.curMoveVelocity_World.normalized).normalized;

        this.timer = 0;

        this.isPassingJump = false;

        base.Enter();
    }
    public override bool IsReset()
    {

        if(this.player.isDead)
            return true;

        if(IsComplete())
            return true;

        return false;
    }

    public override bool IsComplete()
    {
        return (this.timer > this.jumpOutTime*2f) && this.playerMovement.characterController.isGrounded;
    }
    public override void UpdateNode()
    {
        this.timer += Time.deltaTime;

        if (this.timer >= this.jumpOutTime 
            && this.isPassingJump == false)
        {

            this.playerMovement.AddForcePush(this.jumpDir * this.jumpVelocuty,IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
            this.playerMovement.characterController.PushForceUp(this.jumpVerticalVelocuty);

            this.isPassingJump = true;
        }
        else
        {
            float t = this.timer/this.jumpOutTime;
            this.playerMovement.SetRotateToDirWorldSlerp(this.jumpDir,t);
        }

        base.UpdateNode();
    }
}
