using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterState : State
{
    public Animator characterAnimator { get; protected set; }
    public GameObject Character { get; protected set; }
    public CharacterController characterController { get; protected set; }
    protected PlayerStateManager StateManager;
    protected PlayerController playerController;
   
    public void SetUp(Animator animator, GameObject Char, PlayerStateManager stateManager,PlayerController playerController)
    {
        characterAnimator = animator;
        Character = Char;
        StateManager = stateManager;
        this.playerController = playerController;
        characterController = characterAnimator.GetComponent<CharacterController>();
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState(StateManager stateManager)
    {
        base.FrameUpdateState(stateManager);
    }

    public override void PhysicUpdateState(StateManager stateManager)
    {
        base.PhysicUpdateState(stateManager);
    }
    protected virtual void InputPerformed()
    {

    }

   
}
