using System;
using UnityEngine;

public class PlayerDolphinDiveStateNodeLeaf : PlayerStateNodeLeaf
{
    public bool isPassingJump;
    public virtual float jumpOutTime { get => .3f; }
    public float timer;

    protected Vector3 jumpDir;

    protected virtual float stallMinimumTime { get => .5f; }
    public float stallTimeCountDown { get; protected set; }

    protected PlayerMovement playerMovement => this.player.playerMovement;
    protected virtual float jumpVelocuty { get => 5; }
    protected virtual float jumpVerticalVelocuty { get => 3f; }
    public PlayerDolphinDiveStateNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {

        this.stallTimeCountDown = this.stallMinimumTime;

        this.timer = 0;

        this.isPassingJump = false;

        this.CalculateJumpOutDir();
        this.playerMovement.SetProneDir(this.jumpDir);

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
        return (this.stallTimeCountDown <= 0) && this.playerMovement.characterController.isGrounded;
    }
    public override void UpdateNode()
    {
        this.timer += Time.deltaTime;

        this.UpdateJumpOut();

        this.UpdateRotation();

        this.UpdateStall();

    

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    protected virtual void UpdateJumpOut()
    {
        if (this.timer >= this.jumpOutTime
            && this.isPassingJump == false)
        {

            this.playerMovement.AddForcePushVelocityChange(this.jumpDir * this.jumpVelocuty, IMotionImplusePushAble.PushMode.IgnoreMomentum,.05f);
            this.playerMovement.characterController.PushForceUp(this.jumpVerticalVelocuty,.05f);

            this.isPassingJump = true;
        }
    }
    protected virtual void UpdateRotation()
    {
        this.playerMovement.SetRotateToDirWorldSlerp(this.player.cinemachineCamera.targetDir, 1);
    }
    protected virtual void UpdateStall()
    {
        if (this.isPassingJump
            && this.stallTimeCountDown > 0)
        {
            this.stallTimeCountDown -= Time.deltaTime;
        }
    }

    protected virtual void CalculateJumpOutDir()
    {
        this.jumpDir = (this.player.inputMoveDir_World.normalized + this.playerMovement.curMoveVelocity_World.normalized).normalized;
    }
}
