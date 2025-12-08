using System;
using UnityEngine;

public class ArmSwingPainStateConstraintNodeLeaf : ArmIKConstraintNodeLeaf
{
    Vector3 targetHandPosition;
    Vector3 hintArmPosition;
    public ArmSwingPainStateConstraintNodeLeaf(HandArmIKConstraintManager handArmIKConstraintManager, Transform rootIKHandRef, Func<bool> precondition, TransformOffsetSCRP transformHintOffsetSCRP) : base(handArmIKConstraintManager, rootIKHandRef, precondition, transformHintOffsetSCRP)
    {
    }

    protected override void UpdateTargetHandPosition()
    {
        throw new NotImplementedException();
    }
}
public class BioticArmSimulate 
{
    public BioticArmSimulate()
    { 
    }
}
