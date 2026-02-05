using System;
using UnityEngine;

public class PlayerLandingStandStateNodeLeaf : PlayerStateNodeLeaf
{
    protected PlayerMovement playerMovement;
    protected float landingTimeComplete;
    protected float landingTimeEixt;
    protected float timer;
    public PlayerLandingStandStateNodeLeaf(
        Player player
        ,PlayerMovement playerMovement
        ,float landingTimeComplete
        , float landingTimeExit
        , Func<bool> preCondition) 
        : base(player, preCondition)
    {
        this.playerMovement = playerMovement;
        this.landingTimeComplete = landingTimeComplete;
        this.landingTimeEixt = landingTimeExit;
    }
    public override void Enter()
    {
        this.timer = 0;
        this.playerMovement.CancleMomentum();
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.timer += Time.deltaTime;
        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if(this.player.isDead)
            return true;

        if(this.IsComplete()
            && this.player.inputMoveDir_World.magnitude > 0)
            return true;

        if(this.timer >= this.landingTimeEixt)
            return true;

        return false;
    }
    public override bool IsComplete()
    {
        return this.timer >= this.landingTimeComplete;
    }
}
