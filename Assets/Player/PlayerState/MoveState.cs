using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : CharacterState 
{
    protected PlayerController playerController;
    protected PlayerStateManager playerStateManager;
    public MoveState(Player player)
    {
        base.player = player;
        this.playerController = player.playerController;
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
        if (base.player.coverDetection.CheckingObstacleToward(base.player.RayCastPos.transform.position, base.player.RayCastPos.transform.forward))
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
        PlayerController.Input input = this.playerController.input;
        if (playerController.input.movement.phase == InputActionPhase.Waiting || playerController.input.movement.phase == InputActionPhase.Canceled)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.idle);
        }
        if (playerController.input.sprint.phase == InputActionPhase.Started||playerController.input.sprint.phase == InputActionPhase.Performed)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.sprint);
        }
        new WeaponInput().InputWeaponUpdate(input, player);
        if (input.swapShoulder.phase == InputActionPhase.Started || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }


}
