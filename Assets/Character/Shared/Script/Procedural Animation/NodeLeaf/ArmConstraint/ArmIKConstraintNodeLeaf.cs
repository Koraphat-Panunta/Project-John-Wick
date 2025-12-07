using System;
using UnityEngine;

public abstract class ArmIKConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    protected HandArmIKConstraintManager handArmIKConstraintManager;

    protected TransformOffsetSCRP transformHintOffsetSCRP;

    protected Transform rootIKHandRef;
    public Vector3 getOffsetHint { get => transformHintOffsetSCRP ? transformHintOffsetSCRP.postitionOffset : _offsetHint; }
    protected Vector3 _offsetHint;

    protected Vector3 hintHandPosition;

    public ArmIKConstraintNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
        ,TransformOffsetSCRP transformHintOffsetSCRP) 
        : this(
            handArmIKConstraintManager
              , rootIKHandRef
            ,precondition
            ,transformHintOffsetSCRP.postitionOffset
            )
    {
        this.transformHintOffsetSCRP = transformHintOffsetSCRP;
    }

    public ArmIKConstraintNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
        , Vector3 offsetHint) : base(precondition)
    {
        this.handArmIKConstraintManager = handArmIKConstraintManager;
        this._offsetHint = offsetHint;
        this.rootIKHandRef = rootIKHandRef;
    }

    public override void UpdateNode()
    {
        this.UpdateHintHandPotation();
        this.UpdateTargetHandPosition();
        base.UpdateNode();
    }

    protected abstract void UpdateTargetHandPosition();
    protected virtual void UpdateHintHandPotation()
    {
        hintHandPosition = this.rootIKHandRef.position
            +
            (
            this.rootIKHandRef.right * this.getOffsetHint.x
            )
            +
            (
            this.rootIKHandRef.up * this.getOffsetHint.y
            )
            +
            (
            this.rootIKHandRef.forward * this.getOffsetHint.z
            );

        handArmIKConstraintManager.SetHintHandPosition(hintHandPosition);
    }
}
