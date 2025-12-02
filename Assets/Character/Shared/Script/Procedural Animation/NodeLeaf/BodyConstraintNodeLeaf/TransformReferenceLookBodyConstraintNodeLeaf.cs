using System;
using UnityEngine;

public class TransformReferenceLookBodyConstraintNodeLeaf : LookBodyConstraintNodeLeaf
{
    protected Transform lookTransformReference;
    protected float targetWeight;
    protected float weightChnageRate;

    public TransformReferenceLookBodyConstraintNodeLeaf(
        Transform lookTransformRef
        , float targetWeight
        , float weightChangeRate
        , BodyLookConstrain splineLookConstrain
        , AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject
        , Func<bool> precondition) : base(splineLookConstrain, aimSplineLookConstrainScriptableObject, precondition)
    {
        this.lookTransformReference = lookTransformRef;
        this.targetWeight = targetWeight;
        this.weightChnageRate = weightChangeRate;
    }

    protected override void UpdateLookAtTarget()
    {
        base.bodyLookConstrain.
            SetLookAtPosition(this.lookTransformReference.position);
    }

    protected override void UpdateWeight()
    {
        base.bodyLookConstrain.SetWeight
            (
            Mathf.MoveTowards(bodyLookConstrain.GetWeight(), this.targetWeight, this.weightChnageRate * Time.deltaTime)
            );
    }
}
