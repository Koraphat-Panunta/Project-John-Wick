using System;
using UnityEngine;

public class GunFuReloadNodeLeaf : PlayerGunFu_Interaction_NodeLeaf
{

    public SubjectAnimationInteract SubjectAnimationInteract1;
    public SubjectAnimationInteract SubjectAnimationInteract2;

    public AnimationTriggerEventPlayer animationTriggerEventPlayer;
    public AnimationTriggerAudioEventPlayer audioAnimationTriggerEvent;

    public bool isReload;

    public enum GunFuReloadPhase
    {
        Enter,
        TriggerReload,
        Exit
    }
    public GunFuReloadPhase curGunFuReloadPhase;

    public GunFuReloadNodeLeaf
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

        this.SubjectAnimationInteract1 = new SubjectAnimationInteract
            (animationInteractScriptableObject
            , animationInteractScriptableObject.animationInteractCharacterDetail[0]
            );

        this.SubjectAnimationInteract2 = new SubjectAnimationInteract
            (
            animationInteractScriptableObject
            , animationInteractScriptableObject.animationInteractCharacterDetail[1]
            );

        this.animationTriggerEventPlayer.SubscribeEvent("TriggerReload", this.TriggerReload);
        this.animationTriggerEventPlayer.SubscribeEvent("TransitionAble", this.TransitionAbleAll);

        this.audioAnimationTriggerEvent = new AnimationTriggerAudioEventPlayer
            (animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.audioAnimationInteractTriggerEvents);
    }

    public override void Enter()
    {
        
        this.isComplete = false;

        this.isReload = false;
        this.gotGunFuAttackedAble = this.player.attackedAbleGunFu;
        Vector3 anchorPos = this.gotGunFuAttackedAble._character.transform.position;
        Vector3 anchorDir = (this.gotGunFuAttackedAble._character.transform.position - this.gunFuAble._character.transform.position);
        anchorDir = new Vector3 (anchorDir.x, 0 , anchorDir.z).normalized;


        this.SubjectAnimationInteract1.finishWarpEvent += this.Interact;
        this.SubjectAnimationInteract1.RestartSubject(this.gunFuAble._character, anchorPos, anchorDir);
        this.SubjectAnimationInteract2.RestartSubject(this.gotGunFuAttackedAble._character, anchorPos, anchorDir);

        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
        this.animationTriggerEventPlayer.Rewind();
        this.audioAnimationTriggerEvent.Rewind();

        this.curGunFuReloadPhase = GunFuReloadPhase.Enter;
        base.Enter();
    }
    public override void Exit()
    {
        this.player.enableRootMotion = false;
        this.curGunFuReloadPhase = GunFuReloadPhase.Exit;
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
    protected void TriggerReload()
    {
        IWeaponAdvanceUser weaponAdvanceUser = this.player;
        this.curGunFuReloadPhase = GunFuReloadPhase.TriggerReload;

        if (weaponAdvanceUser._currentWeapon != null)
        {
            weaponAdvanceUser._isReloadCommand = true;
            this.isReload = true;
            this.player.NotifyObserver(this.player, this.curGunFuReloadPhase);
            Debug.Log("TriggerReload timeNor = "+this.SubjectAnimationInteract1.animationTriggerEventPlayer.timerNormalized);
        }
    }
}
