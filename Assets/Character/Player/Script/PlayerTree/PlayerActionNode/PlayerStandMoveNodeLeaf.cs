using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandMoveNodeLeaf : PlayerStateNodeLeaf
{
    PlayerMovement playerMovement => this.player._movementCompoent as PlayerMovement;
    public float moveStanceWeight => 1 - this.playerMovement.stanceRateMovement;
    public float changeStanceWeightRate = 3;

    
    public PlayerStandMoveNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        this.player.NotifyObserver(player, this);
        base.Enter();
    }
    public override void FixedUpdateNode()
    {
        

        this.playerMovement.SetStanceWeight(this.playerMovement.stanceRateMovement - this.changeStanceWeightRate * Time.fixedDeltaTime);

        this.playerMovement.UpdateMoveToDirWorld(this.player.inputMoveDir_World * this.player.StandMoveMaxSpeed, this.player.StandMoveAccelerate * this.moveStanceWeight, MoveMode.MaintainMomentum);
        this.playerMovement.SetRotateToDirWorld(Camera.main.transform.forward, this.player.rotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
