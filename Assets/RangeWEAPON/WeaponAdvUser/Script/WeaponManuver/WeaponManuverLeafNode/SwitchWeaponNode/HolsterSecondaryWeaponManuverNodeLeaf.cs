using System;
using System.Collections.Generic;
using UnityEngine;

public class HolsterSecondaryWeaponManuverNodeLeaf : WeaponManuverLeafNode, INodeLeafTransitionAble
{
    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; } = new Dictionary<INode, bool>();
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; } = new NodeLeafTransitionBehavior();
    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);

    private AnimationTriggerEventSCRP _animationTriggerEventSCRP;
    private AnimationTriggerEventPlayer _animationTriggerEventPlayer;
    public bool isHolstered { get; private set; }
    protected bool isComplete;
    public HolsterSecondaryWeaponManuverNodeLeaf(IRangeWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP) : base(weaponAdvanceUser, preCondition)
    {
        _animationTriggerEventSCRP = animationTriggerEventSCRP;
        _animationTriggerEventPlayer = new AnimationTriggerEventPlayer(_animationTriggerEventSCRP);
        _animationTriggerEventPlayer.SubscribeEvent(_animationTriggerEventSCRP.triggerEventDetail[0].eventName, HolsterEvent);
    }

    public override void Enter()
    {
        this.isComplete = false;
        isHolstered = false;
        _animationTriggerEventPlayer.Rewind();
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <HolsterSecondaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    private void HolsterEvent()
    {
        WeaponAttachingBehavior.Attach(
            weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon,
            weaponAdvanceUser._weaponBelt.secondaryWeaponSocket,
            WeaponMountComponent.attatchingDurationGlobal);
        isHolstered = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <HolsterSecondaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit() { }

    public override void FixedUpdateNode() { }

    public override bool IsComplete() 
    {
        return this.isComplete;
    }  

    public override bool IsReset()
    {
        return IsComplete();
    }

    public override void UpdateNode()
    {
        this._animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        if (this._animationTriggerEventPlayer.IsPlayFinish())
        {
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
            this.isComplete = true;
            this.nodeLeafTransitionBehavior.TransitioningCheck(this);
        }
    }
}
