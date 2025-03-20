using System;
using UnityEngine;

public class HolsterSecondaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private float duration = 0.26f;
    private float elapesTime;
    public HolsterSecondaryWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void Enter()
    {
        elapesTime = 0;
        weaponAdvanceUser.weaponAfterAction.SwitchingWeapon(weaponAdvanceUser.currentWeapon, this);
    }

    public override void Exit()
    {
        (weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon).AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
    }

    public override void FixedUpdateNode()
    {

    }
    public override bool IsReset()
    {
        return IsComplete();
    }
    public override bool IsComplete()
    {
        return elapesTime >= duration;
    }

    public override void UpdateNode()
    {
        this.elapesTime += Time.deltaTime;
    }
}
