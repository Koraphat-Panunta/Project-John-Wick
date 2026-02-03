using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIdleNodeLeaf : PlayerStateNodeLeaf
{
    PlayerMovement playerMovement => this.player._movementCompoent as PlayerMovement;
    public float weightMovement => Mathf.Clamp01(1-this.playerMovement.stanceRateMovement);
    public float changeStanceWeightRate = 1;
    public PlayerStandIdleNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {

        player.NotifyObserver(player, this);
        base.Enter();
    }
    public override void FixedUpdateNode()
    {
        this.playerMovement.SetStanceWeight(this.playerMovement.stanceRateMovement - this.changeStanceWeightRate * Time.fixedDeltaTime);

        Vector3 targerMove = Vector3.Lerp(this.playerMovement.curMoveVelocity_World, Vector3.zero, this.player.breakDecelerate * this.weightMovement * Time.fixedDeltaTime);

        playerMovement.UpdateMoveToDirWorld(targerMove, player.breakDecelerate * this.weightMovement, MoveMode.MaintainMomentumDirection);


        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
