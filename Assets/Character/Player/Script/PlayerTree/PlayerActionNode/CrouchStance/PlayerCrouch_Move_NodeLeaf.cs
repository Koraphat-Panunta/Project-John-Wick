using System;
using UnityEngine;

public class PlayerCrouch_Move_NodeLeaf : PlayerStateNodeLeaf
{
    public PlayerCrouch_Move_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        MovementCompoent playerMovement = base.player._movementCompoent;

        playerMovement.MoveToDirWorld(player.inputMoveDir_World, player.CrouchMoveAccelerate, player.CrouchMoveMaxSpeed * player.inputMoveDir_World.magnitude, MoveMode.MaintainMomentum);
        playerMovement.RotateToDirWorld(Camera.main.transform.forward, player.CrouchMoveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
