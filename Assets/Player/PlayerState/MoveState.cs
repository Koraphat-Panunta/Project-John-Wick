using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : CharacterState
{
    Vector2 Movement;
    public MoveState(Animator animator, GameObject Char) : base(animator, Char)
    {
    }
    public override void EnterState()
    {
        base.characterAnimator.Play("Movement");
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
        base.PhysicUpdateState();
    }

    protected float rotationSpeed = 5.0f;
    protected void RotateTowards(Vector3 direction)
    {
        // Ensure the direction is normalized
        direction.Normalize();

        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target rotation
        Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
}
