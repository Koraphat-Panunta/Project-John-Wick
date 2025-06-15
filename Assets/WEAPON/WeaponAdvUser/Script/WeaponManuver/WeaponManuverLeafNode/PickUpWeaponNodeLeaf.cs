using System;
using UnityEngine;

public class PickUpWeaponNodeLeaf : WeaponManuverLeafNode
{
    private FindingWeaponBehavior findingWeaponBehavior => weaponAdvanceUser._findingWeaponBehavior;
    private bool isComplete;
    private WeaponAttachingBehavior weaponAttachingBehavior = new WeaponAttachingBehavior();
    public PickUpWeaponNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }
    public override void Enter()
    {
        isComplete = false;
        if (weaponAdvanceUser._currentWeapon == null)
        {
            weaponAttachingBehavior.Attach(findingWeaponBehavior.weaponFindingSelecting, weaponAdvanceUser._mainHandSocket);
            isComplete = true;
        }
        else if (weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
            && weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
        {
            weaponAttachingBehavior.Detach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser);
            weaponAttachingBehavior.Attach(findingWeaponBehavior.weaponFindingSelecting, weaponAdvanceUser._mainHandSocket);
            isComplete = true;
        }
        else if(weaponAdvanceUser._currentWeapon != null
            &&weaponAdvanceUser._currentWeapon is PrimaryWeapon )
        {
            weaponAttachingBehavior.Attach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
            weaponAttachingBehavior.Attach(findingWeaponBehavior.weaponFindingSelecting, weaponAdvanceUser._mainHandSocket);
            isComplete = true;
        }
        else if(weaponAdvanceUser._currentWeapon != null
            && weaponAdvanceUser._currentWeapon is SecondaryWeapon)
        {
            weaponAttachingBehavior.Attach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket);
            weaponAttachingBehavior.Attach(findingWeaponBehavior.weaponFindingSelecting, weaponAdvanceUser._mainHandSocket);
            isComplete = true;
        }

        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <PickUpWeaponNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
       
   
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
