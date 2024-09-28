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
        InputPerformed();
        player.NotifyObserver(player,SubjectPlayer.PlayerAction.Idle);
    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        PlayerMovement playerMovement = base.player.playerMovement;
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
        new WeaponInput().InputWeaponUpdate(input, player);
        if(input.swapShoulder.phase == InputActionPhase.Started||Input.GetKeyDown(KeyCode.LeftAlt))
        {
            player.NotifyObserver(player,SubjectPlayer.PlayerAction.SwapShoulder);
        }
        
    }
  
}
