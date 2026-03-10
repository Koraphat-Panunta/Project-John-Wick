using System;
using UnityEngine;

public class PlayerLandingRollStateNodeLeaf : PlayerStateNodeLeaf
{
    protected PlayerMovement playerMovement;
    protected float landingTimeComplete;
    protected float landingVelocityForward;
    protected float timer;
    public PlayerLandingRollStateNodeLeaf(
        Player player
        , PlayerMovement playerMovement
        , float landingTimeComplete
        , float landingVelocityForward
        , Func<bool> preCondition)
        : base(player, preCondition)
    {
        this.playerMovement = playerMovement;
        this.landingTimeComplete = landingTimeComplete;
        this.landingVelocityForward = landingVelocityForward;
    }
    public override void Enter()
    {
        this.timer = 0;
        this.playerMovement.CancleMomentum();
        this.playerMovement.AddForcePushInstantly(this.player.transform.forward * this.landingVelocityForward, IMotionImplusePushAble.PushMode.IgnoreMomentum);
        base.Enter();
    }

    public override void FixedUpdateNode()
    {
        this.playerMovement.UpdateMoveToDirWorld(this.player.transform.forward * this.playerMovement.curMoveVelocity_World.magnitude, this.player.StandMoveAccelerate, MoveMode.IgnoreMomentumDirection);
        this.playerMovement.SetRotateToDirWorld(this.player.inputMoveDir_World.normalized, this.player.rotateSpeed);
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.timer += Time.deltaTime;
        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if (this.player.isDead)
            return true;

        if (this.IsComplete())
            return true;

        return false;
    }
    public override bool IsComplete()
    {
        return this.timer >= this.landingTimeComplete;
    }
}
