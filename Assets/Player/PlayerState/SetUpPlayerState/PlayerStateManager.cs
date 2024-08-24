using RealtimeCSG.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour 
{
    public IdleState idle;
    public MoveState move;
    public SprintState sprint;
    public Animator PlayerAnimator;
    public PlayerController playerController;
    public Vector2 Movement = Vector2.zero;

    public CharacterState Current_state;
    public void Start()
    {
        idle = new IdleState(this);
        move = new MoveState(this);
        sprint = new SprintState(this);
        Current_state = idle;
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

    protected void Update()
    {
        Current_state.FrameUpdateState(this);
    }

    protected void FixedUpdate()
    {
        this.Movement = Vector2.Lerp(this.Movement, playerController.movementInput, 10*Time.deltaTime);
        Current_state.PhysicUpdateState(this);
    }

    protected void SetUpState()
    {
        
    }
}
