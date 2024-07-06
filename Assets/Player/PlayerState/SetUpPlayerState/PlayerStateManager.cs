using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : StateManager
{
    public IdleState idle { get; private set; }
    public MoveState move { get; private set; }
    public SprintState sprint { get; private set; }
    [SerializeField] private Animator PlayerAnimator;
    public PlayerStateManager(State StartState) : base(StartState)
    {
    }
    private void Start()
    {
        idle = new IdleState(PlayerAnimator,gameObject);
        move = new MoveState(PlayerAnimator,gameObject);
        sprint = new SprintState(PlayerAnimator,gameObject);
    }
    public override void FixedStateUpdate()
    {
        base.FixedStateUpdate();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
    public override void ChangeState(State Nextstate)
    {
        base.ChangeState(Nextstate);
    }


}
