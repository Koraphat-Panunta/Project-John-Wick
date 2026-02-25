using System;
using UnityEngine;

public class GunFuHitDownNodeLeaf : PlayerGunFu_Interaction_NodeLeaf
    ,IGunFuNode
    ,IHPDamageVisitor
    ,IPostureDamageVisitor
{

    public SubjectAnimationInteract SubjectAnimationInteract1;
    public SubjectAnimationInteract SubjectAnimationInteract2;

    public AnimationTriggerEventPlayer animationTriggerEventPlayer;
    protected GunFuHitScriptableObject hitScriptableObject;
   
    public float _hPDamage => this.hitScriptableObject.gunFuHitDetail[0].hpHitDamage;
    public float _postureDamageVisitor => this.hitScriptableObject.gunFuHitDetail[0].postureHitDamage;

    public enum GunFuHitDownPhase
    {
        Restrain,
        Attack,
        PullUp,
    }

    public GunFuHitDownPhase gunFuHitDownPhase;

    public GunFuHitDownNodeLeaf(
        Player player
        ,AnimationInteractScriptableObject animationInteractScriptableObject 
        ,GunFuHitScriptableObject gunFuHitScriptableObject
        ,Func<bool> preCondition) 
        : base(player, preCondition)
    {
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(
            animationInteractScriptableObject.clip
            ,animationInteractScriptableObject.enterNormalizedTime
            ,animationInteractScriptableObject.endNormalizedTime
            ,animationInteractScriptableObject.triggerEventDetail
            );

        this.SubjectAnimationInteract1 = new SubjectAnimationInteract
            (animationInteractScriptableObject
            , animationInteractScriptableObject.animationInteractCharacterDetail[0]
            );

        this.SubjectAnimationInteract2 = new SubjectAnimationInteract
            (
            animationInteractScriptableObject
            ,animationInteractScriptableObject.animationInteractCharacterDetail[1]
            );

        this.hitScriptableObject = gunFuHitScriptableObject;
        this.animationTriggerEventPlayer.SubscribeEvent("Hit", Hit);
        this.animationTriggerEventPlayer.SubscribeEvent("PullUp", PullUp);
        this.animationTriggerEventPlayer.SubscribeEvent("TransitionAble", this.TransitionAbleAll);
    }

    public override void Enter()
    {


        this.isComplete = false;

        this.gotGunFuAttackedAble = this.player.attackedAbleGunFu;
        Vector3 anchorPos = this.gotGunFuAttackedAble._character.transform.position;
        Vector3 anchorDir = (this.gotGunFuAttackedAble._character.transform.position - this.gunFuAble._character.transform.position);
        anchorDir = new Vector3(anchorDir.x,this.gotGunFuAttackedAble._character.transform.position.y,anchorDir.z).normalized;

        this.SubjectAnimationInteract1.finishWarpEvent += this.Interact;
        this.SubjectAnimationInteract1.RestartSubject(this.gunFuAble._character,anchorPos,anchorDir);
        this.SubjectAnimationInteract2.RestartSubject(this.gotGunFuAttackedAble._character, anchorPos, anchorDir);

        this.gunFuHitDownPhase = GunFuHitDownPhase.Restrain;
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
        this.animationTriggerEventPlayer.Rewind();
        base.Enter();
    }
    public override void Exit()
    {
        this.player.enableRootMotion = false;
        base.Exit();
    }

    public override void UpdateNode()
    {
        this.SubjectAnimationInteract1.UpdateInteract(Time.deltaTime);
        this.SubjectAnimationInteract2.UpdateInteract(Time.deltaTime);
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        if(this.animationTriggerEventPlayer.IsPlayFinish())
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
    protected void Hit()
    {
        this.gunFuHitDownPhase = GunFuHitDownPhase.Attack;
        base.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
        this.player.NotifyObserver(this.player,this);
    }
    protected void PullUp()
    {
        this.gunFuHitDownPhase = GunFuHitDownPhase.PullUp;
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this,this.gunFuAble);
        this.player.NotifyObserver(this.player, this);
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
}
