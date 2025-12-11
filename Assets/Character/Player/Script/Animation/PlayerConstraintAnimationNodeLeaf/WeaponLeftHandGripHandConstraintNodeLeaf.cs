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
    private Transform rightHandBone;

    private Vector3 leftHandTargetPosition;
    private Quaternion leftHandTargetRotation;

    private Vector3 delta_RootHandPosition;

    public WeaponLeftHandGripHandConstraintNodeLeaf(
        Func<bool> precondition
        , Transform leftHandBone
        , Transform rightHandBone
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
        this.rightHandBone = rightHandBone;
    }
    public override void Enter()
    {
        this.OnAttachWeapon(this.weaponAdvanceUser._currentWeapon);
        base.Enter();
    }
    public override void UpdateNode()
    {


        if (this.attachWeapon != weaponAdvanceUser._currentWeapon)
            this.OnAttachWeapon(this.weaponAdvanceUser._currentWeapon);
        else
        {
            this.leftHandTargetPosition = this.attachWeapon._mainHandGripTransform.TransformPoint(this.delta_RootHandPosition);

            Quaternion targetRotation = this.rightHandBone.transform.rotation * this.leftHandTargetRotation;

            this.leftHandConstraintManager.SetTargetHand(this.leftHandTargetPosition, targetRotation);
        }

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

    private void OnAttachWeapon(Weapon curWeapon)
    {
        this.attachWeapon = curWeapon;

        this.leftHandTargetPosition = this.attachWeapon._SecondHandGripTransform.position;
        this.delta_RootHandPosition = this.attachWeapon._mainHandGripTransform.InverseTransformPoint(this.leftHandTargetPosition);

        this.leftHandTargetRotation = (Quaternion.Inverse(this.weaponAdvanceUser._mainHandSocket.transform.rotation) * this.rightHandBone.rotation)
            * (Quaternion.Inverse(this.attachWeapon._SecondHandGripTransform.rotation) * this.weaponAdvanceUser._mainHandSocket.transform.rotation);

    }
}
