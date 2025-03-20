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
        if (weaponAdvanceUser.currentWeapon == null)
        {
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
        else if (weaponAdvanceUser.currentWeapon != weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon
            && weaponAdvanceUser.currentWeapon != weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon)
        {
            weaponAdvanceUser.currentWeapon.DropWeapon();
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
        else if(weaponAdvanceUser.currentWeapon != null
            &&weaponAdvanceUser.currentWeapon is PrimaryWeapon )
        {
            weaponAdvanceUser.currentWeapon.AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
            isComplete = true;
            return;
        }
        else if(weaponAdvanceUser.currentWeapon != null
            && weaponAdvanceUser.currentWeapon is SecondaryWeapon)
        {
            weaponAdvanceUser.currentWeapon.AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
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
