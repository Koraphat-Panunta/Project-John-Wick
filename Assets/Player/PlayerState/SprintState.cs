using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : CharacterState
{
    [SerializeField] Camera main_camera;
    public float speed = 250;
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

    public override void FrameUpdateState()
    {
        base.FrameUpdateState();
    }

    public override void PhysicUpdateState()
    {
        base.StateManager.Movement = Vector2.Lerp(base.StateManager.Movement, base.StateManager.GetComponent<PlayerController>().Movement, 0.1f);
        if (base.StateManager.Movement.x == 0)
        {   
            RotateTowards(main_camera.transform.forward.normalized);
        }
        else
        {
            Vector3 Direction = main_camera.transform.forward + new Vector3(base.StateManager.Movement.x, base.StateManager.Movement.y, 0);
            RotateTowards(Direction);
        }
        base.characterController.SimpleMove(base.Character.transform.forward * speed * Time.deltaTime);
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


}
