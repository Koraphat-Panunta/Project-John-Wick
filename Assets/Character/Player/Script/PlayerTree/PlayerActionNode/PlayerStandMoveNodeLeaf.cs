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
        player.NotifyObserver(player, this);
        base.Enter();
    }
    public override void FixedUpdateNode()
    {
        PlayerMovement playerMovement = base.player._movementCompoent as PlayerMovement;

        playerMovement.UpdateMoveToDirWorld(player.inputMoveDir_World, player.StandMoveAccelerate, player.StandMoveMaxSpeed * player.inputMoveDir_World.magnitude, MoveMode.MaintainMomentum);
        playerMovement.SetRotateToDirWorld(Camera.main.transform.forward, player.StandMoveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
