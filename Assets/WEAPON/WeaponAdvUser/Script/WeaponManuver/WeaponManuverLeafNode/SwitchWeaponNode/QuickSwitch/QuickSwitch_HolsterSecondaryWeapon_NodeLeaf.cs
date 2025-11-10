using System;
using UnityEngine;

public class QuickSwitch_HolsterSecondaryWeapon_NodeLeaf : WeaponManuverLeafNode, IQuickSwitchNode
{
    private bool isComplete;
    private bool isHolsterSecondaryWeapon;
    private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    private Weapon secondaryWeapon;
    private float timer;
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;

    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }

    public QuickSwitch_HolsterSecondaryWeapon_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP) : base(weaponAdvanceUser, preCondition)
    {
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;

        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent(animationTriggerEventSCRP.triggerEventDetail[0].eventName,HolsterSecondary);
        this.animationTriggerEventPlayer.SubscribeEvent(animationTriggerEventSCRP.triggerEventDetail[1].eventName, DrawPrimary);
    }

    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
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
            WeaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket, weaponAdvanceUser._mainHandSocket);
        }
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
    public override void UpdateNode()
    {

        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
    }

    private void HolsterSecondary()
    {
        WeaponAttachingBehavior.Attach(secondaryWeapon, weaponAdvanceUser._weaponBelt.secondaryWeaponSocket);
        isHolsterSecondaryWeapon = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
    private void DrawPrimary()
    {
        WeaponAttachingBehavior.Attach(weaponAdvanceUser._secondHandSocket.curWeaponAtSocket, weaponAdvanceUser._mainHandSocket);
        isComplete = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override bool IsReset()
    {
        if (weaponAdvanceUser._weaponManuverManager.isSwitchWeaponManuverAble == false)
            return true;

        return this.IsComplete();
    }

    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }

    public override void FixedUpdateNode()
    {
    }

}
