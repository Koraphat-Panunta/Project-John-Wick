using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerStateNodeLeaf
{
   

    private PlayerMovement playerMovement => player.playerMovement;

    public PlayerSprintNode(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        
    }

    private float sprintMaxSpeed => player.sprintMaxSpeed;
    private float sprintAcceletion => player.sprintAccelerate;
    private float sprintRotateSpeed => player.sprintRotateSpeed;

   
    public override void Enter()
    {
        (player.playerMovement as IMovementCompoent).CancleMomentum();
        player.playerStance = Player.PlayerStance.stand;
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Sprint);
        base.Enter();
    }
    public override void UpdateNode()
    {
        InputPerformed();

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        playerMovement.MoveToDirWorld(player.transform.forward, sprintAcceletion, sprintMaxSpeed, IMovementCompoent.MoveMode.IgnoreMomenTum);
        playerMovement.RotateToDirWorld(player.inputMoveDir_World.normalized, sprintRotateSpeed );

        base.FixedUpdateNode();
    }

    private  void InputPerformed()
    {
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
   
}
