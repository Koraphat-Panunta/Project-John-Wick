using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackMoveMeleeWeaponNodeLeaf : NodeLeaf , IMeleeAttackNodeLeaf
{
    private IMeleeWeaponUserAble meleeWeaponUserAble;
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;
    IMeleeWeaponUserAble IMeleeAttackNodeLeaf._MeleeWeaponUserAble => this.meleeWeaponUserAble;
    private Character character;
    public MeleeWeaponAttackMoveScriptableObject attackMoveScriptableObject { get; protected set; }

    protected MeleeAttackingPhase attackingPhase;
    public MeleeAttackingPhase _attackingPhase => this.attackingPhase;

    private bool triggerIsReset;

    protected Vector3 targetPos;

    protected float beginmoveNormalizedTime;
    protected float moveNormalizedTime;
    public AttackMoveMeleeWeaponNodeLeaf(
        Func<bool> preCondition
        ,IMeleeWeaponUserAble meleeWeaponUserAble
        ,Character character
        ,MeleeWeaponAttackMoveScriptableObject meleeWeaponAttackMoveScriptableObject
        ) : base(preCondition)
    {
        this.attackMoveScriptableObject = meleeWeaponAttackMoveScriptableObject;
        this.meleeWeaponUserAble = meleeWeaponUserAble;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(meleeWeaponAttackMoveScriptableObject.animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.Anticipate.ToString(), this.Anticipate);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.PreAttack.ToString(), this.PreAttack);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.Attacking.ToString(), this.Attacking);
        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.PostAttack.ToString(), this.PostAttack);

        this.animationTriggerEventPlayer.GetNormalizedTimeFromStateName(
            MeleeAttackingPhase.Anticipate.ToString()
            , out beginmoveNormalizedTime
            );

        this.animationTriggerEventPlayer.GetNormalizedTimeFromStateName(
            MeleeAttackingPhase.PreAttack.ToString()
            , out moveNormalizedTime
            );

        this.character = character;
    }

    public override void Enter()
    {
        

        this.character._movementCompoent.CancleMomentum();

        this.targetPos = this.meleeWeaponUserAble._meleeWeaponUserTransform.position
            + (this.meleeWeaponUserAble._meleeWeaponUserTransform.forward * this.attackMoveScriptableObject._maxAttackMove_Range);

        this.attackingPhase = MeleeAttackingPhase.Anticipate;
        this.triggerIsReset = false;
        this.animationTriggerEventPlayer.Rewind();   
        base.Enter();
        this.meleeWeaponUserAble.OnNotifyMeleeAttack<AttackMoveMeleeWeaponNodeLeaf>(this);
    }
    public override void Exit()
    {
        this.attackingPhase = MeleeAttackingPhase.None;
        this.character.enableRootMotion = false;
        base.Exit();
        this.meleeWeaponUserAble.OnNotifyMeleeAttack<AttackMoveMeleeWeaponNodeLeaf>(this);
    }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        this.MoveUpdate();
        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if (this.meleeWeaponUserAble._isPerformAttackAble == false)
            return true;

        if(this.character.isDead)
            return true;

        if(this.IsComplete())
            return true;

        return this.triggerIsReset;
    }
    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }

    public void Anticipate() => this.attackingPhase = MeleeAttackingPhase.Anticipate;
    public void PreAttack() => this.attackingPhase = MeleeAttackingPhase.PreAttack;
    public void Attacking() => this.attackingPhase = MeleeAttackingPhase.Attacking;
    public void PostAttack() => this.attackingPhase = MeleeAttackingPhase.PostAttack;

    public void TriggerReset() => this.triggerIsReset = true;

    private void MoveUpdate()
    {
        switch (this.attackingPhase)
        {
            case MeleeAttackingPhase.None:
                {
                    this.character.enableRootMotion = true;
                    this.UpdateTargetPos();

                }break;
            case MeleeAttackingPhase.Anticipate:
                {
                    this.RotateUpdate();
                    this.character.enableRootMotion = false;
                    Vector3 targetDir = (this.targetPos - this.meleeWeaponUserAble._meleeWeaponUserTransform.position);
                    targetDir = new Vector3(targetDir.x,0,targetDir.z).normalized;

                    float t = this.animationTriggerEventPlayer.GetRemapNormalizedTimer(this.beginmoveNormalizedTime, this.moveNormalizedTime);

                    if (Vector3.Distance(this.targetPos, this.meleeWeaponUserAble._meleeWeaponUserTransform.position) > this.attackMoveScriptableObject._attackMove_Range)
                        this.character._movementCompoent.Move(targetDir * attackMoveScriptableObject._moveVelocityCurve.Evaluate(t) * this.attackMoveScriptableObject._topVelocityMove * Time.deltaTime);
                    else
                        this.character.enableRootMotion = true;
                    this.UpdateTargetPos();
                }
                break;
            case MeleeAttackingPhase.PreAttack:
                {
                    this.RotateUpdate();
                    this.character.enableRootMotion = true;
                    this.UpdateTargetPos();
                }
                break;
            default:
                {
                    this.character.enableRootMotion = true;
                    this.UpdateTargetPos();
                }
                break;
        }
       
    }
    private void RotateUpdate()
    {

        Vector3 targetDir = (this.targetPos - this.meleeWeaponUserAble._meleeWeaponUserTransform.position).normalized;

        this.character._movementCompoent.SetRotateToDirWorld(targetDir, this.attackMoveScriptableObject._rotateVelocity);
    }

    private void UpdateTargetPos()
    {
        if (this.meleeWeaponUserAble._targetTransform != null)
            this.targetPos = this.meleeWeaponUserAble._targetTransform.position;
    }
}
