using System;
using UnityEngine;

// Loads one shell from the tube into the chamber if the chamber is empty.
// Requires a TimelineTriggerEventScriptableObject with an event named "ChamberLoad".
public class ChamberLoadShotgunNodeLeaf : WeaponManuverLeafNode, IShotgunReloadNode
{
    private bool isComplete;
    private AutomaticShotgunModel _shotgun;
    protected AnimationTriggerEventPlayer timelineTriggerEvent;

    protected override IRangeWeaponAdvanceUser weaponAdvanceUser { get => _shotgun.userWeapon; set { } }

    public ChamberLoadShotgunNodeLeaf(
        AutomaticShotgunModel shotgun,
        AnimationTriggerEventSCRP timelineSCRP,
        Func<bool> preCondition) : base(null, preCondition)
    {
        _shotgun = shotgun;
        timelineTriggerEvent = new AnimationTriggerEventPlayer(timelineSCRP);
        timelineTriggerEvent.SubscribeEvent(
            IShotgunReloadNode.ShotgunReloadStage.ChamberLoad.ToString(),
            LoadChamber);
    }

    public override void Enter()
    {
        curPhase = WeaponManuverLeafNodePhase.Enter;
        isComplete = false;
        timelineTriggerEvent.Rewind();
        _shotgun.Notify<ChamberLoadShotgunNodeLeaf>(_shotgun, this);
        base.Enter();
    }

    public override void Exit()
    {
        isComplete = false;
        curPhase = WeaponManuverLeafNodePhase.Exit;
        _shotgun.Notify<ChamberLoadShotgunNodeLeaf>(_shotgun, this);
        base.Exit();
    }

    public override void UpdateNode()
    {
        timelineTriggerEvent.UpdatePlay(Time.deltaTime);
        if (timelineTriggerEvent.IsPlayFinish())
            isComplete = true;
    }

    public override void FixedUpdateNode() { }

    public override bool IsComplete() => isComplete;

    private void LoadChamber()
    {
        if (_shotgun.TryGetShellFromTube(out Bullet shell))
        {
            _shotgun.chamber.Load(shell);
            _shotgun.Notify(_shotgun, IShotgunReloadNode.ShotgunReloadStage.ChamberLoad);
        }
    }
}
