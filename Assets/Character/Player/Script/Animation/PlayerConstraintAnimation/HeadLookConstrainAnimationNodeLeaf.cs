using System;
using UnityEngine;

public class HeadLookConstrainAnimationNodeLeaf : AnimationConstrainNodeLeaf
{

    private HeadRotationConstraintManager headLookConstrain;
    protected Transform lookAtTransform;
    public HeadLookConstrainAnimationNodeLeaf(
        HeadRotationConstraintManager splineLookConstrain
        , Transform lookAtTransform
        , Func<bool> precondition) : base(precondition)
    {
        this.headLookConstrain = splineLookConstrain;
        this.lookAtTransform = lookAtTransform;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        this.headLookConstrain.SetLookPos(this.lookAtTransform.position);
        this.headLookConstrain.SetWeight(1);
        base.UpdateNode();
    }

}
