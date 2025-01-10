using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : CharacterState 
{
    protected PlayerStateManager playerStateManager;
    public MoveState(Player player)
    {
        base.player = player;
        this.playerStateManager = player.playerStateManager;
    }
    public override void EnterState()
    {
        //base.characterAnimator.Play("Movement");
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdateState(PlayerStateManager stateManager)
    {
        InputPerformed();
        if (base.player.coverDetection.CheckingObstacleToward(base.player.RayCastPos.transform.position, base.player.RayCastPos.transform.forward*0.3f))
        {
            playerStateManager.move = playerStateManager.moveInCover;
            playerStateManager.ChangeState(playerStateManager.moveInCover);

        }
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Move);
    }

  

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        playerMovement.OMNI_DirMovingCharacter();
        playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
    }
    protected override void InputPerformed()
    {
        if (player.inputMoveDir_Local.magnitude<=0)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.idle);
        }
        if (player.isSprint)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.sprint);
        }
        new WeaponInput().InputWeaponUpdate(player);
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }


}
