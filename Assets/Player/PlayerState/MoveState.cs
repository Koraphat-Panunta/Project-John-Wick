using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : CharacterState 
{
    Vector2 MovementInput;
    Vector2 CurentMovement;
  
    public MoveState(Animator animator, GameObject Char,PlayerStateManager stateManager) : base(animator, Char, stateManager)
    {
    }
    public override void EnterState()
    {
        //base.characterAnimator.Play("Movement");
        CurentMovement = new Vector2(characterAnimator.GetFloat("Side_LR"), characterAnimator.GetFloat("ForBack_Ward"));
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState()
    {
        this.CurentMovement = Vector2.Lerp(this.CurentMovement, base.StateManager.GetComponent<PlayerController>().Movement, 0.01f);     
        base.FrameUpdateState();
    }
    public override void PhysicUpdateState()
    {
        base.characterAnimator.SetFloat("ForBack_Ward", this.CurentMovement.y);
        base.characterAnimator.SetFloat("Side_LR", this.CurentMovement.x);
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
