using RealtimeCSG.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager 
{
    public IdleState idle;
    public IdleInCover idleInCover;
    public IdleState normalIdle;

    public MoveState move;
    public MoveInCover moveInCover;
    public MoveState normalMove;

    public SprintState sprint;
    public CharacterState Current_state;

    public Player player;
    public PlayerController playerController;

    public Animator PlayerAnimator;
    


    public PlayerStateManager(Player player)
    {
        this.player = player;
        this.playerController = player.playerController;
    }

    public void ChangeState(CharacterState Nextstate)
    {
            Current_state.ExitState();
            Current_state = Nextstate;
            Current_state.EnterState();
    }
    public void ChangeEncapsulationState(CharacterState curState,CharacterState encapState)
    {
        if(Current_state == curState)
        {
            curState = encapState;
            ChangeState(curState);
        }
        else
        {
            curState = encapState;
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
        idleInCover = new IdleInCover(player);
        player.AddObserver(idleInCover);
        normalIdle = new IdleState(player);

        move = new MoveState(player);
        moveInCover = new MoveInCover(player);
        player.AddObserver(moveInCover);
        normalMove = new MoveState(player);

        sprint = new SprintState(player);

        Current_state = idle;
    }
}
