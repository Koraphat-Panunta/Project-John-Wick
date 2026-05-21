using System;
using UnityEngine;

public class OCM_KnockDown_NodeLeaf : PlayerGunFu_Interaction_NodeLeaf,IHPDamageVisitor
{
    public SubjectAnimationInteract SubjectAnimationInteract1;
    public SubjectAnimationInteract SubjectAnimationInteract2;

    public AnimationTriggerEventPlayer animationTriggerEventPlayer;
    public AnimationTriggerAudioEventPlayer audioAnimationTriggerEvent;


    public float guardDamage { get; }

    public float _hPDamage { get => 0; }

    public enum KnockDownPhase
    {
        Enter,
        TriggerKnockDown,
        Exit
    }
    public KnockDownPhase curKnockDownPhase;

    public OCM_KnockDown_NodeLeaf
        (
        Player player
        , Func<bool> preCondition
        , AnimationInteractScriptableObject animationInteractScriptableObject
        ) : base(player, preCondition)
    {
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(
            animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.triggerEventDetail
            );

        this.SubjectAnimationInteract1 = new SubjectAnimationInteract(
            animationInteractScriptableObject
            , animationInteractScriptableObject.animationInteractCharacterDetail[0]
            );

        this.SubjectAnimationInteract2 = new SubjectAnimationInteract(
            animationInteractScriptableObject
            , animationInteractScriptableObject.animationInteractCharacterDetail[1]
            );

        this.audioAnimationTriggerEvent = new AnimationTriggerAudioEventPlayer(
            animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.audioAnimationInteractTriggerEvents
            );

        this.animationTriggerEventPlayer.SubscribeEvent("KnockDown",this.KnockDown);
    }

    public override void Enter()
    {
        this.isComplete = false;

        this.curKnockDownPhase = KnockDownPhase.Enter;
        this.gotGunFuAttackedAble = this.player.attackedAbleGunFu;

        Vector3 anchorPos = this.gotGunFuAttackedAble._character.transform.position;
        Vector3 anchorDir = (this.gotGunFuAttackedAble._character.transform.position - this.gunFuAble._character.transform.position);
        anchorDir = new Vector3(anchorDir.x, 0, anchorDir.z).normalized;

        this.SubjectAnimationInteract1.finishWarpEvent += this.Interact;
        this.SubjectAnimationInteract1.RestartSubject(this.gunFuAble._character, anchorPos, anchorDir);
        this.SubjectAnimationInteract2.RestartSubject(this.gotGunFuAttackedAble._character, anchorPos, anchorDir);


        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
        this.animationTriggerEventPlayer.Rewind();
        this.audioAnimationTriggerEvent.Rewind();

        base.Enter();
    }

    public override void Exit()
    {
        this.player.enableRootMotion = false;
        this.curKnockDownPhase = KnockDownPhase.Exit;
        base.Exit();
    }

    public override void UpdateNode()
    {
        this.SubjectAnimationInteract1.UpdateInteract(Time.deltaTime);
        this.SubjectAnimationInteract2.UpdateInteract(Time.deltaTime);
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        this.audioAnimationTriggerEvent.Update(Time.deltaTime, this.gunFuAble._character.transform.position);

        if (this.animationTriggerEventPlayer.IsPlayFinish())
            this.isComplete = true;

        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    protected void Interact(Character character)
    {
        _ = SubjectAnimationInteract.DelayRootMotion(character);
    }

    public override bool IsComplete()
    {
        return this.isComplete;
    }

    public override bool IsReset()
    {
        return this.IsComplete();
    }

    protected void TransitionAbleAll() => this.nodeLeafTransitionBehavior.TransitionAbleAll(this);

    public void KnockDown()
    {
        this.curKnockDownPhase = KnockDownPhase.TriggerKnockDown;
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
        this.player.NotifyObserver(this.player, this.curKnockDownPhase);
    }
}
