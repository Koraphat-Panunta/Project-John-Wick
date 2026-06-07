using System;
using System.Collections.Generic;
using UnityEngine;

public class HolsterPrimaryWeaponManuverNodeLeaf : WeaponManuverLeafNode, INodeLeafTransitionAble
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

    private readonly Transform _leftHandTransform;
    private readonly TransformOffsetSCRP _leftHandHoldWeapon;
    private AttachWeaponToLeftHandEvent _attachWeaponToLeftHandEvent;

    public HolsterPrimaryWeaponManuverNodeLeaf(
        IRangeWeaponAdvanceUser weaponAdvanceUser,
        Func<bool> preCondition,
        AnimationTriggerEventSCRP animationTriggerEventSCRP,
        Transform leftHandTransform,
        TransformOffsetSCRP leftHandHoldWeapon) : base(weaponAdvanceUser, preCondition)
    {
        _leftHandTransform = leftHandTransform;
        _leftHandHoldWeapon = leftHandHoldWeapon;
        _animationTriggerEventSCRP = animationTriggerEventSCRP;
        _animationTriggerEventPlayer = new AnimationTriggerEventPlayer(_animationTriggerEventSCRP);
        _animationTriggerEventPlayer.SubscribeEvent(
            _animationTriggerEventSCRP.triggerEventDetail[0].eventName, AttachToLeftHand);
        _animationTriggerEventPlayer.SubscribeEvent(
            _animationTriggerEventSCRP.triggerEventDetail[1].eventName, HolsterEvent);
    }

    public override void Enter()
    {
        isComplete = false;
        isHolstered = false;
        _attachWeaponToLeftHandEvent = new AttachWeaponToLeftHandEvent(
            weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon,
            _leftHandTransform,
            _leftHandHoldWeapon);
        _animationTriggerEventPlayer.Rewind();
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <HolsterPrimaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    private void AttachToLeftHand() => _attachWeaponToLeftHandEvent?.Attach();

    private void HolsterEvent()
    {
        WeaponAttachingBehavior.Attach(
            weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon,
            weaponAdvanceUser._weaponBelt.primaryWeaponSocket,
            WeaponMountComponent.attatchingDurationGlobal);
        isHolstered = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <HolsterPrimaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
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
        _animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        if (_animationTriggerEventPlayer.IsPlayFinish())
        {
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
            isComplete = true;
            nodeLeafTransitionBehavior.TransitioningCheck(this);
        }
    }
}
