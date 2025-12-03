using System;
using UnityEngine;

public class ArmHoldPainPointConstraintNodeLeaf : ArmIKConstraintNodeLeaf
{
    public Vector3 painPoint;
    public ArmHoldPainPointConstraintNodeLeaf
        (
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
        , TransformOffsetSCRP transformOffsetSCRP
        ) 
        : base(handArmIKConstraintManager, rootIKHandRef, precondition, transformOffsetSCRP)
    {
    }

    public void SetPainPoint(Vector3 painPointPosition)
    {
        this.painPoint = painPointPosition;
    }
    protected override void UpdateTargetHandPosition()
    {
        handArmIKConstraintManager.SetTargetHand(this.painPoint,handArmIKConstraintManager.twoBoneIKConstraint.data.mid.rotation);
    }
}
