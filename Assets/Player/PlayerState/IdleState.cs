using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : CharacterState 
{
    public IdleState(PlayerStateManager playerStateManager)
    {
        base.playerController = playerStateManager.playerController;
        base.Character = playerStateManager.gameObject;
        base.StateManager = playerStateManager;
        base.characterAnimator = playerStateManager.PlayerAnimator;
    }
    public override void EnterState()
    {
       
    }

    public override void ExitState()
    { 
        
    }

    public override void FrameUpdateState(PlayerStateManager stateManager)
    {
       
    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        InputPerformed();
        base.characterAnimator.SetFloat("ForBack_Ward", base.StateManager.Movement.y);
        base.characterAnimator.SetFloat("Side_LR", base.StateManager.Movement.x);
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
