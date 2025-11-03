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
        weaponAfterAction = weaponAdvanceUser._weaponAfterAction;

        curPhase = TransitionPhase.None;
    }

    public override void Enter()
    {
        curPhase = TransitionPhase.Enter;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <SecondaryToPrimarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        elapsTime = 0;
        isComplete = false;
    }

    public override void Exit()
    {
        curPhase = TransitionPhase.Exit;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <SecondaryToPrimarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
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

        Weapon primaryWeapon = weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon;
        Weapon secondaryWeapon = weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon;
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
                    weaponAfterAction.SendFeedBackWeaponAfterAction
                        <SecondaryToPrimarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

                    if (elapsTime > holsterSecondaryWeaponTime)
                    curPhase = TransitionPhase.Switch;
                    
                }
                break;
            case TransitionPhase.Switch:
                {
                    WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket);
                    WeaponAttachingBehavior.Attach(primaryWeapon, weaponAdvanceUser._mainHandSocket);
                    weaponAfterAction.SendFeedBackWeaponAfterAction
                        <SecondaryToPrimarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

                    curPhase = TransitionPhase.GripingPrimaryWeapon;
                }
                break;
            case TransitionPhase.GripingPrimaryWeapon:
                {

                    weaponAfterAction.SendFeedBackWeaponAfterAction
                        <SecondaryToPrimarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

                    if (elapsTime >= /*(1 / curWeapon.drawSpeed)*/ holsterSecondaryWeaponTime + drawPrimaryWeaponTime)
                    {
                        WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._mainHandSocket);
                        isComplete = true;
                    }
                }
                break;
        }
    }
}
