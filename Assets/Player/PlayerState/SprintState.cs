using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprintState : CharacterState
{
    Transform cameraTrans;
    private Animator characterAnimator;
    private PlayerController playerController;
    private PlayerStateManager playerStateManager;
    public SprintState(Player player)
    {
        base.player = player;
        this.characterAnimator = player.animator;
        this.playerController = player.playerController;
        this.playerStateManager = player.playerStateManager;
    }
    public override void EnterState()
    {
        if(cameraTrans == null)
        {
            cameraTrans = Camera.main.transform;
        }
        characterAnimator.SetBool("IsSprinting", true);
    }

    public override void ExitState()
    {
        characterAnimator.SetBool("IsSprinting", false);
    }

    public override void FrameUpdateState(PlayerStateManager stateManager)
    {

    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        InputPerformed();
        RotateTowards(TransformDirectionObject(new Vector3(playerController.input.movement.ReadValue<Vector2>().x,0,playerController.input.movement.ReadValue<Vector2>().y),Camera.main.transform.forward));
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
            base.player.transform.rotation = Quaternion.Slerp(base.player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private Vector3 TransformDirectionObject(Vector3 dirWolrd,Vector3 dirObjectLocal)
    {
        float zeta;
        
        Vector3 Direction;
        zeta = Mathf.Atan2(dirObjectLocal.z , dirObjectLocal.x)-Mathf.Deg2Rad*90;
        Direction.x = dirWolrd.x*Mathf.Cos(zeta)-dirWolrd.z*Mathf.Sin(zeta);
        Direction.z = dirWolrd.x*Mathf.Sin(zeta)+dirWolrd.z*Mathf.Cos(zeta);
        Direction.y = 0;
        
        return Direction;
    }
    protected override void InputPerformed()
    {
        if (playerController.input.sprint.phase == InputActionPhase.Canceled|| playerController.input.sprint.phase == InputActionPhase.Waiting)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.move);
        }
        base.InputPerformed();
    }


}
