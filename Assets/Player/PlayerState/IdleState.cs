using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : CharacterState 
{

    Vector2 CurentMovement;
    public IdleState(Animator animator, GameObject Char,PlayerStateManager stateManager) : base(animator, Char,stateManager)
    {

    }
    public override void EnterState()
    {
        CurentMovement = new Vector2(characterAnimator.GetFloat("Side_LR"), characterAnimator.GetFloat("ForBack_Ward"));
        base.EnterState();
    }

    public override void ExitState()
    { 
        base.ExitState();
    }

    public override void FrameUpdateState()
    {
        this.CurentMovement = Vector2.Lerp(this.CurentMovement, Vector2.zero, 0.01f);
        base.FrameUpdateState();
    }

    public override void PhysicUpdateState()
    {
 
        base.characterAnimator.SetFloat("ForBack_Ward", this.CurentMovement.y);
        base.characterAnimator.SetFloat("Side_LR", this.CurentMovement.x);
        base.PhysicUpdateState();
    }
}
