using System;
using UnityEngine;

public class BodySetRotationConstraintNodeLeaf : BodyRotationConstraintNodeLeaf
{
    public float transitionDuration;
    public float normalizedTime;
    protected Transform lookAtDirection;

    public BodySetRotationConstraintNodeLeaf(
        BodyConstraintManager bodyConstraint
        , BodyRotationConstrainScriptableObject bodyRotationConstrainScriptableObject
        ,float transitionDuration
        ,Transform lookAtDirection
        , Func<bool> precondition) 
        : base
        (
            bodyConstraint
            , bodyRotationConstrainScriptableObject
            , precondition
            )
    {
        this.transitionDuration = transitionDuration;
        this.lookAtDirection = lookAtDirection;
    }

    public void SetTransitionDuration(float transtitionDuration) => this.transitionDuration = transtitionDuration;

    public override void Enter()
    {
        this.normalizedTime = 0f;
        base.Enter();
    }

    protected override void UpdateLookAtTarget()
    {
        this.bodyConstraint.SetLookDirection(this.lookAtDirection.forward);
    }

    protected override void UpdateWeight()
    {
        float step = this.transitionDuration > 0f ? Time.deltaTime / this.transitionDuration : 1f;
        this.normalizedTime = Mathf.MoveTowards(this.normalizedTime, 1f, step);
        this.bodyConstraint.SetWeight(Mathf.Lerp(this.bodyConstraint.GetWeight(),1,this.normalizedTime));
    }
}
