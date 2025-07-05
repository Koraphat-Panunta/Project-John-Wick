using System;
using UnityEngine;

public class Hit2GunFuNode : PlayerGunFuHitNodeLeaf
{


    private bool isAlreadyHit;
    public Hit2GunFuNode(Player player, Func<bool> preCondition, OldGunFuHitScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {

    }
    public override void UpdateNode()
    {

        if (_timer >= _animationClip.length * hitAbleTime_Normalized 
            && _timer <= _animationClip.length * endHitableTime_Normalized
            && isAlreadyHit == false)
        {
            gotGunFuAttackedAble.TakeGunFuAttacked(this, player);
            curGunFuHitPhase = GunFuHitPhase.Hit;
            player.NotifyObserver(player,this);
            TimeControlBehavior.TriggerTimeStop(gunFuNodeScriptableObject.HitStopDuration,gunFuNodeScriptableObject.HitResetDuration);
            isAlreadyHit = true;
        }

        base.UpdateNode();
    }
    public override void Enter()
    {
        isAlreadyHit = false;
        base.Enter();
    }
    public override void FixedUpdateNode()
    {

        LerpingToTargetPos();
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {

        if (IsComplete())
            return true;

        if (player._movementCompoent.isGround == false)
            return true;

        return false;
    }

   
    

   
}
