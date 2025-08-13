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

        

        if (findingWeaponBehavior.weaponFindingSelecting is PrimaryWeapon && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null)
            weaponAttachingBehavior.Detach(weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon,weaponAdvanceUser);

        if (findingWeaponBehavior.weaponFindingSelecting is SecondaryWeapon && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null)
            weaponAttachingBehavior.Detach(weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon, weaponAdvanceUser);

        if(weaponAdvanceUser._currentWeapon != null)
        {
            if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon)
                weaponAttachingBehavior.Attach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
            else if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
                weaponAttachingBehavior.Attach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket);
        }

        weaponAttachingBehavior.Attach(findingWeaponBehavior.weaponFindingSelecting, weaponAdvanceUser._mainHandSocket);
        isComplete = true;

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
