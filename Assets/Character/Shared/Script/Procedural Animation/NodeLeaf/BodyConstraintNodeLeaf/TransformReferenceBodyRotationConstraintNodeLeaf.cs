using System;
using UnityEngine;

public class TransformReferenceBodyRotationConstraintNodeLeaf : BodyRotationConstraintNodeLeaf
{
    protected Transform lookTransformReference;
    protected float targetWeight;
    protected float weightChangeRate;

    public TransformReferenceBodyRotationConstraintNodeLeaf(
        Transform lookTransformRef
        , float targetWeight
        , float weightChangeRate
        , BodyConstraintManager bodyConstraint
        , BodyRotationConstrainScriptableObject bodyRotationConstrainScriptableObject
        , Func<bool> precondition) : base(bodyConstraint, bodyRotationConstrainScriptableObject, precondition)
    {
        this.lookTransformReference = lookTransformRef;
        this.targetWeight = targetWeight;
        this.weightChangeRate = weightChangeRate;
    }

    protected override void UpdateLookAtTarget()
    {
        this.bodyConstraint.SetLookPos(this.lookTransformReference.position);
    }

    protected override void UpdateWeight()
    {
        this.bodyConstraint.SetWeight(
            Mathf.MoveTowards(this.bodyConstraint.GetWeight(), this.targetWeight, this.weightChangeRate * Time.deltaTime));
    }
}
