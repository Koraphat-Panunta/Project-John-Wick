using System;
using UnityEngine;

public class AimAtHandIKConstriantNodeLeaf : AnimationConstrainNodeLeaf
{

    public Vector3 aimDirConstriant 
    {
        get 
        {
            Vector3 refDir = Quaternion.LookRotation(this.rootCharacter.forward,Vector3.up) * Quaternion.Euler(this.rightHandIK_ConstraintSCRP.rotateRefDirOffset) * Vector3.forward;

            Debug.DrawRay(this.rootCharacter.position, refDir * 2, Color.yellow);

            Vector3 dir = (this.aimingAtTransfrom.position - this.handIK_Transform_Ref_Pos.position).normalized;
            dir = ClampDirection.GetClampDirection(refDir, dir, this.maxHorizontalHandTargetDegree, this.maxVerticalHandTargetDegree);
            return dir;
        }
    }

    protected Vector3 forward => this.aimDirConstriant.normalized;
    protected Vector3 rightWard => Vector3.Cross(Vector3.up, this.aimDirConstriant).normalized;
    protected Vector3 upWard => Vector3.Cross(forward, rightWard).normalized;
    public Vector3 targetAnchorHandPosition 
    {
        get 
        {
            //Debug.DrawRay(base.rootIKHandRef.position, forward,Color.blue);
            //Debug.DrawRay(base.rootIKHandRef.position, rightWard, Color.red);
            //Debug.DrawRay(base.rootIKHandRef.position, upWard, Color.green);

            return this.handIK_Transform_Ref_Pos.position 
                + (forward * this.rightHandIK_ConstraintSCRP.positionOffset.z)
                + (rightWard * this.rightHandIK_ConstraintSCRP.positionOffset.x)
                + (upWard * this.rightHandIK_ConstraintSCRP.positionOffset.y);
        }
    }
    public Quaternion targetAnchorHandRotaion
    {
        get 
        {
            return Quaternion.LookRotation((this.aimingAtTransfrom.position - this.targetHandPosition).normalized,this.rootCharacter.up) * Quaternion.Euler(this.rightHandIK_ConstraintSCRP.rotationEulerOffset);
        }
    }
    public Vector3 targerAnchorHintHandPosition
    {
        get
        {
            Vector3 hintHandPos = this.targetHandPosition
           + this.handArmIKConstraintManager.GetTargetHandTransform().forward * this.rightHandIK_ConstraintSCRP.hintPositionOffset.z
           + this.handArmIKConstraintManager.GetTargetHandTransform().up * this.rightHandIK_ConstraintSCRP.hintPositionOffset.y
           + this.handArmIKConstraintManager.GetTargetHandTransform().right * this.rightHandIK_ConstraintSCRP.hintPositionOffset.x;

            return hintHandPos;
        }
    }

    public float weight;
    public virtual Vector3 targetHandPosition
    {
        get 
        {
            return Vector3.Lerp(this.handArmIKConstraintManager.GetTargetHandTransform().position,this.targetAnchorHandPosition,this.weight);
        }
    }
    public virtual Quaternion targetHandRotation
    {
        get
        {
            return Quaternion.Lerp(this.handArmIKConstraintManager.GetTargetHandTransform().rotation, this.targetAnchorHandRotaion, this.weight);
        }
    }

    public virtual Vector3 targetHintHandPosition
    {
        get
        {
            return Vector3.Lerp(this.handArmIKConstraintManager.GetHintHandTransform().position, this.targerAnchorHintHandPosition, this.weight);
        }
    }

    public HandArmIKConstraintManager handArmIKConstraintManager { get; protected set; }

    protected Transform handIK_Transform_Ref_Pos;
    protected Transform handIK_Transform_Ref_Rot;

    protected Transform rootHintHandTransform;

    protected HandIK_ConstraintSCRP rightHandIK_ConstraintSCRP;

    protected Transform aimingAtTransfrom;
    protected Transform rootCharacter;

    protected float maxVerticalHandTargetDegree => this.rightHandIK_ConstraintSCRP.maxVerticalHandAimDeg;
    protected float maxHorizontalHandTargetDegree => this.rightHandIK_ConstraintSCRP.maxHorizontalHandAimDeg;
    public AimAtHandIKConstriantNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform aimingAtTransform
        , Transform handIK_Transform_Ref_Pos
        , Transform handIK_Transform_Ref_Rot
        , Transform rootHintHand
        , Transform rootCharacter
        , HandIK_ConstraintSCRP rightHandIK_ConstraintSCRP
        , Func<bool> precondition) : base(precondition)
    {
        this.handArmIKConstraintManager = handArmIKConstraintManager;

        this.handIK_Transform_Ref_Pos = handIK_Transform_Ref_Pos;
        this.handIK_Transform_Ref_Rot = handIK_Transform_Ref_Rot;

        this.rootCharacter = rootCharacter;
        this.aimingAtTransfrom = aimingAtTransform;
        this.rightHandIK_ConstraintSCRP = rightHandIK_ConstraintSCRP;
        this.rootHintHandTransform = rootHintHand;
    }
    public override void Enter()
    {
        weight = 0;
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.weight = Mathf.Clamp01(weight + Time.deltaTime );
        this.UpdateTargetHandPosition();
        this.UpdateHintHandPotation();
        base.UpdateNode();
    }
    protected void UpdateHintHandPotation()
    {

        this.handArmIKConstraintManager.SetHintHandPosition(this.targetHintHandPosition);
    }

    protected void UpdateTargetHandPosition()
    {
        this.handArmIKConstraintManager.SetTargetHand(this.targetHandPosition, this.targetHandRotation);
    }
}
