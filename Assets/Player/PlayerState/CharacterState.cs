using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : State
{
    public Animator characterAnimator { get; protected set; }
    public GameObject Character { get; protected set; }
    public CharacterState(Animator animator, GameObject Char) 
    {
        characterAnimator = animator;
        Character = Char;
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
