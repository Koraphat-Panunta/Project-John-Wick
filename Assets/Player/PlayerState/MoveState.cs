using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : CharacterState 
{
    [SerializeField] private Camera main_camera;
    public float Speed = 100;
    
    public override void EnterState()
    {
        //base.characterAnimator.Play("Movement");
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState()
    {
        
        base.FrameUpdateState();
    }
    public override void PhysicUpdateState()
    {
        InputPerformed();
        base.characterAnimator.SetFloat("ForBack_Ward", base.StateManager.Movement.y);
        base.characterAnimator.SetFloat("Side_LR", base.StateManager.Movement.x);
        //base.characterController.SimpleMove(base.Character.transform.forward * Speed * Time.deltaTime);
        RotateTowards(main_camera.transform.forward);
     
        //character.velocity = base.Character.transform.forward.normalized*Speed;
        base.PhysicUpdateState();
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
    protected override void InputPerformed()
    {
        if (base.playerController.movementInput == Vector2.zero)
        {
            base.StateManager.ChangeState(base.StateManager.idle);
        }
        if (base.playerController.sprintInputX.phase.IsInProgress())
        {
            base.StateManager.ChangeState(base.StateManager.sprint);
        }
        base.InputPerformed();
    }


}
