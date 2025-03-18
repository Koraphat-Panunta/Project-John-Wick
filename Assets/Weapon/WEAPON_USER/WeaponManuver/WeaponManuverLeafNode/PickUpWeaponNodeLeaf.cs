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
        if(weaponAdvanceUser.currentWeapon != null
            &&weaponAdvanceUser.currentWeapon is PrimaryWeapon )
        {
            weaponAdvanceUser.currentWeapon.AttachWeaponTo(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
            weaponAdvanceUser.currentWeapon = null;
        }
        else if(weaponAdvanceUser.currentWeapon != null
            && weaponAdvanceUser.currentWeapon is SecondaryWeapon)
        {
            weaponAdvanceUser.currentWeapon.AttachWeaponTo(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
            weaponAdvanceUser.currentWeapon=null;
        }
        if (weaponAdvanceUser.currentWeapon == null)
        {
            Debug.Log("Pick up weapon enter");
            isComplete = false;
            Debug.Log(findingWeaponBehavior);
            findingWeaponBehavior.weaponFindingSelecting.AttatchWeaponTo(weaponAdvanceUser);
        }
        isComplete = true;
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
