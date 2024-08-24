using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprintState : CharacterState
{
     Transform cameraTrans;
    public SprintState(PlayerStateManager playerStateManager)
    {
        base.playerController = playerStateManager.playerController;
        base.Character = playerStateManager.gameObject;
        base.StateManager = playerStateManager;
        base.characterAnimator = playerStateManager.PlayerAnimator;
    }
    public override void EnterState()
    {
        if(cameraTrans == null)
        {
            cameraTrans = Camera.main.transform;
        }
        base.characterAnimator.SetBool("IsSprinting", true);
    }

    public override void ExitState()
    {
        base.characterAnimator.SetBool("IsSprinting", false);
    }

    public override void FrameUpdateState(PlayerStateManager stateManager)
    {

    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        InputPerformed();
        RotateTowards(TransformDirectionObject(new Vector3(base.playerController.movementInput.x,0,base.playerController.movementInput.y),cameraTrans.forward));
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
            base.Character.transform.rotation = Quaternion.Slerp(base.Character.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    Vector3 CameraVector;
    Vector3 Input;
    Vector3 InputOfCam;
    float Zeta;
    private Vector3 TransformDirectionObject(Vector3 dirWolrd,Vector3 dirObjectLocal)
    {
        float zeta;
        CameraVector = dirObjectLocal.normalized;
        Input = dirWolrd;
        Vector3 Direction;
        zeta = Mathf.Atan2(dirObjectLocal.z , dirObjectLocal.x)-Mathf.Deg2Rad*90;
        Direction.x = dirWolrd.x*Mathf.Cos(zeta)-dirWolrd.z*Mathf.Sin(zeta);
        Direction.z = dirWolrd.x*Mathf.Sin(zeta)+dirWolrd.z*Mathf.Cos(zeta);
        Direction.y = 0;
        InputOfCam = Direction;
        Zeta = Mathf.Rad2Deg*zeta;
        return Direction;
    }
    protected override void InputPerformed()
    {
        if (base.playerController.sprintInputX.phase.IsInProgress() == false)
        {
            base.StateManager.ChangeState(base.StateManager.move);
        }
        base.InputPerformed();
    }


}
