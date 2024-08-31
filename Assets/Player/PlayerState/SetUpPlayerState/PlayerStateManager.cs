using RealtimeCSG.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager 
{
    public IdleState idle;
    public MoveState move;
    public SprintState sprint;
    public CharacterState Current_state;

    public Player player;
    public PlayerController playerController;

    public Animator PlayerAnimator;
    


    public PlayerStateManager(Player player)
    {
        this.player = player;
        this.playerController = player.playerController;
        this.PlayerAnimator = player.animator;
    }

    public void ChangeState(CharacterState Nextstate)
    {
        if(Current_state != Nextstate)
        {
            Current_state.ExitState();
            Current_state = Nextstate;
            Current_state.EnterState();
        }
       
    }
    public void Update()
    {
        Current_state.FrameUpdateState(this);
    }

    public void FixedUpdate()
    {
        Current_state.PhysicUpdateState(this);
    }
    public void SetupState(Player player)
    {
        idle = new IdleState(player);
        move = new MoveState(player);
        sprint = new SprintState(player);
        Current_state = idle;
    }
}
