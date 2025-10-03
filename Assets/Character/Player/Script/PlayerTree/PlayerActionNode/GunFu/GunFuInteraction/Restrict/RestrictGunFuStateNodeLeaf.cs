using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class RestrictGunFuStateNodeLeaf : PlayerStateNodeLeaf, IGunFuNode,INodeLeafTransitionAble
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public float phaseTimer { get; private set; }
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }
    public AnimationClip _animationClip { get; set; }

    private Transform targetAdjustTransform => gunFuAble._targetAdjustTranform;
    private Vector3 targetAdjustPosition => targetAdjustTransform.position /*+ restrictScriptableObject.offset*/;
    private Quaternion targetAdjustRotation => targetAdjustTransform.rotation * Quaternion.Euler(restrictScriptableObject.rotationOffset);

    private AnimationClip restrictEnterClip;
    private AnimationClip restrictExitClip;

    public float StayDuration => restrictScriptableObject.StayDuration;
    private bool isRestrictExitHit;
    private RestrictScriptableObject restrictScriptableObject { get; set; }
    public string _stateName => restrictScriptableObject.stateName;
    public enum RestrictGunFuPhase
    {
        Enter,
        Stay,
        Exit,
        ExitAttack,
        Done,
    }
    public RestrictGunFuPhase curRestrictGunFuPhase { get; private set; }
    public INodeManager nodeManager { get => player.playerStateNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public RestrictGunFuStateNodeLeaf(RestrictScriptableObject restrictScriptableObject, Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        this.restrictScriptableObject = restrictScriptableObject;
        this.restrictEnterClip = restrictScriptableObject.restrictEnterClip;
        this.restrictExitClip = restrictScriptableObject.restrictExitClip;

        this.transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

    }

    public override void Enter()
    {
        gotGunFuAttackedAble = gunFuAble.attackedAbleGunFu;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        _timer = 0;
        base.isComplete = false;
        curRestrictGunFuPhase = RestrictGunFuPhase.Enter;
        isRestrictExitHit = false;
        gotGunFuAttackedAble._character._movementCompoent.CancleMomentum();
        gotGunFuAttackedAble.TakeGunFuAttacked(this, player);
        player.NotifyObserver(player, this);
        base.Enter();
    }

    public override void Exit()
    {
        curRestrictGunFuPhase = RestrictGunFuPhase.Exit;
        player.NotifyObserver(player, this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
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

        if(player._triggerHitedGunFu)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        switch (curRestrictGunFuPhase)
        {
            case RestrictGunFuPhase.Enter:
                {
                    phaseTimer += Time.deltaTime;

                    player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, MoveMode.MaintainMomentum);

                    float w = phaseTimer / restrictEnterClip.length * restrictScriptableObject.restrictEnter_exitNormalized;
                   

                    gotGunFuAttackedAble._character.transform.position = Vector3.Lerp(
                        gotGunFuAttackedAble._character.transform.position,
                        targetAdjustPosition,
                        w
                        );

                    gotGunFuAttackedAble._character.transform.rotation = Quaternion.Lerp(
                        gotGunFuAttackedAble._character.transform.rotation,
                        targetAdjustRotation,
                       w
                        );

       

                    if (phaseTimer >= restrictEnterClip.length * restrictScriptableObject.restrictEnter_exitNormalized)
                    {
                        curRestrictGunFuPhase = RestrictGunFuPhase.Stay;
                        phaseTimer = 0;
                        player.NotifyObserver(player, this);
                    }
                }
                break;
            case RestrictGunFuPhase.Stay:
                {
                    phaseTimer += Time.deltaTime;

                    gotGunFuAttackedAble._character.transform.position = targetAdjustPosition;
                    gotGunFuAttackedAble._character.transform.rotation = targetAdjustRotation;

                    

                    player._movementCompoent.MoveToDirLocal(player.inputMoveDir_Local, player.StandMoveAccelerate, player.StandMoveMaxSpeed, MoveMode.MaintainMomentum);


                    if (gotGunFuAttackedAble._character.isDead)
                        isComplete = true;

                    if(phaseTimer >= StayDuration
                        ||(player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false)
                    {
                        phaseTimer = 0;
                       curRestrictGunFuPhase = RestrictGunFuPhase.Exit;
                        player.NotifyObserver(player, this);
                    }
                }
                break;
            case RestrictGunFuPhase.Exit:
                {
                    phaseTimer += Time.deltaTime;
                    player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, MoveMode.MaintainMomentum);
                    if(isRestrictExitHit == false)
                    {
                        gotGunFuAttackedAble._character.transform.position = targetAdjustPosition;
                        gotGunFuAttackedAble._character.transform.rotation = targetAdjustRotation;
                    }
                    if (isRestrictExitHit == false && phaseTimer > restrictExitClip.length * restrictScriptableObject.restrictExit_hitNormalized)
                    {
                        if(gotGunFuAttackedAble._character._movementCompoent is IMotionImplusePushAble movePush)
                        {
                            curRestrictGunFuPhase = RestrictGunFuPhase.ExitAttack;
                            player.NotifyObserver(player, this);
                            movePush.AddForcePush(gunFuAble._character.transform.forward * restrictScriptableObject.restrictExit_HitForce, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
                        }
                        isRestrictExitHit = true;
                    }

                    break;

                }
            case RestrictGunFuPhase.ExitAttack:
                {

                    phaseTimer += Time.deltaTime;
                    if (phaseTimer >= restrictExitClip.length * restrictScriptableObject.restrictExit_exitNormalized)
                    {
                        nodeLeafTransitionBehavior.TransitionAbleAll(this);
                        isComplete = true;
                        phaseTimer = 0;
                    }
                }
                break;

        }
        this.TransitioningCheck();
        base.UpdateNode();
    }

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
