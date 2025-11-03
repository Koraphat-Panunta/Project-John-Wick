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
        weaponAfterAction = weaponAdvanceUser._weaponAfterAction;

        curPhase = TransitionPhase.None;
    }

    public override void Enter()
    {
        curPhase = TransitionPhase.Enter;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <PrimaryToSecondarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        elapsTime = 0;
        isComplete = false;
    }

    public override void Exit()
    {
        curPhase = TransitionPhase.Exit;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <PrimaryToSecondarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
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
                    curPhase = TransitionPhase.HolsterPrimaryWeapon;
                }
                break;
            case TransitionPhase.HolsterPrimaryWeapon:
                {
                    weaponAfterAction.SendFeedBackWeaponAfterAction
                        <PrimaryToSecondarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
                    if (elapsTime >= holsterPrimaryWeaponTime)
                    {
                        curPhase = TransitionPhase.Switch;
                    }
                }
                break;
            case TransitionPhase.Switch:
                {

                    WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
                    WeaponAttachingBehavior.Attach(secondaryWeapon, weaponAdvanceUser._mainHandSocket);
                    weaponAfterAction.SendFeedBackWeaponAfterAction
                        <PrimaryToSecondarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
                    curPhase = TransitionPhase.DrawSecondaryWeapon;
                } 
                break;
            case TransitionPhase.DrawSecondaryWeapon: 
                {
                    weaponAfterAction.SendFeedBackWeaponAfterAction
                        <PrimaryToSecondarySwitchWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
                    if (elapsTime >= holsterPrimaryWeaponTime + drawSecondaryWeaponTime)
                    {
                        WeaponAttachingBehavior.Attach(curWeapon, weaponAdvanceUser._mainHandSocket);
                        isComplete = true;
                    }
                } 
                break;
        }
    }
}
