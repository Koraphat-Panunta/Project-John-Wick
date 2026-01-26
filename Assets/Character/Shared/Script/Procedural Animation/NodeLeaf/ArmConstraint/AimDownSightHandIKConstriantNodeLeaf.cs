using System;
using UnityEngine;

public class AimDownSightHandIKConstriantNodeLeaf : ArmIKConstraintNodeLeaf
{

    protected IWeaponAdvanceUser weaponAdvanceUser;
    public Vector3 aimDirConstriant 
    {
        get 
        {
            Vector3 dir = (this.weaponAdvanceUser._pointingPos - base.rootIKHandRef.position).normalized;
            return new Vector3(0, dir.y, dir.z);
        }
    }

    public Vector3 targetAnchorHandPosition 
    {
        get 
        {
            Vector3 forward = this.aimDirConstriant.normalized;
            Vector3 rightWard = Vector3.Cross(this.aimDirConstriant, Vector3.up).normalized;
            Vector3 upWard = Vector3.Cross(forward,rightWard).normalized;

            return this.rootIKHandRef.position 
                + (forward * this.rightHandIK_ConstraintSCRP.positionOffset.z)
                + (rightWard * this.rightHandIK_ConstraintSCRP.positionOffset.x)
                + (upWard * this.rightHandIK_ConstraintSCRP.positionOffset.y);
        }
    }
    public Quaternion targetAnchorHandRotaion
    {
        get 
        {
            return Quaternion.LookRotation(this.aimDirConstriant, Vector3.up) * Quaternion.Euler(this.rightHandIK_ConstraintSCRP.rotationEulerOffset.x, 0,this.rightHandIK_ConstraintSCRP.rotationEulerOffset.z);
        }
    }

    public float weight => weaponAdvanceUser._weaponManuverManager.aimingWeight;
    public Vector3 targetHandPosition
    {
        get 
        {
            return Vector3.Lerp(this.handArmIKConstraintManager.GetTargetHandTransform().position, this.targetAnchorHandPosition, this.weight);
        }
    }
    public Quaternion targetHandRotation
    {
        get
        {
            return Quaternion.Lerp(this.handArmIKConstraintManager.GetTargetHandTransform().rotation, this.targetAnchorHandRotaion, this.weight);
        }
    }

    protected Transform rootHintHandTransform;

    protected RightHandIK_ConstraintSCRP rightHandIK_ConstraintSCRP;

    public AimDownSightHandIKConstriantNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Transform rootHintHand
        , RightHandIK_ConstraintSCRP rightHandIK_ConstraintSCRP
        , IWeaponAdvanceUser weaponAdvanceUser
        , Func<bool> precondition) : base(handArmIKConstraintManager, rootIKHandRef, precondition)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.rightHandIK_ConstraintSCRP = rightHandIK_ConstraintSCRP;
        this.rootHintHandTransform = rootHintHand;
    }

    protected override void UpdateHintHandPotation()
    {
        Vector3 hintHandPos = this.rootHintHandTransform.position
             + this.rootHintHandTransform.forward * this.rightHandIK_ConstraintSCRP.hintPositionOffset.z
             + this.rootHintHandTransform.up * this.rightHandIK_ConstraintSCRP.hintPositionOffset.y
             + this.rootHintHandTransform.right * this.rightHandIK_ConstraintSCRP.hintPositionOffset.x;

        this.handArmIKConstraintManager.SetHintHandPosition(hintHandPos);
    }

    protected override void UpdateTargetHandPosition()
    {
        this.handArmIKConstraintManager.SetTargetHand(this.targetHandPosition, this.targetHandRotation);
    }
}
