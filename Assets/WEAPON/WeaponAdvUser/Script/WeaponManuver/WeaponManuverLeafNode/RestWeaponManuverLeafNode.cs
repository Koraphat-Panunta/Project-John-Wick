using System;
using UnityEngine;

public class RestWeaponManuverLeafNode : WeaponManuverLeafNode
{
    Weapon curWeapon;
    WeaponAfterAction weaponAfterAction;
    WeaponManuverManager weaponManuverManager => weaponAdvanceUser._weaponManuverManager;
    private float recoverFormAimDownSight = 2.5f;


    public RestWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.curWeapon = weaponAdvanceUser._currentWeapon;
        this.weaponAfterAction = weaponAdvanceUser._weaponAfterAction;
    }
    public override void Enter()
    {
      this.weaponAfterAction.SendFeedBackWeaponAfterAction
            <RestWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdateNode()
    {
        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight - Time.deltaTime * recoverFormAimDownSight);
    }

    public override bool IsComplete()
    {
        return true;
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool Precondition()
    {
        return true;
    }

    public override void UpdateNode()
    {
        this.weaponAfterAction.SendFeedBackWeaponAfterAction
       <RestWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
}
