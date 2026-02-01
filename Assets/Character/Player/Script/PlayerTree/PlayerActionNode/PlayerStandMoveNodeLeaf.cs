using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandMoveNodeLeaf : PlayerStateNodeLeaf
{
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
        PlayerMovement playerMovement = base.player._movementCompoent as PlayerMovement;

        playerMovement.UpdateMoveToDirWorld(this.player.inputMoveDir_World * this.player.StandMoveMaxSpeed, this.player.StandMoveAccelerate, MoveMode.MaintainMomentum);
        playerMovement.SetRotateToDirWorld(Camera.main.transform.forward, player.StandMoveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
