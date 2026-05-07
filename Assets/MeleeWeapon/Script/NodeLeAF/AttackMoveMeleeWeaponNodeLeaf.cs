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
        this.character = character;
    }


    public override void Enter()
    {
        this.attackingPhase = MeleeAttackingPhase.Anticipate;
        this.triggerIsReset = false;
        this.animationTriggerEventPlayer.Rewind();   
        base.Enter();
        this.meleeWeaponUserAble.OnNotifyMeleeAttack<AttackMoveMeleeWeaponNodeLeaf>(this);
    }
    public override void Exit()
    {
        this.attackingPhase = MeleeAttackingPhase.None;
        base.Exit();
        this.meleeWeaponUserAble.OnNotifyMeleeAttack<AttackMoveMeleeWeaponNodeLeaf>(this);
    }
    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        this.MoveUpdate();
        base.UpdateNode();
    }

    public override bool IsReset()
    {
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
        if(this.meleeWeaponUserAble._targetTransform == null
            || Vector3.Distance
            (this.meleeWeaponUserAble._meleeWeaponUserTransform.position
            ,this.meleeWeaponUserAble._targetTransform.position) > this.attackMoveScriptableObject._attackRange
            )
            this.character.enableRootMotion = true;
        else
            this.character.enableRootMotion = false;
    }
}
