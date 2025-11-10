using System;
using UnityEngine;
using UnityEngine.Events;

public class QuickSwitch_Draw_NodeLeaf : WeaponManuverLeafNode,IQuickSwitchNode
{
    private bool isComplete;
    private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    private bool isDrawSecondary;
    private Weapon secondHandWeapon;

    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get ; set ; }
    private TransformOffsetSCRP quickSwitchHoldOffset;

    private AnimationTriggerEventPlayer animationTriggerEventPlayer;

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

        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(this.animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent(animationTriggerEventSCRP.triggerEventDetail[0].eventName, Draw);

    }
  
    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
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
        Debug.Log("Before draw");
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
    }
    public override void FixedUpdateNode()
    {

    }
    private void Draw()
    {
        Debug.Log("Draw timer = "+animationTriggerEventPlayer.timer);
        isDrawSecondary = true;
        WeaponAttachingBehavior.Attach(weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon, weaponAdvanceUser._mainHandSocket);
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_Draw_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
    public override bool IsReset()
    {
        if(weaponAdvanceUser._weaponManuverManager.isSwitchWeaponManuverAble == false)
            return true;

        return this.IsComplete();
    }
    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }

   
}
