using System;
using Unity.Cinemachine;
using UnityEngine;

public class RestrictGunFuStateNodeLeaf : PlayerStateNodeLeaf, IGunFuNode
{
    public float _transitionAbleTime_Nornalized { get; set; }
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get => player; set { } }
    public IGunFuGotAttackedAble attackedAbleGunFu { get => player.attackedAbleGunFu; set { } }
    public AnimationClip _animationClip { get; set; }

    private Transform targetAdjustTransform => gunFuAble._targetAdjustTranform;
    private Vector3 targetAdjustPosition => targetAdjustTransform.position /*+ restrictScriptableObject.offset*/;
    private Quaternion targetAdjustRotation => targetAdjustTransform.rotation * Quaternion.Euler(restrictScriptableObject.rotationOffset);

    private AnimationClip restrictEnterClip;
    private AnimationClip restrictExitClip;

    private float StayDuration => restrictScriptableObject.StayDuration;
    private bool isRestrictExitHit;
    private RestrictScriptableObject restrictScriptableObject { get; set; }

    public enum RestrictGunFuPhase
    {
        Enter,
        Stay,
        Exit,
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
        attackedAbleGunFu._movementCompoent.CancleMomentum();
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);
        base.Enter();
    }

    public override void Exit()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);
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

                    attackedAbleGunFu.TakeGunFuAttacked(this, player);

                    player.playerMovement.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);

                    float w = _timer / restrictEnterClip.length * restrictScriptableObject.restrictEnter_exitNormalized;
                   

                    attackedAbleGunFu._gunFuAttackedAble.position = Vector3.Lerp(
                        attackedAbleGunFu._gunFuAttackedAble.position,
                        targetAdjustPosition,
                        w
                        );

                    attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(
                        attackedAbleGunFu._gunFuAttackedAble.rotation,
                        targetAdjustRotation,
                       w
                        );

       

                    if (_timer >= restrictEnterClip.length * restrictScriptableObject.restrictEnter_exitNormalized)
                    {
                        curRestrictGunFuPhase = RestrictGunFuPhase.Stay;
                        _timer = 0;
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
                    }
                }
                break;
            case RestrictGunFuPhase.Stay:
                {
                    _timer += Time.deltaTime;

                    attackedAbleGunFu._gunFuAttackedAble.position = targetAdjustPosition;
                    attackedAbleGunFu._gunFuAttackedAble.rotation = targetAdjustRotation;

                    

                    player.playerMovement.MoveToDirLocal(player.inputMoveDir_Local, player.StandMoveAccelerate, player.StandMoveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);


                    if (attackedAbleGunFu._isDead)
                        isComplete = true;

                    if(_timer >= StayDuration
                        ||player._weaponManuverManager.curNodeLeaf is AimDownSightWeaponManuverNodeLeaf == false)
                    {
                        _timer = 0;
                       curRestrictGunFuPhase = RestrictGunFuPhase.Exit;
                        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
                    }
                }
                break;
            case RestrictGunFuPhase.Exit:
                {
                    _timer += Time.deltaTime;
                    player.playerMovement.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
                    if(isRestrictExitHit == false)
                    {
                        attackedAbleGunFu._gunFuAttackedAble.position = targetAdjustPosition;
                        attackedAbleGunFu._gunFuAttackedAble.rotation = targetAdjustRotation;
                    }
                    if (isRestrictExitHit == false && _timer > restrictExitClip.length * restrictScriptableObject.restrictExit_hitNormalized)
                    {
                        if(attackedAbleGunFu._movementCompoent is IMotionImplusePushAble movePush)
                        {
                            player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuAttack);
                            movePush.AddForcePush(gunFuAble._gunFuUserTransform.forward * restrictScriptableObject.restrictExit_HitForce, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
                        }
                        isRestrictExitHit = true;
                    }

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
