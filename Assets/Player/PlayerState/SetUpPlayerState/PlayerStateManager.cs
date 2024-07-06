using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : StateManager 
{
    public IdleState idle { get; private set; }
    public MoveState move { get; private set; }
    public SprintState sprint { get; private set; }
    [SerializeField] private Animator PlayerAnimator;
    public PlayerController playerController { get; private set; }
    public PlayerStateManager(State StartState) : base(StartState)
    {
    }
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        idle = new IdleState(PlayerAnimator,gameObject,this);
        move = new MoveState(PlayerAnimator,gameObject,this);
        sprint = new SprintState(PlayerAnimator,gameObject, this);
        Current_state = idle;
    }
   
    public override void ChangeState(State Nextstate)
    {
        base.ChangeState(Nextstate);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
