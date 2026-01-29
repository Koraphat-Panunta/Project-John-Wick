using System;
using UnityEngine;

public class WeaponUserAimAtHandIKConstriantNodeLeaf : AimAtHandIKConstriantNodeLeaf
{

    protected float blockedWeight;

    protected Vector3 targetBlockedHand_Position
    {
        get
        {
            return this.handIK_Transform_Ref_Pos.position
          + (forward * this.weaponRightHandIK_ConstraintSCRP.onBlocked_positionOffset.z)
          + (rightWard * this.weaponRightHandIK_ConstraintSCRP.onBlocked_positionOffset.x)
          + (upWard * this.weaponRightHandIK_ConstraintSCRP.onBlocked_positionOffset.y);
        }
    }
    protected Quaternion targetBlockedHand_Rotation
    {
        get
        {
            return Quaternion.LookRotation((this.aimingAtTransfrom.position - this.targetHandPosition).normalized, this.handIK_Transform_Ref_Rot.up) * Quaternion.Euler(this.weaponRightHandIK_ConstraintSCRP.onBlocked_rotationEulerOffset);
        }
    }
    protected Vector3 hintBlockedHand_Position
    {
        get
        {
            Vector3 hintHandPos = this.targetHandPosition
           + this.handArmIKConstraintManager.GetHintHandTransform().forward * this.weaponRightHandIK_ConstraintSCRP.onBlocked_hintPositionOffset.z
           + this.handArmIKConstraintManager.GetHintHandTransform().up * this.weaponRightHandIK_ConstraintSCRP.onBlocked_hintPositionOffset.y
           + this.handArmIKConstraintManager.GetHintHandTransform().right * this.weaponRightHandIK_ConstraintSCRP.onBlocked_hintPositionOffset.x;

            return hintHandPos;
        }
    }

    public override Vector3 targetHandPosition
    {
        get { return Vector3.Lerp(base.targetHandPosition, this.targetBlockedHand_Position, this.blockedWeight); }
    }
    public override Quaternion targetHandRotation
    {
        get { return Quaternion.Lerp(base.targetHandRotation, this.targetBlockedHand_Rotation, this.blockedWeight); }
    }
    public override Vector3 targetHintHandPosition
    {
        get { return Vector3.Lerp(base.targetHintHandPosition, this.hintBlockedHand_Position, this.blockedWeight); }
    }

    protected WeaponHandIK_ConstraintSCRP weaponRightHandIK_ConstraintSCRP;
    protected IWeaponAdvanceUser weaponAdvanceUser;

    public WeaponUserAimAtHandIKConstriantNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform aimingAtTransform
        , Transform handIK_Transform_Ref_Pos
        , Transform handIK_Transform_Ref_Rot
        , Transform rootHintHand
        , Transform rootCharacter
        , IWeaponAdvanceUser weaponAdvanceUser
        , WeaponHandIK_ConstraintSCRP rightHandIK_ConstraintSCRP
        , Func<bool> precondition) : 
        base(
            handArmIKConstraintManager
            , aimingAtTransform
            , handIK_Transform_Ref_Pos
            , handIK_Transform_Ref_Rot
            , rootHintHand
            , rootCharacter
            , rightHandIK_ConstraintSCRP
            , precondition
            )
    {
        this.weaponRightHandIK_ConstraintSCRP = rightHandIK_ConstraintSCRP;
        this.weaponAdvanceUser = weaponAdvanceUser;
    }
    public override void Enter()
    {
        this.blockedWeight = 0;
        this.targetBlockWeight = 0;
        base.Enter();
    }
    public override void UpdateNode()
    {
        this.BlockingCheck();
        base.UpdateNode();
    }

    LayerMask blockedMaskDefault = LayerMask.GetMask("Default");
    private float targetBlockWeight = 0;
    protected void BlockingCheck()
    {
        Vector3 castPos = Vector3.Project(this.weaponAdvanceUser._currentWeapon.bulletSpawner.transform.position - base.handIK_Transform_Ref_Pos.position
            , base.aimDirConstriant) + base.handIK_Transform_Ref_Pos.position;

        Debug.DrawLine(base.handIK_Transform_Ref_Pos.position, castPos, Color.green);

        Vector3 castPosToStart = castPos - base.handIK_Transform_Ref_Pos.position;

        if (Physics.Raycast(base.handIK_Transform_Ref_Pos.position, castPosToStart.normalized, castPosToStart.magnitude, this.blockedMaskDefault, QueryTriggerInteraction.Ignore))
        {
            this.targetBlockWeight = Mathf.Clamp01(this.targetBlockWeight + Time.deltaTime * 2);
        }
        else if(Physics.Raycast(base.handIK_Transform_Ref_Pos.position, castPosToStart.normalized, castPosToStart.magnitude + .15f, this.blockedMaskDefault, QueryTriggerInteraction.Ignore) == false)
        {
            this.targetBlockWeight = Mathf.Clamp01(this.targetBlockWeight - Time.deltaTime * 5);
        }
        this.blockedWeight = Mathf.Lerp(this.blockedWeight, this.targetBlockWeight, Time.deltaTime * 80);

    }
}
