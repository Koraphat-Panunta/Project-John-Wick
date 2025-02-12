using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandMoveNode : PlayerStateNodeLeaf
{
    public PlayerStandMoveNode(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.StandMove);
        base.Enter();
    }
    public override void FixedUpdateNode()
    {
        PlayerMovement playerMovement = base.player.playerMovement;

        playerMovement.MoveToDirWorld(player.inputMoveDir_World, player.StandMoveAccelerate, player.StandMoveMaxSpeed *player.inputMoveDir_World.magnitude, IMovementCompoent.MoveMode.MaintainMomentum);
        playerMovement.RotateToDirWorld(Camera.main.transform.forward, player.StandMoveRotateSpeed);

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
