using System;
using UnityEngine;

public class WeaponLeftHandGripHandConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    protected Vector3 weaponMainHandSecondHandGrip_Position_Offset 
    {
        get
        {
            try
            {
                Weapon curWeapon = this.weaponAdvanceUser._currentWeapon;
                Vector3 offset = curWeapon._SecondHandGripTransform.localPosition - curWeapon._mainHandGripTransform.localPosition;
                return offset;
            }
            catch
            {
                Debug.LogError("WeaponLeftHandGripHandConstraintNodeLeaf weapom null");
                return Vector3.zero;
            }
        }
    }
    protected Quaternion weaponMainHandSecondHandGrip_Rotation_Offset
    {
        get
        {
            try
            {
                Weapon curWeapon = this.weaponAdvanceUser._currentWeapon;
                Quaternion resutl = curWeapon._SecondHandGripTransform.localRotation * Quaternion.Inverse(curWeapon._mainHandGripTransform.localRotation);
                return resutl;
            }
            catch
            {
                Debug.LogError("WeaponLeftHandGripHandConstraintNodeLeaf weapom null");
                return Quaternion.identity;
            }
        }
    }

    public Vector3 leftHandTargetPosition 
    {
        get
        {
            return this.rightHandTransform.position
            +(this.rightHandTransform.up * (weaponMainHandSecondHandGrip_Position_Offset.z + this.handIK_ConstraintSCRP.positionOffset.z))
            +(this.rightHandTransform.right * -1 *( weaponMainHandSecondHandGrip_Position_Offset.y + this.handIK_ConstraintSCRP.positionOffset.y))
            + (this.rightHandTransform.forward * -1 *( weaponMainHandSecondHandGrip_Position_Offset.x + this.handIK_ConstraintSCRP.positionOffset.x));
        }
    }
    public Quaternion leftHandTargetRotation => this.rightHandTransform.rotation * weaponMainHandSecondHandGrip_Rotation_Offset * Quaternion.Euler(this.handIK_ConstraintSCRP.rotationEulerOffset);

    public Vector3 leftHandHintPosition
    {
        get
        {
            return this.leftHandConstraintManager.GetTargetHandTransform().position 
                + (this.leftHandConstraintManager.GetTargetHandTransform().forward * this.handIK_ConstraintSCRP.hintPositionOffset.z)
                + (this.leftHandConstraintManager.GetTargetHandTransform().up * this.handIK_ConstraintSCRP.hintPositionOffset.y)
                + (this.leftHandConstraintManager.GetTargetHandTransform().right * this.handIK_ConstraintSCRP.hintPositionOffset.x);
        }
    }

    protected TwoBoneIK_ConstraintSCRP handIK_ConstraintSCRP;
    protected Transform rightHandTransform;
    protected HandArmIKConstraintManager leftHandConstraintManager;
    protected IWeaponAdvanceUser weaponAdvanceUser;
    public WeaponLeftHandGripHandConstraintNodeLeaf(
        Func<bool> precondition
        , Transform rightHandTransform
        , HandArmIKConstraintManager leftHandConstraintManager
        ,  TwoBoneIK_ConstraintSCRP handIK_ConstraintSCRP
        , IWeaponAdvanceUser weaponAdvanceUser
        ) : base(precondition)
    {
        this.handIK_ConstraintSCRP = handIK_ConstraintSCRP;
        this.rightHandTransform = rightHandTransform;
        this.leftHandConstraintManager = leftHandConstraintManager;
        this.weaponAdvanceUser = weaponAdvanceUser;
    }
    public override void UpdateNode()
    {
        this.UpdateLeftHandGripPosition();
        base.UpdateNode();
    }

    protected void UpdateLeftHandGripPosition()
    {
        this.leftHandConstraintManager.SetTargetHand(this.leftHandTargetPosition, this.leftHandTargetRotation);
        this.leftHandConstraintManager.SetHintHandPosition(this.leftHandHintPosition);
    }





}
