using UnityEngine;

public class CodeDrivenMotionState : MotionState
{
    private Animator _animator;
    public CodeDrivenMotionState(Animator animator)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.applyRootMotion = false;
        _animator.enabled = true;

        base.Enter();
    }

    public override void Exit()
    {
        _animator.enabled = true;
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
