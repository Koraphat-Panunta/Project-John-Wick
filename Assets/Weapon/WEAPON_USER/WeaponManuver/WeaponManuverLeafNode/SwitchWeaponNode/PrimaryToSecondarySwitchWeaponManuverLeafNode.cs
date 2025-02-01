using System;
using UnityEngine;


public class PrimaryToSecondarySwitchWeaponManuverLeafNode : WeaponManuverLeafNode
{


    private bool isComplete;

    private float elapsTime;
    Weapon curWeapon => weaponAdvanceUser.currentWeapon;
    WeaponAfterAction weaponAfterAction;
   
    TransitionPhase curPhase;

    private float holsterPrimaryWeaponTime = 0.5f;
    public PrimaryToSecondarySwitchWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser.weaponAfterAction;

        curPhase = TransitionPhase.None;
    }

    public override void Enter()
    {
        weaponAfterAction.SwitchingWeapon(curWeapon, WeaponTransition.PrimaryToSecondary);
        curPhase = TransitionPhase.HolsterPrimaryEnter;
        elapsTime = 0;
        curWeapon.ChangeActionManualy(curWeapon.restNode);
        isComplete = false;
    }

    public override void Exit()
    {
        curPhase = TransitionPhase.None;
        elapsTime = 0;
    }

    public override void FixedUpdateNode()
    {
    }

    public override bool IsComplete()
    {
        return isComplete;
    }
    public override bool IsReset()
    {
        return IsComplete();
    }
    public override void UpdateNode()
    {
        elapsTime += Time.deltaTime;

        Weapon primaryWeapon = weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon;
        Weapon secondaryWeapon = weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon;
        Weapon curWeapon = weaponAdvanceUser.currentWeapon;

        switch (curPhase)
        {
            case TransitionPhase.HolsterPrimaryEnter:
                {
                    curPhase = TransitionPhase.HolsteringPrimary;
                }
                break;
            case TransitionPhase.HolsteringPrimary:
                {
                    if (elapsTime >= holsterPrimaryWeaponTime)
                    {
                        curWeapon.AttachWeaponTo(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
                        curPhase = TransitionPhase.DrawSecondaryEnter;
                    }
                }
                break;
            case TransitionPhase.DrawSecondaryEnter:
                {
                    secondaryWeapon.AttatchWeaponTo(weaponAdvanceUser);
                    curPhase = TransitionPhase.DrawingSecondary;
                } 
                break;
            case TransitionPhase.DrawingSecondary: 
                {
                    if(elapsTime >= (1/curWeapon.drawSpeed) + holsterPrimaryWeaponTime)
                        isComplete = true;
                } 
                break;
        }
    }
}
