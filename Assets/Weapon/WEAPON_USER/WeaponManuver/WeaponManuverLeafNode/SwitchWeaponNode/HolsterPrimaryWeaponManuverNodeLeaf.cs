using System;
using UnityEngine;

public class HolsterPrimaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private float duration = 0.26f;
    private float elapesTime;
    public HolsterPrimaryWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void Enter()
    {
        elapesTime = 0;
        weaponAdvanceUser.weaponAfterAction.SendFeedBackWeaponAfterAction
            <HolsterPrimaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit()
    {
        (weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon).AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
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
