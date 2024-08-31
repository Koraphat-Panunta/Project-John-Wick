using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : CharacterState 
{
    private Animator characterAnimator;
    private PlayerController playerController;
    private PlayerStateManager playerStateManager;
    public MoveState(Player player)
    {
        base.player = player;
        this.characterAnimator = player.animator;
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
        player.playerAnimation.AnimateMove(player.playerMovement);
    }
    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        InputPerformed();
        //characterAnimator.SetFloat("ForBack_Ward", stateManager.playerController.input.movement.ReadValue<Vector2>().y);
        //characterAnimator.SetFloat("Side_LR", stateManager.playerController.input.movement.ReadValue<Vector2>().x);
        playerMovement.OMNI_DirMovingCharacter();
        playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
    }

    protected float rotationSpeed = 5.0f;
    protected void RotateTowards(Vector3 direction)
    {
        // Ensure the direction is normalized
        direction.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        direction.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (direction != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            base.player.gameObject.transform.rotation = Quaternion.Slerp(base.player.gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    protected override void InputPerformed()
    {
        if (playerController.input.movement.phase == InputActionPhase.Canceled)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.idle);
        }
        if (playerController.input.sprint.phase == InputActionPhase.Started||playerController.input.sprint.phase == InputActionPhase.Performed)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.sprint);
        }
        base.InputPerformed();
    }
    


}
