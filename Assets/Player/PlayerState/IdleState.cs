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

    public override void FrameUpdateState(StateManager stateManager)
    {
        base.FrameUpdateState(stateManager);
    }

    public override void PhysicUpdateState(StateManager stateManager)
    {
        InputPerformed();
        base.characterAnimator.SetFloat("ForBack_Ward", base.StateManager.Movement.y);
        base.characterAnimator.SetFloat("Side_LR", base.StateManager.Movement.x);
        base.PhysicUpdateState(stateManager);
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
