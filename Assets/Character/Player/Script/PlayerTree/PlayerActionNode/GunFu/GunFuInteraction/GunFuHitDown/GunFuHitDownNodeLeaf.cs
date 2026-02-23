using System;
using UnityEngine;

public class GunFuHitDownNodeLeaf : PlayerGunFu_Interaction_NodeLeaf,IHPDamageVisitor,IPostureDamageVisitor
{
    public AnimationTriggerEventPlayer animationTriggerEventPlayer;
    protected GunFuHitScriptableObject hitScriptableObject;
   
    public float _hPDamage => this.hitScriptableObject.gunFuHitDetail[0].hpHitDamage;
    public float _postureDamageVisitor => this.hitScriptableObject.gunFuHitDetail[0].postureHitDamage;

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

        this.hitScriptableObject = gunFuHitScriptableObject;
        this.animationTriggerEventPlayer.SubscribeEvent("Hit", Hit);
    }

    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
        base.Enter();
    }

    protected void Hit()
    {
        base.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
    }
}
