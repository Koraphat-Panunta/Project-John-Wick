using System;
using UnityEngine;

public class WeaponLeftHandGripHandConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    private Transform referenceTransform;
    private IWeaponAdvanceUser weaponAdvanceUser;
    private Transform secondHandGripTransform => weaponAdvanceUser._currentWeapon._SecondHandGripTransform;
    private HandArmIKConstraintManager leftHandConstraintManager;
    private WeaponGripLeftHandScriptableObject weaponGripLeftHandScriptableObject;

    private Weapon attachWeapon;

    private Vector3 hintPosition;

    private float enterWeightSpeed = 5;

    private Transform leftHandBone;
    public WeaponLeftHandGripHandConstraintNodeLeaf(
        Func<bool> precondition
        , Transform leftHandBone
        , Transform referenceTransform
        , HandArmIKConstraintManager leftHandConstraintManager
        , WeaponGripLeftHandScriptableObject weaponGripLeftHandScriptableObject
        , IWeaponAdvanceUser weaponAdvanceUser
        ) : base(precondition)
    {
        this.referenceTransform = referenceTransform;
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.leftHandConstraintManager = leftHandConstraintManager;
        this.weaponGripLeftHandScriptableObject = weaponGripLeftHandScriptableObject;
        this.leftHandBone = leftHandBone;
    }
    public override void Enter()
    {
        this.attachWeapon = weaponAdvanceUser._currentWeapon;
        base.Enter();
    }
    public override void UpdateNode()
    {
        Vector3 setPos = this.secondHandGripTransform.position
            - (this.weaponAdvanceUser._secondHandSocket.weaponAttachingAbleTransform.position - this.leftHandBone.position);
        setPos = setPos
            + (this.secondHandGripTransform.forward * weaponGripLeftHandScriptableObject.leftHandGripPositionOffset.z)
            + (this.secondHandGripTransform.right * weaponGripLeftHandScriptableObject.leftHandGripPositionOffset.x)
            + (this.secondHandGripTransform.up * weaponGripLeftHandScriptableObject.leftHandGripPositionOffset.y);

        Quaternion setRot = this.secondHandGripTransform.rotation * (Quaternion.Inverse(this.weaponAdvanceUser._secondHandSocket.transform.rotation) * this.leftHandBone.rotation);
        setRot = setRot * Quaternion.Euler(weaponGripLeftHandScriptableObject.leftHandGripRotationOffset);

        if (this.attachWeapon != weaponAdvanceUser._currentWeapon)
            this.attachWeapon = weaponAdvanceUser._currentWeapon;
        else
            this.leftHandConstraintManager.SetTargetHand(setPos, setRot);

        this.hintPosition = this.referenceTransform.position
            +
            (
            this.referenceTransform.right * this.weaponGripLeftHandScriptableObject.hintTargetPositionAdditionOffset.x
            )
            +
            (
            this.referenceTransform.up * this.weaponGripLeftHandScriptableObject.hintTargetPositionAdditionOffset.y
            )
            +
            (
            this.referenceTransform.forward * this.weaponGripLeftHandScriptableObject.hintTargetPositionAdditionOffset.z
            );

        this.leftHandConstraintManager.SetHintHandPosition(hintPosition);

        this.leftHandConstraintManager.SetWeight(this.leftHandConstraintManager.GetWeight() + (this.enterWeightSpeed * Time.deltaTime));
        base.UpdateNode();
    }
    public override void Exit()
    {
        this.leftHandConstraintManager.RemoveTargetHandParentConstraint();
        this.attachWeapon = null;
        base.Exit();
    }
}
