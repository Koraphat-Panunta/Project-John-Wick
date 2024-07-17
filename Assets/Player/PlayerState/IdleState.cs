using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : CharacterState 
{
    public override void EnterState()
    {
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
        base.PhysicUpdateState();
    }
    protected override void InputPerformed()
    {
        if (playerController.movementInput != Vector2.zero)
        {
            base.StateManager.ChangeState(base.StateManager.move);
        }
        base.InputPerformed();
    }
}
