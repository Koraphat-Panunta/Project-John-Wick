using System;
using UnityEngine;

public class PickUpWeaponNodeLeaf : WeaponManuverLeafNode
{
    private FindingWeaponBehavior findingWeaponBehavior => weaponAdvanceUser._findingWeaponBehavior;
    private bool isComplete;
    public PickUpWeaponNodeLeaf(IRangeWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }
    public override void Enter()
    {
        isComplete = false;

        if (findingWeaponBehavior.weaponFindingSelecting is PrimaryWeapon && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null)
            WeaponAttachingBehavior.Detach(weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon,weaponAdvanceUser);

        if (findingWeaponBehavior.weaponFindingSelecting is SecondaryWeapon && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null)
            WeaponAttachingBehavior.Detach(weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon, weaponAdvanceUser);

        if(weaponAdvanceUser._currentWeapon != null)
        {
            if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon)
                WeaponAttachingBehavior.Attach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket, WeaponMountComponent.attatchingDurationGlobal);
            else if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon)
                WeaponAttachingBehavior.Attach(weaponAdvanceUser._currentWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket, WeaponMountComponent.attatchingDurationGlobal);
        }

        WeaponAttachingBehavior.Attach(findingWeaponBehavior.weaponFindingSelecting, weaponAdvanceUser._mainHandSocket, WeaponMountComponent.attatchingDurationGlobal);
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
