using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : State
{
    public Animator characterAnimator { get; protected set; }
    public GameObject Character { get; protected set; }
    protected PlayerStateManager StateManager;
   
    public void SetUp(Animator animator, GameObject Char, PlayerStateManager stateManager)
    {
        characterAnimator = animator;
        Character = Char;
        StateManager = stateManager;
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState()
    {
        base.FrameUpdateState();
    }

    public override void PhysicUpdateState()
    {
        base.PhysicUpdateState();
    }

   
}
