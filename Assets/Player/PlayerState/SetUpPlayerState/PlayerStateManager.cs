using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : StateManager 
{
    [SerializeField] private IdleState idle;
    [SerializeField] private MoveState move;
    [SerializeField] private SprintState sprint;
    [SerializeField] private Animator PlayerAnimator;
    public PlayerController playerController { get; private set; }
    public PlayerStateManager(State StartState) : base(StartState)
    {
    }
    private void Start()
    {
       playerController = GetComponent<PlayerController>();
        
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
