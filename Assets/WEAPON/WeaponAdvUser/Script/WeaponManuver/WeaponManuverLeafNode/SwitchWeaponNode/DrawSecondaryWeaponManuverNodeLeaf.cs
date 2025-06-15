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
        WeaponAttachingBehavior weaponAttachingBehavior = new WeaponAttachingBehavior();

        elapseTime = 0;

        if (weaponAdvanceUser._currentWeapon == null)
            weaponAttachingBehavior.Attach((weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon), weaponAdvanceUser._mainHandSocket);
        else if (weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
            && weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
        {
            weaponAttachingBehavior.Detach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser);
            weaponAttachingBehavior.Attach((weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon), weaponAdvanceUser._mainHandSocket);
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
