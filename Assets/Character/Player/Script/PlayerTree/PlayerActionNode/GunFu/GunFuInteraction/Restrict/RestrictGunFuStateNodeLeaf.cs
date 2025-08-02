using System;
using Unity.Cinemachine;
using UnityEngine;

public class RestrictGunFuStateNodeLeaf : PlayerStateNodeLeaf, IGunFuNode
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunExecutedAble { get => player.attackedAbleGunFu; set { } }
    public AnimationClip _animationClip { get; set; }

    private Transform targetAdjustTransform => gunFuAble._targetAdjustTranform;
    private Vector3 targetAdjustPosition => targetAdjustTransform.position /*+ restrictScriptableObject.offset*/;
    private Quaternion targetAdjustRotation => targetAdjustTransform.rotation * Quaternion.Euler(restrictScriptableObject.rotationOffset);

    private AnimationClip restrictEnterClip;
    private AnimationClip restrictExitClip;

    private float StayDuration => restrictScriptableObject.StayDuration;
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

    public RestrictGunFuStateNodeLeaf(RestrictScriptableObject restrictScriptableObject, Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        this.restrictScriptableObject = restrictScriptableObject;
        this.restrictEnterClip = restrictScriptableObject.restrictEnterClip;
        this.restrictExitClip = restrictScriptableObject.restrictExitClip;

    }

    public override void Enter()
    {
        base.isComplete = false;
        curRestrictGunFuPhase = RestrictGunFuPhase.Enter;
        isRestrictExitHit = false;
        gotGunExecutedAble._character._movementCompoent.CancleMomentum();
        gotGunExecutedAble.TakeGunFuAttacked(this, player);
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
        switch (curRestrictGunFuPhase)
        {
            case RestrictGunFuPhase.Enter:
                {
                    _timer += Time.deltaTime;

                    player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, MoveMode.MaintainMomentum);

                    float w = _timer / restrictEnterClip.length * restrictScriptableObject.restrictEnter_exitNormalized;
                   

                    gotGunExecutedAble._character.transform.position = Vector3.Lerp(
                        gotGunExecutedAble._character.transform.position,
                        targetAdjustPosition,
                        w
                        );

                    gotGunExecutedAble._character.transform.rotation = Quaternion.Lerp(
                        gotGunExecutedAble._character.transform.rotation,
                        targetAdjustRotation,
                       w
                        );

       

                    if (_timer >= restrictEnterClip.length * restrictScriptableObject.restrictEnter_exitNormalized)
                    {
                        curRestrictGunFuPhase = RestrictGunFuPhase.Stay;
                        _timer = 0;
                        player.NotifyObserver(player, this);
                    }
                }
                break;
            case RestrictGunFuPhase.Stay:
                {
                    _timer += Time.deltaTime;

                    gotGunExecutedAble._character.transform.position = targetAdjustPosition;
                    gotGunExecutedAble._character.transform.rotation = targetAdjustRotation;

                    

                    player._movementCompoent.MoveToDirLocal(player.inputMoveDir_Local, player.StandMoveAccelerate, player.StandMoveMaxSpeed, MoveMode.MaintainMomentum);


                    if (gotGunExecutedAble._character.isDead)
                        isComplete = true;

                    if(_timer >= StayDuration
                        ||(player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>() == false)
                    {
                        _timer = 0;
                       curRestrictGunFuPhase = RestrictGunFuPhase.Exit;
                        player.NotifyObserver(player, this);
                    }
                }
                break;
            case RestrictGunFuPhase.Exit:
                {
                    _timer += Time.deltaTime;
                    player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, MoveMode.MaintainMomentum);
                    if(isRestrictExitHit == false)
                    {
                        gotGunExecutedAble._character.transform.position = targetAdjustPosition;
                        gotGunExecutedAble._character.transform.rotation = targetAdjustRotation;
                    }
                    if (isRestrictExitHit == false && _timer > restrictExitClip.length * restrictScriptableObject.restrictExit_hitNormalized)
                    {
                        if(gotGunExecutedAble._character._movementCompoent is IMotionImplusePushAble movePush)
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
                    _timer += Time.deltaTime;
                    if (_timer >= restrictExitClip.length * restrictScriptableObject.restrictExit_exitNormalized)
                    {
                        isComplete = true;
                        _timer = 0;
                    }
                }
                break;

        }
        base.UpdateNode();
    }
}
