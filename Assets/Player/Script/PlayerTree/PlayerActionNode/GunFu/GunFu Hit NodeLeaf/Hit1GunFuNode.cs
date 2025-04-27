using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit1GunFuNode : PlayerGunFuHitNodeLeaf
{


    private TimeControlBehavior timeControlBehavior;
    private bool isHitAlready;
    public Hit1GunFuNode(Player player,Func<bool> preCondition , GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {
        timeControlBehavior = new TimeControlBehavior();
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
            timeControlBehavior.TriggerTimeStop(gunFuNodeScriptableObject.HitStopDuration,gunFuNodeScriptableObject.HitResetDuration);
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuAttack);
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
