using System;
using System.Collections.Generic;
using UnityEngine;

// Loads one shell from the tube into the chamber if the chamber is empty.
// Requires a TimelineTriggerEventScriptableObject with an event named "ChamberLoad".
public class ChamberLoadShotgunNodeLeaf : 
    WeaponManuverLeafNode
    , IShotgunReloadNode
    , INodeLeafTransitionAble
{
    private bool isComplete;
    private AutomaticShotgunModel _shotgun;
    protected AnimationTriggerEventPlayer timelineTriggerEvent;

    protected override IRangeWeaponAdvanceUser weaponAdvanceUser { get => _shotgun.userWeapon; set { } }

    public INodeManager nodeManager { get => weaponAdvanceUser._weaponManuverManager._reloadNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public float _reloadTime => timelineTriggerEvent.endTimer;

    public ChamberLoadShotgunNodeLeaf(
        AutomaticShotgunModel shotgun,
        AnimationTriggerEventSCRP timelineSCRP,
        Func<bool> preCondition) : base(null, preCondition)
    {
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.transitionAbleNode = new Dictionary<INode, bool>();

        _shotgun = shotgun;
        timelineTriggerEvent = new AnimationTriggerEventPlayer(timelineSCRP);
        timelineTriggerEvent.SubscribeEvent(
            IShotgunReloadNode.ShotgunReloadStage.Load.ToString(),
            LoadChamber);
    }

    public override void Enter()
    {
        Debug.Log("ChamberLoad Enter");

        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);

        curPhase = WeaponManuverLeafNodePhase.Enter;
        isComplete = false;
        timelineTriggerEvent.Rewind();
        _shotgun.Notify<ChamberLoadShotgunNodeLeaf>(_shotgun, this);
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        base.Enter();
    }

    public override void Exit()
    {
        isComplete = false;
        curPhase = WeaponManuverLeafNodePhase.Exit;
        _shotgun.Notify<ChamberLoadShotgunNodeLeaf>(_shotgun, this);
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

    public override bool IsComplete() => isComplete;

    private void LoadChamber()
    {
        _shotgun.userWeapon._weaponBelt.ammoProuch.GetAmmoOut(BulletType.buckShotAmmo, 1, out int ammoOut);
        _shotgun.chamber.Load(_shotgun.bullet);
        _shotgun.Notify(_shotgun, IShotgunReloadNode.ShotgunReloadStage.Load);
        _shotgun.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);


    }

    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);


    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this, node);
   
}
