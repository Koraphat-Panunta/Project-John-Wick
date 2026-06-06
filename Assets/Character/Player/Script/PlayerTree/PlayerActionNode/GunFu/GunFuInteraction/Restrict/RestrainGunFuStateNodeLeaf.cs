using System;
using System.Collections.Generic;
using UnityEngine;

public class RestrainGunFuStateNodeLeaf : PlayerStateNodeLeaf
    , I_OCM_Node
    , INodeLeafTransitionAble
{
  
    public I_OCM_Attack_Able gunFuAble { get => player; set { } }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get; set; }

    public float holdTimer;
    public float timer;
    private Transform targetAdjustTransform => gunFuAble._targetAdjustTranform;
    private Vector3 targetAdjustPosition => targetAdjustTransform.position
        + targetAdjustTransform.forward * restrictScriptableObject.holdTargetOffset.z
        + targetAdjustTransform.up    * restrictScriptableObject.holdTargetOffset.y
        + targetAdjustTransform.right * restrictScriptableObject.holdTargetOffset.x;
    private Quaternion targetAdjustRotation => targetAdjustTransform.rotation * Quaternion.Euler(restrictScriptableObject.holdRotationOffset);

    private RestrictScriptableObject restrictScriptableObject { get; set; }
    public string _stateName => restrictScriptableObject.stateName;
    public float holdDuration => this.restrictScriptableObject.holdDuration;
    public float onHoldTimeNormalized;
    public float holdNormalizedTimer;
    public enum RestrictGunFuPhase
    {
        Enter,
        Stay,
        Exit,
        None,
    }
    public RestrictGunFuPhase curRestrictGunFuPhase { get; private set; }
    public INodeManager nodeManager { get => player.playerStateNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }


    private AnimationTriggerEventPlayer _enterEventPlayer;

    private SubjectAnimationInteract _exitPlayerSubject;
    private SubjectAnimationInteract _exitEnemySubject;
    private AnimationTriggerEventPlayer _exitEventPlayer;

    public RestrainGunFuStateNodeLeaf(RestrictScriptableObject restrictScriptableObject, Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        this.restrictScriptableObject = restrictScriptableObject;

        _enterEventPlayer   = new AnimationTriggerEventPlayer(restrictScriptableObject.enterInteractSCRP);

        _exitPlayerSubject  = new SubjectAnimationInteract(restrictScriptableObject.exitInteractSCRP, restrictScriptableObject.exitInteractSCRP.animationInteractCharacterDetail[0]);
        _exitEnemySubject   = new SubjectAnimationInteract(restrictScriptableObject.exitInteractSCRP, restrictScriptableObject.exitInteractSCRP.animationInteractCharacterDetail[1]);
        _exitEventPlayer    = new AnimationTriggerEventPlayer(restrictScriptableObject.exitInteractSCRP);

        this.onHoldTimeNormalized = this._enterEventPlayer.GetEventNormalizedTime("EnterDone");

        this._enterEventPlayer.SubscribeEvent("EnterDone", _OnEnterDone);
        this._exitEventPlayer.SubscribeEvent("ExitDone", _OnExitDone);

        this.transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }

    
    public override void Enter()
    {
        this.holdTimer = 0;
        this.timer = 0;

        gotGunFuAttackedAble = gunFuAble.attackedAbleGunFu;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);

        isComplete = false;
        curRestrictGunFuPhase = RestrictGunFuPhase.Enter;

        gotGunFuAttackedAble._character._movementCompoent.CancleMomentum();
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, player, true);

        player.NotifyObserver(player, this);

        Vector3 anchorPos = targetAdjustPosition;
        Vector3 anchorDir = player.transform.forward;
        _enterEventPlayer.Rewind();

        base.Enter();
    }

    public override void Exit()
    {
        this.player.enableRootMotion = false;
        curRestrictGunFuPhase = RestrictGunFuPhase.None;
        player.NotifyObserver(player, this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        player._movementCompoent.UpdateMovement();
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;
        if (player.isDead)
            return true;
        if (player._triggerEnterGotAttacked_OCM)
            return true;
        return false;
    }

    public override void UpdateNode()
    {
        this.timer += Time.deltaTime;

        this.holdNormalizedTimer = this._enterEventPlayer.GetRemapNormalizedTimer(0, this.onHoldTimeNormalized);

        switch (curRestrictGunFuPhase)
        {
            case RestrictGunFuPhase.Enter:
                {
                    this.gotGunFuAttackedAble._character._movementCompoent.SetPosition(
                        Vector3.Lerp(
                            this.gotGunFuAttackedAble._character._movementCompoent.curPosition
                            , this.targetAdjustPosition
                            , this.holdNormalizedTimer)
                        );

                    this.gotGunFuAttackedAble._character._movementCompoent.SetRotation(
                        Quaternion.Lerp(
                            this.gotGunFuAttackedAble._character._movementCompoent.curRotation
                            , this.targetAdjustRotation, this.holdNormalizedTimer)
                        );

                    player._movementCompoent.UpdateMoveToDirWorld(Vector3.zero, player.breakDecelerate, MoveMode.MaintainMomentumDirection);
                    _enterEventPlayer.UpdatePlay(Time.deltaTime);
                }
                break;

            case RestrictGunFuPhase.Stay:
                {
        

                    this.gotGunFuAttackedAble._character._movementCompoent.SetPosition(
                        Vector3.Lerp(
                            this.gotGunFuAttackedAble._character._movementCompoent.curPosition
                            , this.targetAdjustPosition
                            ,this.holdNormalizedTimer)
                        );

                    this.gotGunFuAttackedAble._character._movementCompoent.SetRotation(
                        Quaternion.Lerp(
                            this.gotGunFuAttackedAble._character._movementCompoent.curRotation
                            ,this.targetAdjustRotation,this.holdNormalizedTimer)
                        );

                    this.player._movementCompoent.UpdateMoveToDirLocal(this.player.inputMoveDir_Local * this.player.StandMoveMaxSpeed, player.StandMoveAccelerate, MoveMode.MaintainMomentumDirection);

                    if (gotGunFuAttackedAble._character.isDead)
                        isComplete = true;

                    if (this.holdTimer >= restrictScriptableObject.holdDuration
                        || (player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false)
                    {
                        _StartExit();
                    }

                    this.holdTimer += Time.deltaTime;
                }
                break;

            case RestrictGunFuPhase.Exit:
                {
        
                    player._movementCompoent.UpdateMoveToDirWorld(Vector3.zero, player.breakDecelerate, MoveMode.MaintainMomentumDirection);
                    _exitPlayerSubject.UpdateInteract(Time.deltaTime);
                    _exitEnemySubject.UpdateInteract(Time.deltaTime);
                    _exitEventPlayer.UpdatePlay(Time.deltaTime);
                }
                break;
        }

        this.TransitioningCheck();
        base.UpdateNode();
    }

    private void _StartExit()
    {
        this.player.enableRootMotion = true;
        curRestrictGunFuPhase = RestrictGunFuPhase.Exit;
        player.NotifyObserver(player, this);
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.player, false);

        Vector3 anchorPos = targetAdjustPosition;
        Vector3 anchorDir = player.transform.forward;
        _exitPlayerSubject.RestartSubject(player._character, anchorPos, anchorDir);
        _exitEnemySubject.RestartSubject(gotGunFuAttackedAble._character, anchorPos, anchorDir);
        _exitEventPlayer.Rewind();
    }
    private void _OnEnterDone()
    {
        this.player.enableRootMotion = false;
        this.curRestrictGunFuPhase = RestrictGunFuPhase.Stay;
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.player, false);

        player.NotifyObserver(player, this);
    }
    private void _OnExitDone()
    {
        nodeLeafTransitionBehavior.TransitionAbleAll(this);
        isComplete = true;
    }

    private void EnablePlayerRootMotion(Character character)
    {
        if (character == player)
        {
            character.enableRootMotion = true;
        }
    }


    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        this.player.OnNotifyFeedBackVisitor(damageAble);
    }
}
