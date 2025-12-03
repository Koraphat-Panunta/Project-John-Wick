using System;
using UnityEngine;

public class ArmIKConstriantRefTransformNodeLeaf : ArmIKConstraintNodeLeaf
{
    protected Transform targetHandRef;
    protected TransformOffsetSCRP targetHandOffsetSCRP;

    public Vector3 getTargetHandOffsetPosition 
    { get => targetHandOffsetSCRP ? targetHandOffsetSCRP.postitionOffset : this._targetHandOffsetPosition; }
    protected Vector3 _targetHandOffsetPosition;

    public Quaternion getTargetHandOffsetRotation 
    { get => targetHandOffsetSCRP ? Quaternion.Euler(targetHandOffsetSCRP.rotationEulerOffset) : this._targetHandOffsetRotation; }
    protected Quaternion _targetHandOffsetRotation;
    public ArmIKConstriantRefTransformNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Transform targetHandReference
        , Func<bool> precondition
        , TransformOffsetSCRP transformOffsetSCRP
        , TransformOffsetSCRP targetHandOffsetSCRP) 
        : this (
              handArmIKConstraintManager
              , rootIKHandRef
              ,targetHandReference
              , precondition
              ,transformOffsetSCRP
              , targetHandOffsetSCRP.postitionOffset
              ,Quaternion.Euler(targetHandOffsetSCRP.rotationEulerOffset)
              )
    {
        this.targetHandRef = targetHandReference;
        this.targetHandOffsetSCRP = targetHandOffsetSCRP;
    }

    public ArmIKConstriantRefTransformNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Transform targetHandReference
        , Func<bool> precondition
        , TransformOffsetSCRP transformOffsetSCRP
        , Vector3 targetHandOffsetPosition
        , Quaternion targetHandOffsetRotation) : base(handArmIKConstraintManager, rootIKHandRef, precondition, transformOffsetSCRP)
    {
        this._targetHandOffsetPosition = targetHandOffsetPosition;
        this._targetHandOffsetRotation = targetHandOffsetRotation;
    }

    protected override void UpdateTargetHandPosition()
    {
        Vector3 targetPosition 
            = rootIKHandRef.transform.position 
            + (rootIKHandRef.forward * getTargetHandOffsetPosition.z)
            + (rootIKHandRef.up * getTargetHandOffsetPosition.y)
            + (rootIKHandRef.right * getTargetHandOffsetPosition.x
            );
        Quaternion targetRot = rootIKHandRef.rotation * getTargetHandOffsetRotation;
        this.handArmIKConstraintManager.SetTargetHand(targetPosition, targetRot);
    }
}
