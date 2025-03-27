using System;
using UnityEngine;


public class PrimaryToSecondarySwitchWeaponManuverLeafNode : WeaponManuverLeafNode
{
    public enum TransitionPhase
    {
        None,
        Enter,
        HolsterPrimaryWeapon,
        Switch,
        DrawSecondaryWeapon,
        Exit,
    }

    private bool isComplete;

    private float elapsTime;
    Weapon curWeapon => weaponAdvanceUser._currentWeapon;
    WeaponAfterAction weaponAfterAction;
   
    public TransitionPhase curPhase;

    private float holsterPrimaryWeaponTime = 0.3f;
    private float drawSecondaryWeaponTime = 0.3f;
    public PrimaryToSecondarySwitchWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser.weaponAfterAction;

        curPhase = TransitionPhase.None;
    }

    public override void Enter()
    {
        curPhase = TransitionPhase.Enter;
        weaponAfterAction.SwitchingWeapon(curWeapon, this);
        elapsTime = 0;
        curWeapon.ChangeActionManualy(curWeapon.restNode);
        isComplete = false;
    }

    public override void Exit()
    {
        curPhase = TransitionPhase.Exit;
        weaponAfterAction.SwitchingWeapon(curWeapon, this);
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
        Weapon curWeapon = weaponAdvanceUser._currentWeapon;

        switch (curPhase)
        {
            case TransitionPhase.Enter:
                {
                    curPhase = TransitionPhase.HolsterPrimaryWeapon;
                }
                break;
            case TransitionPhase.HolsterPrimaryWeapon:
                {
                    weaponAfterAction.SwitchingWeapon(curWeapon, this);
                    if (elapsTime >= holsterPrimaryWeaponTime)
                    {
                        curPhase = TransitionPhase.Switch;
                    }
                }
                break;
            case TransitionPhase.Switch:
                {
                    curWeapon.AttachWeaponToSocketNoneAnimatorOverride(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
                    secondaryWeapon.AttatchWeaponToNoneOverrideAnimator(weaponAdvanceUser);
                    weaponAfterAction.SwitchingWeapon(curWeapon, this);
                    curPhase = TransitionPhase.DrawSecondaryWeapon;
                } 
                break;
            case TransitionPhase.DrawSecondaryWeapon: 
                {
                    weaponAfterAction.SwitchingWeapon(curWeapon, this);
                    if (elapsTime >= holsterPrimaryWeaponTime + drawSecondaryWeaponTime)
                    {
                        curWeapon.AttatchWeaponTo(weaponAdvanceUser);
                        isComplete = true;
                    }
                } 
                break;
        }
    }
}
