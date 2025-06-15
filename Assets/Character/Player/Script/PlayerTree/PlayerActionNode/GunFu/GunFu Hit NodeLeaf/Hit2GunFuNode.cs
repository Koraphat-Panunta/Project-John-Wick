using System;
using UnityEngine;

public class Hit2GunFuNode : PlayerGunFuHitNodeLeaf
{

    private TimeControlBehavior timeControlBehavior;
    private bool isAlreadyHit;
    public Hit2GunFuNode(Player player, Func<bool> preCondition, GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {
        timeControlBehavior = new TimeControlBehavior();
    }
    public override void UpdateNode()
    {

        if (_timer >= _animationClip.length * hitAbleTime_Normalized 
            && _timer <= _animationClip.length * endHitableTime_Normalized
            && isAlreadyHit == false)
        {
            attackedAbleGunFu.TakeGunFuAttacked(this, player);
            player.NotifyObserver(player, SubjectPlayer.NotifyEvent.GunFuAttack);
            timeControlBehavior.TriggerTimeStop(gunFuNodeScriptableObject.HitStopDuration,gunFuNodeScriptableObject.HitResetDuration);
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

        if (player.playerMovement.isGround == false)
            return true;

        return false;
    }

   
    

   
}
