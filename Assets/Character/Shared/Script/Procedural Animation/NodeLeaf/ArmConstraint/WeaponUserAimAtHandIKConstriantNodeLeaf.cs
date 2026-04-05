using System;
using UnityEngine;

public class WeaponUserAimAtHandIKConstriantNodeLeaf : AimAtHandIKConstriantNodeLeaf
{

    protected float blockedWeight;

    protected Vector3 handPos => weaponAdvanceUser._userWeapon.humanoidBone._rightHandBone.position;

    protected Vector3 targetBlockedHand_Position
    {
        get
        {
            return this.handPos
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
        get 
        {
            Vector3 calHandPos = Vector3.Lerp(base.targetHandPosition, this.targetBlockedHand_Position, this.blockedWeight);
            Transform recoilDir = this.weaponAdvanceUser._currentWeapon.bulletSpawner.transform ;

            Vector3 recoilPos = calHandPos
                + (recoilDir.forward * this.weaponRightHandIK_ConstraintSCRP.recoil_Additional_Position_Offset.z)
                + (recoilDir.up * this.weaponRightHandIK_ConstraintSCRP.recoil_Additional_Position_Offset.y)
                + (recoilDir.right * this.weaponRightHandIK_ConstraintSCRP.recoil_Additional_Position_Offset.x);

            calHandPos = Vector3.Lerp(calHandPos, recoilPos, this.recoilWeightPos);
            
            return calHandPos;
        }
    }
    public override Quaternion targetHandRotation
    {
        get 
        {
            Quaternion calculateRot = Quaternion.Lerp(base.targetHandRotation, this.targetBlockedHand_Rotation, this.blockedWeight);
            Quaternion recoilRot = calculateRot * Quaternion.Euler(this.weaponRightHandIK_ConstraintSCRP.recoil_Additional_Rotation_Offset);
            calculateRot = Quaternion.Lerp(calculateRot,recoilRot,this.recoilWeightRot);

            //Debug.DrawRay(this.targetHandPosition, calculateRot * Vector3.forward,Color.yellow);

            return calculateRot;
        }
    }
    public override Vector3 targetHintHandPosition
    {
        get { return Vector3.Lerp(base.targetHintHandPosition, this.hintBlockedHand_Position, this.blockedWeight); }
    }

    protected WeaponHandIK_ConstraintSCRP weaponRightHandIK_ConstraintSCRP => base.handIK_ConstraintSCRP as WeaponHandIK_ConstraintSCRP;
    protected IWeaponAdvanceUser weaponAdvanceUser;

    protected float recoilWeightPos;
    protected float recoilWeightRot;

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
        this.weaponAdvanceUser = weaponAdvanceUser;
    }
    public override void Enter()
    {
        this.blockedWeight = 0;
        this.targetBlockWeight = 0;
        this.recoilWeightPos = 0;
        this.recoilWeightRot = 0;
        base.Enter();
    }
    public override void UpdateNode()
    {
        //Debug.Log("target Block weight = "+targetBlockWeight);
        this.RecoilWeightUpdate();
        this.BlockingCheck();
        base.UpdateNode();
    }

    LayerMask blockedMaskDefault = LayerMask.GetMask("Default");
    private float targetBlockWeight = 0;
    protected void BlockingCheck()
    {
        Vector3 castPos = Vector3.Project(this.weaponAdvanceUser._currentWeapon.bulletSpawner.transform.position - this.handPos
            , base.aimDirConstriant) + this.handPos;

        Vector3 castPosToStart = castPos - this.handPos;

        //Debug.DrawLine(castPos, this.handPos, Color.red);

        if (Physics.Raycast(this.handPos, castPosToStart.normalized,out RaycastHit hitInfo, castPosToStart.magnitude, this.blockedMaskDefault, QueryTriggerInteraction.Ignore))
        {
            this.targetBlockWeight = Mathf.Clamp01(this.targetBlockWeight + Time.deltaTime * 2);

        }
        else if(Physics.Raycast(this.handPos, castPosToStart.normalized, castPosToStart.magnitude + .15f, this.blockedMaskDefault, QueryTriggerInteraction.Ignore) == false)
        {
            this.targetBlockWeight = Mathf.Clamp01(this.targetBlockWeight - Time.deltaTime * 5);
        }
        this.blockedWeight = Mathf.Lerp(this.blockedWeight, this.targetBlockWeight, Time.deltaTime * 80);

    }
    protected void RecoilWeightUpdate()
    {

        this.recoilWeightPos = Mathf.Clamp01(this.recoilWeightPos - Time.deltaTime * 4);
        this.recoilWeightRot = Mathf.Clamp01(this.recoilWeightRot - Time.deltaTime * 10);


        //this.recoilWeightPos = 1;
        //this.recoilWeightRot = 1;
    }
    public void TriggeRecoilWeight(float weight)
    {
        this.recoilWeightPos = Mathf.Clamp01(weight);
        this.recoilWeightRot = Mathf.Clamp01(weight);
    }
}
