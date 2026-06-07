using System;
using UnityEngine;

public class DrawPrimaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private AnimationTriggerEventSCRP _animationTriggerEventSCRP;
    private AnimationTriggerEventPlayer _animationTriggerEventPlayer;
    private bool _isDrawn;

    private readonly Transform _leftHandTransform;
    private readonly TransformOffsetSCRP _leftHandHoldWeapon;
    private AttachWeaponToLeftHandEvent _attachWeaponToLeftHandEvent;

    public DrawPrimaryWeaponManuverNodeLeaf(
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
            _animationTriggerEventSCRP.triggerEventDetail[1].eventName, DrawEvent);
    }

    public override void Enter()
    {
        _isDrawn = false;
        _attachWeaponToLeftHandEvent = new AttachWeaponToLeftHandEvent(
            weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon,
            _leftHandTransform,
            _leftHandHoldWeapon);
        _animationTriggerEventPlayer.Rewind();
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <DrawPrimaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    private void AttachToLeftHand() => _attachWeaponToLeftHandEvent?.Attach();

    private void DrawEvent()
    {
        WeaponAttachingBehavior.Attach(
            weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon,
            weaponAdvanceUser._mainHandSocket,
            WeaponMountComponent.attatchingDurationGlobal);
        _isDrawn = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <DrawPrimaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit()
    {
        if (_isDrawn == false)
            WeaponAttachingBehavior.Attach(
                weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon,
                weaponAdvanceUser._mainHandSocket,
                WeaponMountComponent.attatchingDurationGlobal);
    }

    public override void FixedUpdateNode() { }

    public override bool IsComplete() => _animationTriggerEventPlayer.IsPlayFinish();

    public override bool IsReset() => IsComplete();

    public override void UpdateNode()
    {
        _animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
    }
}
