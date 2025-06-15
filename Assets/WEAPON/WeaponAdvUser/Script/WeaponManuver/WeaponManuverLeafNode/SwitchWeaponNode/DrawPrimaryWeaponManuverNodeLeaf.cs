using System;
using UnityEngine;

public class DrawPrimaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private float duration = 0.26f;
    private float elapesTime;
    public DrawPrimaryWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void Enter()
    {
        elapesTime = 0;
        Debug.Log("DrawPrimaryWeaponManuverNodeLeaf");
        if(weaponAdvanceUser._currentWeapon == null)
            (weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon).AttatchWeaponTo(weaponAdvanceUser);
        else if(weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon 
            && weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
        {
            weaponAdvanceUser._currentWeapon.DropWeapon();
            (weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon).AttatchWeaponTo(weaponAdvanceUser);
        }
        else
        {
            throw new Exception("DrawPrimaryWeaponManuver corrupt");
        }
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <DrawPrimaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);
    }

    public override void Exit()
    {
        
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
