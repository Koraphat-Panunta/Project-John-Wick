using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIdleNodeLeaf : PlayerStateNodeLeaf
{
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
        PlayerMovement playerMovement = base.player._movementCompoent as PlayerMovement;

        playerMovement.UpdateMoveToDirWorld(Vector3.zero,player.breakDecelerate,player.breakMaxSpeed, MoveMode.MaintainMomentum);
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
