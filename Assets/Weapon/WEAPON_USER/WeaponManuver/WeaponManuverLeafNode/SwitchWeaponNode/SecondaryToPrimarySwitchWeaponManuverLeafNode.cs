using System;
using UnityEngine;

public class SecondaryToPrimarySwitchWeaponManuverLeafNode : WeaponManuverLeafNode,IWeaponTransitionNodeLeaf
{

    private bool isComplete;

    private float elapsTime;
    Weapon curWeapon => weaponAdvanceUser._currentWeapon;
    WeaponAfterAction weaponAfterAction;
    public enum TransitionPhase
    {
        None,
        Enter,
        DrawPrimaryWeapon,
        HolsterSecondaryWeapon,
        Switch,
        GripingPrimaryWeapon,
        Exit,
    }

    public TransitionPhase curPhase;

    private float holsterSecondaryWeaponTime = 0.5f;
    private float drawPrimaryWeaponTime = 0.33f;
    public SecondaryToPrimarySwitchWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser.weaponAfterAction;

        curPhase = TransitionPhase.None;
    }

    public override void Enter()
    {
        curPhase = TransitionPhase.Enter;
        weaponAfterAction.SwitchingWeapon(curWeapon, this);
        curWeapon.ChangeActionManualy(curWeapon.restNode);
        elapsTime = 0;
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
                    curPhase = TransitionPhase.DrawPrimaryWeapon;
                }
                break;
            case TransitionPhase.DrawPrimaryWeapon:
                {
                    weaponAfterAction.SwitchingWeapon(curWeapon, this);

                    if (elapsTime > holsterSecondaryWeaponTime)
                    curPhase = TransitionPhase.Switch;
                    
                }
                break;
            case TransitionPhase.Switch:
                {
                    curWeapon.AttachWeaponToSocketNoneAnimatorOverride(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
                    curWeapon = primaryWeapon;
                    primaryWeapon.AttatchWeaponToNoneOverrideAnimator(weaponAdvanceUser);

                    weaponAfterAction.SwitchingWeapon(curWeapon, this);

                    curPhase = TransitionPhase.GripingPrimaryWeapon;
                }
                break;
            case TransitionPhase.GripingPrimaryWeapon:
                {
                    weaponAdvanceUser._currentWeapon.AttatchWeaponTo(weaponAdvanceUser);
                    weaponAfterAction.SwitchingWeapon(curWeapon, this);

                    if (elapsTime >= /*(1 / curWeapon.drawSpeed)*/ holsterSecondaryWeaponTime + drawPrimaryWeaponTime)
                        isComplete = true;
                }
                break;
        }
    }
}
