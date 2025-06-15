using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class QuickDrawWeaponManuverLeafNode : WeaponManuverLeafNode
{
    Weapon weapon => weaponAdvanceUser._currentWeapon;
    private Weapon secondHandWeapon;
    public enum QuickDrawPhase
    {
        Draw,
        Stay,
        HolsterSecondary,
        HolsterPrimary,
        Exit
    }
    public QuickDrawPhase quickDrawPhase { get; protected set; }
    private bool isComplete;
    WeaponManuverManager weaponManuverManager => weaponAdvanceUser._weaponManuverManager;
    protected float elapseDrawTime;
    protected float elapseHolsterTime;
    public QuickDrawWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {

    }

    public override void Enter()
    {
        this.secondHandWeapon = weaponAdvanceUser._currentWeapon;
        if(weaponAdvanceUser._weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
        {
            new WeaponAttachingBehavior().Attach(weapon, weaponAdvanceUser._secondHandSocket);
            quickDrawPhase = QuickDrawPhase.Draw;
            weaponAfterActionPlayer.SendFeedBackWeaponAfterAction
                <QuickDrawWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
            weapon.ChangeActionManualy(weapon.restNode);
        }
        isComplete = false;
        elapseDrawTime = 0f;
        elapseHolsterTime = 0f;
    }

    public override void Exit()
    {
        secondHandWeapon = null;
        quickDrawPhase = QuickDrawPhase.Exit;
        if (weaponAdvanceUser._weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
            weaponAfterActionPlayer.SendFeedBackWeaponAfterAction
                <QuickDrawWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
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
        if (IsComplete())
            return true;

        if (weaponManuverManager.isReloadManuverAble 
            && weaponAdvanceUser._isReloadCommand
            && weaponAdvanceUser._currentWeapon._reloadSelecotrOverriden.Precondition())
        {
            HolsteringPrimaryWeapon();
            return true;
        }

        return false;

    }
    public override bool Precondition()
    {
        return base.Precondition();
    }

    public override void UpdateNode()
    {

        switch (quickDrawPhase)
        {
            case QuickDrawPhase.Draw:
                {
                    float drawGrab = 0.184f;
                    float drawAim = 0.34f;
                    elapseDrawTime += Time.deltaTime;

                    if (elapseDrawTime >= drawGrab)
                        new WeaponAttachingBehavior().Attach(weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon, weaponAdvanceUser._mainHandSocket);
                    
                    if (elapseDrawTime >= drawAim)
                    {
                        quickDrawPhase = QuickDrawPhase.Stay;

                        if (weaponAdvanceUser._weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
                            weaponAfterActionPlayer.SendFeedBackWeaponAfterAction
                                <QuickDrawWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
                    }
                }
                break;
            case QuickDrawPhase.Stay:
                {
                    WeaponAfterActionPlayer weaponAfterActionPlayer = weaponAdvanceUser._weaponAfterAction as WeaponAfterActionPlayer;    
                    weaponAfterActionPlayer.SendFeedBackWeaponAfterAction
                        <QuickDrawWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

                    weaponManuverManager.aimingWeight = 1;

                    if (weaponManuverManager.isSwitchWeaponManuverAble && weaponAdvanceUser._isSwitchWeaponCommand)
                    {
                        quickDrawPhase = QuickDrawPhase.HolsterSecondary;
                        weaponAfterActionPlayer.SendFeedBackWeaponAfterAction
                            <QuickDrawWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

                    }

                    if (weaponManuverManager.isPullTriggerManuverAble && weaponAdvanceUser._isPullTriggerCommand)
                        weapon.PullTrigger();

                    if(weaponManuverManager.isAimingManuverAble == false || weaponAdvanceUser._isAimingCommand == false)
                        quickDrawPhase = QuickDrawPhase.HolsterPrimary;

                }
                break;
            case QuickDrawPhase.HolsterSecondary:
                {
                    elapseHolsterTime += Time.deltaTime;

                    float holsterSecondary = 0.34f;
                    float drawPrimary = 0.42f;

                    if (elapseHolsterTime >= holsterSecondary && weapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon) 
                        new WeaponAttachingBehavior().Attach(weapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket);

                    if (elapseHolsterTime >= drawPrimary)
                    {
                        new WeaponAttachingBehavior().Attach(this.secondHandWeapon,weaponAdvanceUser._mainHandSocket);
                        isComplete = true;
                    }
                }
                break;
            case QuickDrawPhase.HolsterPrimary: 
                {
                    HolsteringPrimaryWeapon();
                    isComplete= true;
                }
                break;
        }
    }

    private void HolsteringPrimaryWeapon()//++
    {
        if (this.secondHandWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon)
        {
            new WeaponAttachingBehavior().Attach(this.secondHandWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
        }
        else
            new WeaponAttachingBehavior().Detach(this.secondHandWeapon, weaponAdvanceUser);

        new WeaponAttachingBehavior().Attach(weapon, weaponAdvanceUser._mainHandSocket);

        if (weaponAdvanceUser._weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
            weaponAfterActionPlayer.SendFeedBackWeaponAfterAction
                <QuickDrawWeaponManuverLeafNode>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

   
}
