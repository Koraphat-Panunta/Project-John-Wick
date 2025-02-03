using System;
using UnityEngine;

public class SecondaryToPrimarySwitchWeaponManuverLeafNode : WeaponManuverLeafNode
{

    private bool isComplete;

    private float elapsTime;
    Weapon curWeapon => weaponAdvanceUser.currentWeapon;
    WeaponAfterAction weaponAfterAction;

    TransitionPhase curPhase;

    private float holsterSecondaryWeaponTime = 0.5f;
    public SecondaryToPrimarySwitchWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser.weaponAfterAction;

        curPhase = TransitionPhase.None;
    }

    public override void Enter()
    {
        weaponAfterAction.SwitchingWeapon(curWeapon, WeaponTransition.SecondaryToPrimary);
        curPhase = TransitionPhase.HolsterSecondaryEnter;
        curWeapon.ChangeActionManualy(curWeapon.restNode);
        elapsTime = 0;
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
            case TransitionPhase.HolsterSecondaryEnter:
                {
                    //curWeapon.AttachWeaponToSecondHand(weaponAdvanceUser.leftHandSocket);
                    curPhase = TransitionPhase.HolsteringSecondary;
                }
                break;
            case TransitionPhase.HolsteringSecondary:
                {
                    if (elapsTime > holsterSecondaryWeaponTime)
                    {
                        curWeapon.AttachWeaponTo(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
                        curPhase = TransitionPhase.DrawPrimaryEnter;
                    }
                }
                break;
            case TransitionPhase.DrawPrimaryEnter:
                {
                    curWeapon = primaryWeapon;
                    primaryWeapon.AttatchWeaponTo(weaponAdvanceUser);
                    curPhase = TransitionPhase.DrawingPrimary;
                }
                break;
            case TransitionPhase.DrawingPrimary:
                {
                    if (elapsTime >= (1 / curWeapon.drawSpeed) + holsterSecondaryWeaponTime)
                        isComplete = true;
                }
                break;
        }
    }
}
