using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIdleNode : PlayerStateNodeLeaf
{
    public PlayerStandIdleNode(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.StandIdle);
        base.Enter();
    }
    public override void FixedUpdateNode()
    {
        PlayerMovement playerMovement = base.player.playerMovement;

        playerMovement.MoveToDirWorld(Vector3.zero,player.breakDecelerate,player.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        InputPerformed();
        base.UpdateNode();
    }

    private void InputPerformed()
    {
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
