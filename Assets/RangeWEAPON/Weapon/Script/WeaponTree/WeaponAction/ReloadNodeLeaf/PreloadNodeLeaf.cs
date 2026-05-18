using System;
using System.Collections.Generic;
using UnityEngine;

// Enters the reload stance before shell loading begins.
// Runs once per reload sequence; sets _isReloadStanceEntered on completion.
public class PreloadNodeLeaf :
    WeaponManuverLeafNode
    , IShotgunReloadNode
    , INodeLeafTransitionAble
{
    private bool isComplete;
    private AutomaticShotgunModel _shotgun;
    protected AnimationTriggerEventPlayer timelineTriggerEvent;

    public INodeManager nodeManager { get => weaponAdvanceUser._weaponManuverManager._reloadNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public float _reloadTime => timelineTriggerEvent.endTimer;
    protected override IRangeWeaponAdvanceUser weaponAdvanceUser { get => _shotgun.userWeapon; set { } }

    public PreloadNodeLeaf(
        AutomaticShotgunModel shotgun,
        AnimationTriggerEventSCRP timelineSCRP,
        Func<bool> preCondition) : base(null, preCondition)
    {
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.transitionAbleNode = new Dictionary<INode, bool>();

        _shotgun = shotgun;
        timelineTriggerEvent = new AnimationTriggerEventPlayer(timelineSCRP);
        this.timelineTriggerEvent.SubscribeEvent(IShotgunReloadNode.ShotgunReloadStage.Load.ToString(), this.Load);
    }

    public override void Enter()
    {
        Debug.Log("PreloadNodeLeaf Enter");

        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        curPhase = WeaponManuverLeafNodePhase.Enter;
        isComplete = false;
        timelineTriggerEvent.Rewind();
        _shotgun.Notify<PreloadNodeLeaf>(_shotgun, this);
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        base.Enter();
    }

    public override void Exit()
    {
        isComplete = false;
        curPhase = WeaponManuverLeafNodePhase.Exit;
        _shotgun.Notify<PreloadNodeLeaf>(_shotgun, this);
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        base.Exit();
    }

    public override void UpdateNode()
    {
        timelineTriggerEvent.UpdatePlay(Time.deltaTime);
        if (timelineTriggerEvent.IsPlayFinish())
        {
            isComplete = true;
            this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        }

        this.TransitioningCheck();
    }

    private void Load()
    {
        IShotgunReloadNode.QuadLoad(this._shotgun);
        _shotgun.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }
    public override bool IsReset()
    {
        if (weaponAdvanceUser == null)
            return true;

        if (this._shotgun.userWeapon._weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) <= 0)
            return true;

        if (IsComplete())
            return true;

        if (weaponAdvanceUser._weaponManuverManager.isReloadManuverAble == false)
            return true;

        return false;

    }

    public override void FixedUpdateNode() { }

    public override bool IsComplete()
    {
        if(this._shotgun.curBulletCapacity >= this._shotgun.maxAmmoCapacity)
            return true;

        return isComplete;
    }

    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);

    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this, node);

}
