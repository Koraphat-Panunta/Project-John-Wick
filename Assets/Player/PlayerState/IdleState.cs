using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : CharacterState 
{
    private Animator characterAnimator;
    private PlayerController playerController;
    private PlayerStateManager playerStateManager;
    public IdleState(Player player)
    {
        base.player = player;
        this.characterAnimator = player.animator;
        this.playerController = player.playerController;
        this.playerStateManager = player.playerStateManager;
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
        PlayerMovement playerMovement = base.player.playerMovement;
        InputPerformed();
        playerMovement.FreezingCharacter();
        //characterAnimator.SetFloat("ForBack_Ward",stateManager.playerController.input.movement.ReadValue<Vector2>().y);
        //characterAnimator.SetFloat("Side_LR", stateManager.playerController.input.movement.ReadValue<Vector2>().x);
    }
    protected override void InputPerformed()
    {
        if(playerController.player.playerController.input.movement.phase == InputActionPhase.Started)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.move);
        }
    }
}
