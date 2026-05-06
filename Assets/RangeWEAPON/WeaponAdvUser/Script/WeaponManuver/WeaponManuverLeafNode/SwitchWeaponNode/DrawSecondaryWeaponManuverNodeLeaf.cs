using System;
using UnityEngine;

public class DrawSecondaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private float duration = 0.25f;
    private float elapseTime;
    
    public DrawSecondaryWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void Enter()
    {

        elapseTime = 0;

        if (weaponAdvanceUser._currentWeapon == null)
            WeaponAttachingBehavior.Attach((weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon), weaponAdvanceUser._mainHandSocket, WeaponMountComponent.attatchingDurationGlobal);
        else if (weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon
            && weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon)
        {
            WeaponAttachingBehavior.Detach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser);
            WeaponAttachingBehavior.Attach((weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon), weaponAdvanceUser._mainHandSocket, WeaponMountComponent.attatchingDurationGlobal);
        }
        else
        {
            throw new Exception("DrawSecondaryWeaponManuver corrupt");
        }
        weaponAdvanceUser._weaponAfterAction.
            SendFeedBackWeaponAfterAction<DrawSecondaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return elapseTime > duration;
    }

    public override void UpdateNode()
    {
        elapseTime += Time.deltaTime;
    }
}
