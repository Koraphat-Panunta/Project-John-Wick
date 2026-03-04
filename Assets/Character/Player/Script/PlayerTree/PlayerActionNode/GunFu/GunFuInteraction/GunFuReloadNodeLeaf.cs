using System;
using UnityEngine;

public class GunFuReloadNodeLeaf : PlayerGunFu_Interaction_NodeLeaf
{

    public SubjectAnimationInteract SubjectAnimationInteract1;
    public SubjectAnimationInteract SubjectAnimationInteract2;

    public AnimationTriggerEventPlayer animationTriggerEventPlayer;

    public bool isReload;

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
    }

    public override void Enter()
    {
        
        this.isComplete = false;

        this.isReload = false;
        this.gotGunFuAttackedAble = this.player.attackedAbleGunFu;
        Vector3 anchorPos = this.gotGunFuAttackedAble._character.transform.position;
        Vector3 anchorDir = (this.gotGunFuAttackedAble._character.transform.position - this.gunFuAble._character.transform.position);
        anchorDir = new Vector3(anchorDir.x, this.gotGunFuAttackedAble._character.transform.position.y, anchorDir.z).normalized;

        this.SubjectAnimationInteract1.finishWarpEvent += this.Interact;
        this.SubjectAnimationInteract1.RestartSubject(this.gunFuAble._character, anchorPos, anchorDir);
        this.SubjectAnimationInteract2.RestartSubject(this.gotGunFuAttackedAble._character, anchorPos, anchorDir);

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
        IWeaponAdvanceUser weaponAdvanceUser = this.player.weaponAdvanceUser;

        if(weaponAdvanceUser._currentWeapon != null)
        {
            weaponAdvanceUser._isReloadCommand = true;
            this.isReload = true;
        }
    }
}
