using System;
using UnityEngine;

public class PlayerCrouch_Idle_NodeLeaf : PlayerStateNodeLeaf
{

    public PlayerCrouch_Idle_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }
    public override void Enter()
    {
        player.NotifyObserver(player,this);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        MovementCompoent playerMovement = base.player._movementCompoent;

        playerMovement.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, MoveMode.MaintainMomentum);

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
