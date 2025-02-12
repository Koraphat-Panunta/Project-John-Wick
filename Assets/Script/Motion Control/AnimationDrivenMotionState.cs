using UnityEngine;

public class AnimationDrivenMotionState : MotionState
{
    private Animator animator;
    public AnimationDrivenMotionState(Animator animator) 
    {
        this.animator = animator;
    }

    public override void Enter()
    {
        animator.applyRootMotion = true;
        animator.enabled = true;
        base.Enter();
    }

    public override void Exit()
    {
        animator.applyRootMotion = true;
        animator.enabled = false;
        base.Exit();
    }
}
