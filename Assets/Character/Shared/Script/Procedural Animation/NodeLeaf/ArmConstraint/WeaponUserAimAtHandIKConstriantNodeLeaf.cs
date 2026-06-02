using System;
using UnityEngine;

public class WeaponUserAimAtHandIKConstriantNodeLeaf : AimAtHandIKConstriantNodeLeaf
{
    private readonly HandRecoilModifier recoilModifier = new HandRecoilModifier();
    private readonly HandBlockModifier blockModifier = new HandBlockModifier();

    protected IRangeWeaponAdvanceUser weaponAdvanceUser;

    private WeaponHandIK_ConstraintSCRP weaponSCRP => base.handIK_ConstraintSCRP as WeaponHandIK_ConstraintSCRP;
    private WeaponHandRecoilSCRP recoilData => this.weaponSCRP != null ? this.weaponSCRP.recoilData : null;
    private WeaponHandBlockSCRP blockData => this.weaponSCRP != null ? this.weaponSCRP.blockData : null;

    private Vector3 handPos => this.weaponAdvanceUser._character.humanoidBone._rightHandBone.position;
    private Transform recoilDir => this.weaponAdvanceUser._currentWeapon.bulletSpawner.transform;

    public override Vector3 targetHandPosition
    {
        get
        {
            Vector3 pos = base.targetHandPosition;
            pos = this.blockModifier.ApplyPosition(pos, this.handIK_Transform_Ref_Pos.position, this.handIK_Transform_Ref_Rot.forward, this.handIK_Transform_Ref_Rot.right, this.handIK_Transform_Ref_Rot.up, this.blockData);
            pos = this.recoilModifier.ApplyPosition(pos, this.recoilDir, this.recoilData);
            return pos;
        }
    }
    public override Quaternion targetHandRotation
    {
        get
        {
            Quaternion rot = base.targetHandRotation;
            rot = this.blockModifier.ApplyRotation(rot, this.targetHandPosition, this.aimingAtTransfrom.position, this.handIK_Transform_Ref_Rot.up, this.blockData);
            rot = this.recoilModifier.ApplyRotation(rot, this.recoilData);
            return rot;
        }
    }
    public override Vector3 targetHintHandPosition
    {
        get
        {
            return this.blockModifier.ApplyHint(base.targetHintHandPosition, this.rootHintHandTransform.position, this.rootHintHandTransform, this.blockData);
        }
    }

    public WeaponUserAimAtHandIKConstriantNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform aimingAtTransform
        , Transform handIK_Transform_Ref_Pos
        , Transform handIK_Transform_Ref_Rot
        , Transform rootHintHand
        , Transform rootCharacter
        , IRangeWeaponAdvanceUser weaponAdvanceUser
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
        this.blockModifier.Reset();
        this.recoilModifier.Reset();
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.recoilModifier.UpdateWeights(this.recoilData);
        this.blockModifier.UpdateBlocking(this.handIK_Transform_Ref_Pos.position, base.aimDirConstriant, this.recoilDir.position, this.blockData);
        base.UpdateNode();
    }

    public void TriggeRecoilWeight(float weight) => this.recoilModifier.Trigger(weight);
}
