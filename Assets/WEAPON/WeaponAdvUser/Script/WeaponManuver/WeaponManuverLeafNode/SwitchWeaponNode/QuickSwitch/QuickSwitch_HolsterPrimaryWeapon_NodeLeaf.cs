using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSwitch_HolsterPrimaryWeapon_NodeLeaf : WeaponManuverLeafNode,IQuickSwitchNode,INodeLeafTransitionAble
{
    private bool isComplete;
    private bool isHolsterPrimaryWeapon;
    private AnimationTriggerEventSCRP animationTriggerEventSCRP;
    private Weapon secondHandWeapon;
    private float timer;

    public INodeManager nodeManager { get => weaponAdvanceUser._weaponManuverManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    public IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble { get; set; }
    private AnimationTriggerEventPlayer animationTriggerEventPlayer { get; set; }
    public QuickSwitch_HolsterPrimaryWeapon_NodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,IQuickSwitchWeaponManuverAble quickSwitchWeaponManuverAble, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP) : base(weaponAdvanceUser, preCondition)
    {

        this.quickSwitchWeaponManuverAble = quickSwitchWeaponManuverAble;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent(animationTriggerEventSCRP.triggerEventDetail[0].eventName,HolsterPrimary);
    }

    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
        timer = 0;
        isComplete = false;
        isHolsterPrimaryWeapon = false;
        secondHandWeapon = weaponAdvanceUser._secondHandSocket.curWeaponAtSocket;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit()
    {
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
    public override void UpdateNode()
    {
        this.TransitioningCheck();

        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

    }

    private void HolsterPrimary()
    {
        WeaponAttachingBehavior.Attach(secondHandWeapon, weaponAdvanceUser._weaponBelt.primaryWeaponSocket);
        isHolsterPrimaryWeapon = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        nodeLeafTransitionBehavior.TransitionAbleAll(this);
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

    public override void FixedUpdateNode()
    {
    }

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);


    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this,node);
   
}
