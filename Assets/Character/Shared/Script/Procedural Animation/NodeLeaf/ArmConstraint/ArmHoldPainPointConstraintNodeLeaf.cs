using System;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class ArmHoldPainPointConstraintNodeLeaf : ArmIKConstraintNodeLeaf
{
    public Vector3 painPoint;
    public Vector3 targetHoldPoint;

    // Stored deltas relative to root
    protected Vector3 deltaPos_RootSpace;
    protected Quaternion deltaRot_RootSpace;

    public TransformOffsetSCRP transformHintOffsetSCRP;
    public ArmHoldPainPointConstraintNodeLeaf
        (
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
        , TransformOffsetSCRP transformHintOffsetSCRP
        ) 
        : base(handArmIKConstraintManager, rootIKHandRef, precondition)
    {
        this.transformHintOffsetSCRP = transformHintOffsetSCRP;
    }
    public override void Enter()
    {
        this.targetHoldPoint = handArmIKConstraintManager.twoBoneIKConstraint.data.target.position;
        base.Enter();
    }
    public void SetPainPoint(Vector3 painPointPosition)
    {
        this.painPoint = painPointPosition;

        // WORLD ? ROOT SPACE
        Vector3 targetDirection = (painPointPosition - rootIKHandRef.position).normalized;
        deltaPos_RootSpace = rootIKHandRef.InverseTransformPoint(painPointPosition);
        deltaRot_RootSpace = Quaternion.FromToRotation(Vector3.forward, targetDirection);
    }
    protected override void UpdateTargetHandPosition()
    {
        this.painPoint = rootIKHandRef.TransformPoint(deltaPos_RootSpace);

        this.targetHoldPoint = Vector3.Lerp(this.targetHoldPoint,this.painPoint,Time.deltaTime * 2.5f);

        Vector3 targetPos = targetHoldPoint + (handArmIKConstraintManager.twoBoneIKConstraint.data.mid.up * -.1f);

        handArmIKConstraintManager.SetTargetHand(targetPos, handArmIKConstraintManager.twoBoneIKConstraint.data.mid.rotation);
    }

    public Vector3 getOffsetHint { get => transformHintOffsetSCRP ? transformHintOffsetSCRP.postitionOffset : _offsetHint; }
    protected Vector3 _offsetHint;

    protected Vector3 hintHandPosition;
    protected override void UpdateHintHandPotation()
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
