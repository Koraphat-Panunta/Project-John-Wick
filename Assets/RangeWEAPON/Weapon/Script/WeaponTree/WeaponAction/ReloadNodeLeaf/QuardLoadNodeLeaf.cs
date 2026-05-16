using System;
using UnityEngine;

// Inserts 2 shells into the tube per cycle. Repeats until the tube is full or ammo runs out.
// Requires a TimelineTriggerEventScriptableObject with an event named "LoadShell".
public class QuardLoadNodeLeaf : WeaponManuverLeafNode, IShotgunReloadNode
{
    private const int SHELLS_PER_LOAD = 2;

    private bool isComplete;
    private AutomaticShotgunModel _shotgun;
    protected TimelineTriggerEvent timelineTriggerEvent;

    protected override IRangeWeaponAdvanceUser weaponAdvanceUser { get => _shotgun.userWeapon; set { } }

    public QuardLoadNodeLeaf(
        AutomaticShotgunModel shotgun,
        TimelineTriggerEventScriptableObject timelineSCRP,
        Func<bool> preCondition) : base(null, preCondition)
    {
        _shotgun = shotgun;
        timelineTriggerEvent = new TimelineTriggerEvent(_shotgun.reloadTime, timelineSCRP.triggerEventDetail);
        timelineTriggerEvent.SubscribeEvent(
            IShotgunReloadNode.ShotgunReloadStage.LoadShell.ToString(),
            LoadShells);
    }

    public override void Enter()
    {
        curPhase = WeaponManuverLeafNodePhase.Enter;
        isComplete = false;
        timelineTriggerEvent.SetDuration(_shotgun.reloadTime);
        timelineTriggerEvent.Rewind();
        _shotgun.Notify<QuardLoadNodeLeaf>(_shotgun, this);
        base.Enter();
    }

    public override void Exit()
    {
        isComplete = false;
        curPhase = WeaponManuverLeafNodePhase.Exit;
        _shotgun.Notify<QuardLoadNodeLeaf>(_shotgun, this);
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

    public override bool IsReset()
    {
        bool shouldReset = base.IsReset();
        if (shouldReset)
            _shotgun._isReloadStanceEntered = false;
        return shouldReset;
    }

    private void LoadShells()
    {
        _shotgun.LoadShellsIntoTube(SHELLS_PER_LOAD);
        _shotgun.Notify(_shotgun, IShotgunReloadNode.ShotgunReloadStage.LoadShell);
    }
}
