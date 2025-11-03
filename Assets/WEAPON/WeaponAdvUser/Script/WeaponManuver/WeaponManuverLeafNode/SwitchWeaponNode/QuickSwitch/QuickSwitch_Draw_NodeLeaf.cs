using System;
using UnityEngine;

public class QuickSwitch_Draw_NodeLeaf : WeaponManuverLeafNode,IQuickSwitchNode
{
    private bool isComplete;
    private float elapseTime;
    private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    private bool isDrawSecondary;
    private Weapon secondHandWeapon;

    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get ; set ; }
    private TransformOffsetSCRP quickSwitchHoldOffset;

    public QuickSwitch_Draw_NodeLeaf(
        IWeaponAdvanceUser weaponAdvanceUser
        , IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble
        , Func<bool> preCondition
        , AnimationTriggerEventSCRP animationTriggerEventSCRP
        , TransformOffsetSCRP quickSwitchHoldSCRP) : base(weaponAdvanceUser, preCondition)
    {
        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        this.quickSwitchHoldOffset = quickSwitchHoldSCRP;
    }

    public override void Enter()
    {
        elapseTime = 0;
        this.secondHandWeapon = this.weaponAdvanceUser._currentWeapon;
        WeaponAttachingBehavior.Attach(
            secondHandWeapon
            , weaponAdvanceUser._secondHandSocket
            , quickSwitchHoldOffset.postitionOffset
            , Quaternion.Euler(quickSwitchHoldOffset.rotationEulerOffset));
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_Draw_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);
        isDrawSecondary = false;
        isComplete = false;
    }

    public override void Exit()
    {
        if(isDrawSecondary == false)
        {
            WeaponAttachingBehavior.Attach(secondHandWeapon,weaponAdvanceUser._mainHandSocket);
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_Draw_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
    }
    public override void UpdateNode()
    {
        elapseTime += Time.deltaTime;
        if(elapseTime >= animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.triggerNormalizedTime && isDrawSecondary == false)
        {
            WeaponAttachingBehavior.Attach(weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon, weaponAdvanceUser._mainHandSocket);
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_Draw_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
            isDrawSecondary = true;
        }
        if(elapseTime >= animationTriggerEventSCRP.clip.length * animationTriggerEventSCRP.endNormalizedTime)
            isComplete = true;
    }
    public override void FixedUpdateNode()
    {

    }
    public override bool IsReset()
    {
        if(weaponAdvanceUser._weaponManuverManager.isSwitchWeaponManuverAble == false)
            return true;

        return this.IsComplete();
    }
    public override bool IsComplete()
    {
        return isComplete;
    }

   
}
