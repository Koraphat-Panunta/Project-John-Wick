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
        base.StateManager.Movement = Vector2.Lerp(base.StateManager.Movement, Vector2.zero, 0.1f);
        base.characterAnimator.SetFloat("ForBack_Ward", base.StateManager.Movement.y);
        base.characterAnimator.SetFloat("Side_LR", base.StateManager.Movement.x);
        base.PhysicUpdateState();
    }
}
