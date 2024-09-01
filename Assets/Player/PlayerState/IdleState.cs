using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : CharacterState 
{
    private PlayerController playerController;
    private PlayerStateManager playerStateManager;
    
    public IdleState(Player player)
    {
        base.player = player;
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
       player.NotifyObserver(player,SubjectPlayer.PlayerAction.Idle);
    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        InputPerformed();
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Idle);
        playerMovement.FreezingCharacter();
    }
    protected override void InputPerformed()
    {
        PlayerController.Input input = this.playerController.input;
        if (input.movement.phase == InputActionPhase.Started)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.move);
        }
        if(input.aiming.phase == InputActionPhase.Performed||input.aiming.phase == InputActionPhase.Started)
        {
            Debug.Log("Aim Idle");
            player.playerWeaponCommand.Aim();
        }
        else
        {
            player.playerWeaponCommand.LowWeapon();
        }

        if(input.firing.phase == InputActionPhase.Performed)
        {
            Debug.Log("Pull Trigger Idle");
            player.playerWeaponCommand.Pulltriger();
        }
        else
        {
            player.playerWeaponCommand.CancelTrigger();
        }

        if(input.reloading.phase == InputActionPhase.Performed)
        {
            player.playerWeaponCommand.Reload();
        }
    }
}
