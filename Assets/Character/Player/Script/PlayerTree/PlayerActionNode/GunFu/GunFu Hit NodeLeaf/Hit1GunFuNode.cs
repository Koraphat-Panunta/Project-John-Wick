using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit1GunFuNode : PlayerGunFuHitNodeLeaf
{



    private bool isHitAlready;
    public Hit1GunFuNode(Player player,Func<bool> preCondition , GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {

    }
    public override void Enter()
    {
        isHitAlready = false;
        base.Enter();
    }
    public override void UpdateNode()
    {
        
        if ( _timer>=_animationClip.length*hitAbleTime_Normalized 
            && _timer <= _animationClip.length * endHitableTime_Normalized 
            && isHitAlready == false)
        { 
            attackedAbleGunFu.TakeGunFuAttacked(this,player);
            TimeControlBehavior.TriggerTimeStop(gunFuNodeScriptableObject.HitStopDuration,gunFuNodeScriptableObject.HitResetDuration);
            curGunFuHitPhase = GunFuHitPhase.Hit;
            player.NotifyObserver(player, this);
            isHitAlready = true;
        }

        base.UpdateNode();
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

        if(player.playerMovement.isGround == false)
            return true;

        return false;
    }
    
}
