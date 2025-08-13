using System;
using UnityEngine;

public class QuickSwitch_HolsterSecondaryWeapon_NodeLeaf : WeaponManuverLeafNode, IQuickSwitchNode
{
    private bool isComplete;
    private bool isHolsterSecondaryWeapon;
    private WeaponAttachingBehavior weaponAttachingBehavior;
    private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    private Weapon secondaryWeapon;
    private float timer;

    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }

    public QuickSwitch_HolsterSecondaryWeapon_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP) : base(weaponAdvanceUser, preCondition)
    {
        weaponAttachingBehavior = new WeaponAttachingBehavior();
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
    }

    public override void Enter()
    {
        timer = 0;
        isComplete = false;
        isHolsterSecondaryWeapon = false;
        secondaryWeapon = weaponAdvanceUser._currentWeapon;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit()
    {
        if(isHolsterSecondaryWeapon && isComplete == false)
        {
            weaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket, weaponAdvanceUser._mainHandSocket);
        }
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
    public override void UpdateNode()
    {

        timer += Time.deltaTime;
        if (timer >= animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.triggerNormalizedTime && isHolsterSecondaryWeapon == false)
        {
            weaponAttachingBehavior.Attach(secondaryWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket);
            isHolsterSecondaryWeapon = true;
            weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
        if (timer >= animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.endNormalizedTime)
        {
            weaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket, weaponAdvanceUser._mainHandSocket);
            isComplete = true;
            weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
    }

    public override bool IsReset()
    {
        if (weaponAdvanceUser._weaponManuverManager.isSwitchWeaponManuverAble == false)
            return true;

        return this.IsComplete();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override void FixedUpdateNode()
    {
    }

}
