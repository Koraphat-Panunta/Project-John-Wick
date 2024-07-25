using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprintState : CharacterState
{
    [SerializeField] Transform cameraTrans;
    public override void EnterState()
    {
        base.characterAnimator.SetBool("IsSprinting", true);
        base.EnterState();
    }

    public override void ExitState()
    {
        base.characterAnimator.SetBool("IsSprinting", false);
        base.ExitState();
    }

    public override void FrameUpdateState(StateManager stateManager)
    {
        base.FrameUpdateState(stateManager);
    }

    public override void PhysicUpdateState(StateManager stateManager)
    {
        InputPerformed();
        RotateTowards(TransformDirectionObject(new Vector3(base.playerController.movementInput.x,0,base.playerController.movementInput.y),cameraTrans.forward));
        base.PhysicUpdateState(stateManager);
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
    Vector3 Camera;
    Vector3 Input;
    Vector3 InputOfCam;
    float Zeta;
    private Vector3 TransformDirectionObject(Vector3 dirWolrd,Vector3 dirObjectLocal)
    {
        float zeta;
        Camera = dirObjectLocal.normalized;
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
