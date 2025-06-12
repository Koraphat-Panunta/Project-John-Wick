using System;
using UnityEngine;

public class PlayerCrouch_Move_NodeLeaf : PlayerStateNodeLeaf
{
    public PlayerCrouch_Move_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.CrouchMove);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        PlayerMovement playerMovement = base.player.playerMovement;

        playerMovement.MoveToDirWorld(player.inputMoveDir_World, player.CrouchMoveAccelerate, player.CrouchMoveMaxSpeed * player.inputMoveDir_World.magnitude, IMovementCompoent.MoveMode.MaintainMomentum);
        playerMovement.RotateToDirWorld(Camera.main.transform.forward, player.CrouchMoveRotateSpeed);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
