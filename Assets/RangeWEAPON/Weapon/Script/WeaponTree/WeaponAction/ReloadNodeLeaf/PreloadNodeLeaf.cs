using System;
using UnityEngine;

// Enters the reload stance before shell loading begins.
// Runs once per reload sequence; sets _isReloadStanceEntered on completion.
public class PreloadNodeLeaf : WeaponManuverLeafNode, IShotgunReloadNode
{
    private bool isComplete;
    private AutomaticShotgunModel _shotgun;
    protected AnimationTriggerEventPlayer timelineTriggerEvent;

    protected override IRangeWeaponAdvanceUser weaponAdvanceUser { get => _shotgun.userWeapon; set { } }

    public PreloadNodeLeaf(
        AutomaticShotgunModel shotgun,
        AnimationTriggerEventSCRP timelineSCRP,
        Func<bool> preCondition) : base(null, preCondition)
    {
        _shotgun = shotgun;
        timelineTriggerEvent = new AnimationTriggerEventPlayer(timelineSCRP);
    }

    public override void Enter()
    {
        curPhase = WeaponManuverLeafNodePhase.Enter;
        isComplete = false;
        timelineTriggerEvent.Rewind();
        _shotgun.Notify<PreloadNodeLeaf>(_shotgun, this);
        base.Enter();
    }

    public override void Exit()
    {
        isComplete = false;
        curPhase = WeaponManuverLeafNodePhase.Exit;
        _shotgun.Notify<PreloadNodeLeaf>(_shotgun, this);
        base.Exit();
    }

    public override void UpdateNode()
    {
        timelineTriggerEvent.UpdatePlay(Time.deltaTime);
        if (timelineTriggerEvent.IsPlayFinish())
        {
            _shotgun._isReloadStanceEntered = true;
            isComplete = true;
        }
    }

    public override void FixedUpdateNode() { }

    public override bool IsComplete() => isComplete;
}
