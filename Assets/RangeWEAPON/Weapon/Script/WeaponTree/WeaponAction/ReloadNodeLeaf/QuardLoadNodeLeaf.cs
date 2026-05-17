using System;
using UnityEngine;

// Inserts 2 shells into the tube per cycle. Repeats until the tube is full or ammo runs out.
// Requires a TimelineTriggerEventScriptableObject with an event named "LoadShell".
public class QuardLoadNodeLeaf : 
    WeaponManuverLeafNode
    , IShotgunReloadNode
{
    private const int SHELLS_PER_LOAD = 2;

    private bool isComplete;
    private AutomaticShotgunModel _shotgun;
    protected TimelineTriggerEvent timelineTriggerEvent;

    protected override IRangeWeaponAdvanceUser weaponAdvanceUser { get => _shotgun.userWeapon; set { } }

    public float _reloadTime => this._shotgun.reloadTime;

    public QuardLoadNodeLeaf(
        AutomaticShotgunModel shotgun,
        TimelineTriggerEventScriptableObject timelineSCRP,
        Func<bool> preCondition) : base(null, preCondition)
    {
        _shotgun = shotgun;
        timelineTriggerEvent = new TimelineTriggerEvent(_shotgun.reloadTime, timelineSCRP.triggerEventDetail);
        timelineTriggerEvent.SubscribeEvent(
            IShotgunReloadNode.ShotgunReloadStage.Load.ToString(),
            Load);
    }

    public override void Enter()
    {
        Debug.Log("QuardLoadNodeLeaf Enter");

        curPhase = WeaponManuverLeafNodePhase.Enter;
        isComplete = false;
        timelineTriggerEvent.SetDuration(_shotgun.reloadTime);
        timelineTriggerEvent.Rewind();
        _shotgun.Notify<QuardLoadNodeLeaf>(_shotgun, this);
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("QuardLoadNodeLeaf Exit");
        isComplete = false;
        curPhase = WeaponManuverLeafNodePhase.Exit;
        _shotgun.Notify<QuardLoadNodeLeaf>(_shotgun, this);
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        base.Exit();
    }

    public override void UpdateNode()
    {
        curPhase = WeaponManuverLeafNodePhase.Update;
        timelineTriggerEvent.UpdatePlay(Time.deltaTime);

        if (timelineTriggerEvent.IsPlayFinish()
            && this.IsComplete() == false)
        {
            this.Enter();
        }
    }

    public override void FixedUpdateNode() { }

    public override bool IsComplete() 
    {
        return this._shotgun.curBulletCapacity >= this._shotgun.maxAmmoCapacity;
    }

    public override bool IsReset()
    {
        if (weaponAdvanceUser == null)
            return true;

        if (IsComplete())
            return true;

        if(this._shotgun.userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) <= 0)
            return true;

        if (weaponAdvanceUser._weaponManuverManager.isReloadManuverAble == false)
            return true;

        return false;

    }

    private void Load()
    {
        IShotgunReloadNode.QuadLoad(this._shotgun);
        _shotgun.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        Debug.Log("Onload = " + _shotgun.curBulletCapacity);
    }
}
