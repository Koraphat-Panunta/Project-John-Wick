using System;
using UnityEngine;

public class PickUpWeaponNodeLeaf : WeaponManuverLeafNode
{
    private FindingWeaponBehavior findingWeaponBehavior => weaponAdvanceUser.findingWeaponBehavior;
    private bool isComplete;
    public PickUpWeaponNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }
    public override void Enter()
    {
        isComplete = false;
        if (weaponAdvanceUser._currentWeapon == null)
        {
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
        else if (weaponAdvanceUser._currentWeapon != weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon
            && weaponAdvanceUser._currentWeapon != weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon)
        {
            weaponAdvanceUser._currentWeapon.DropWeapon();
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
        else if(weaponAdvanceUser._currentWeapon != null
            &&weaponAdvanceUser._currentWeapon is PrimaryWeapon )
        {
            weaponAdvanceUser._currentWeapon.AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
        else if(weaponAdvanceUser._currentWeapon != null
            && weaponAdvanceUser._currentWeapon is SecondaryWeapon)
        {
            weaponAdvanceUser._currentWeapon.AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
       
   
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateNode()
    {
        
    }
    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        return false;
        //return base.IsReset();
    }
    public override bool IsComplete()
    {
       return isComplete;
    }

    public override void UpdateNode()
    {
    }
}
