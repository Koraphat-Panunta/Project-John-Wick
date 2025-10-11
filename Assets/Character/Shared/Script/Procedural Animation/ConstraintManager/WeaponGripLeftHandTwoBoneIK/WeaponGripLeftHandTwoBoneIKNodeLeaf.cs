using System;
using UnityEngine;

public class WeaponGripLeftHandTwoBoneIKNodeLeaf : AnimationConstrainNodeLeaf
{
    private Transform referenceTransform;
    private IWeaponAdvanceUser weaponAdvanceUser;
    private Transform secondHandGripTransform => weaponAdvanceUser._currentWeapon._SecondHandGripTransform;
    private HandArmIKConstraintManager leftHandConstraintManager;
    private WeaponGripLeftHandScriptableObject weaponGripLeftHandScriptableObject;

    private Weapon attachWeapon;

    private Vector3 hintPosition;

    private float enterWeightSpeed = 5;
    public WeaponGripLeftHandTwoBoneIKNodeLeaf(
        Func<bool> precondition
        , Transform referenceTransform
        , HandArmIKConstraintManager leftHandConstraintManager
        , WeaponGripLeftHandScriptableObject weaponGripLeftHandScriptableObject
        ,IWeaponAdvanceUser weaponAdvanceUser
        ) : base(precondition)
    {
        this.referenceTransform = referenceTransform;
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.leftHandConstraintManager = leftHandConstraintManager;
        this.weaponGripLeftHandScriptableObject = weaponGripLeftHandScriptableObject;
    }
    public override void Enter()
    {
        this.leftHandConstraintManager.SetTargetHandParentConstraint(this.secondHandGripTransform);
        this.attachWeapon = weaponAdvanceUser._currentWeapon;
        base.Enter();
    }
    public override void UpdateNode()
    {
        if(this.attachWeapon != weaponAdvanceUser._currentWeapon)
        {
            this.leftHandConstraintManager.SetTargetHandParentConstraint(this.secondHandGripTransform);
            this.attachWeapon = weaponAdvanceUser._currentWeapon;
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
}
