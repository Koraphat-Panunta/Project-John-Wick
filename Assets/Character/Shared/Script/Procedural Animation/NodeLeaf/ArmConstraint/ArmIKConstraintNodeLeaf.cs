using System;
using UnityEngine;

public abstract class ArmIKConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    protected HandArmIKConstraintManager handArmIKConstraintManager;

    public Transform rootIKHandRef { get; protected set; }
   

   
    public ArmIKConstraintNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
       ) : base(precondition)
    {
        this.handArmIKConstraintManager = handArmIKConstraintManager;
        this.rootIKHandRef = rootIKHandRef;
    }

    public override void UpdateNode()
    {
        this.UpdateHintHandPotation();
        this.UpdateTargetHandPosition();
        base.UpdateNode();
    }

    protected abstract void UpdateTargetHandPosition();
    protected abstract void UpdateHintHandPotation();
   
}
