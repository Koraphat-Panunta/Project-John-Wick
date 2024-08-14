using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : StateManager 
{
    [SerializeField] public IdleState idle;
    [SerializeField] public MoveState move;
    [SerializeField] public SprintState sprint;
    [SerializeField] private Animator PlayerAnimator;
    public PlayerController playerController { get; private set; }
    public Vector2 Movement = Vector2.zero;
    protected override void Start()
    {

        playerController = GetComponent<PlayerController>();
        Current_state = idle;
        base.Start();
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
        this.Movement = Vector2.Lerp(this.Movement, playerController.movementInput, 10*Time.deltaTime);
        base.FixedUpdate();
    }

    protected override void SetUpState()
    {
        idle.SetUp(PlayerAnimator, gameObject, this,playerController);
        move.SetUp(PlayerAnimator, gameObject, this,playerController);
        sprint.SetUp(PlayerAnimator, gameObject, this, playerController);
    }
}
