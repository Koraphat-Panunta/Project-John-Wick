using System;
using UnityEngine;

public class DrawSecondaryWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    private AnimationTriggerEventSCRP _animationTriggerEventSCRP;
    private AnimationTriggerEventPlayer _animationTriggerEventPlayer;
    private bool _isDrawn;

    public DrawSecondaryWeaponManuverNodeLeaf(IRangeWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP) : base(weaponAdvanceUser, preCondition)
    {
        _animationTriggerEventSCRP = animationTriggerEventSCRP;
        _animationTriggerEventPlayer = new AnimationTriggerEventPlayer(_animationTriggerEventSCRP);
        _animationTriggerEventPlayer.SubscribeEvent(_animationTriggerEventSCRP.triggerEventDetail[0].eventName, DrawEvent);
    }

    public override void Enter()
    {
        _isDrawn = false;
        _animationTriggerEventPlayer.Rewind();
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <DrawSecondaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    private void DrawEvent()
    {
        WeaponAttachingBehavior.Attach(
            weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon,
            weaponAdvanceUser._mainHandSocket,
            WeaponMountComponent.attatchingDurationGlobal);
        _isDrawn = true;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <DrawSecondaryWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void Exit()
    {
        if (_isDrawn == false)
            WeaponAttachingBehavior.Attach(
                weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon,
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
