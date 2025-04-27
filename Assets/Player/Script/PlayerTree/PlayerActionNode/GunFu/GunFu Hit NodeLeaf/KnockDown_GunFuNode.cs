using System;
using UnityEngine;

public class KnockDown_GunFuNode : PlayerGunFuHitNodeLeaf
{
    private bool isKicking;
    private TimeControlBehavior timeControlBehavior;
    public KnockDown_GunFuNode(Player player,Func<bool> preCondition, GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {
        timeControlBehavior = new TimeControlBehavior();
    }

    public override void UpdateNode()
    {
        if (_timer >= _animationClip.length * hitAbleTime_Normalized && _timer <= _animationClip.length * endHitableTime_Normalized)
        {
            if (isKicking == false)
            {
                attackedAbleGunFu.TakeGunFuAttacked(this, player);
                player.attackedAbleGunFu = null;
                isKicking = true;
                timeControlBehavior.TriggerTimeStop(gunFuNodeScriptableObject.HitStopDuration, gunFuNodeScriptableObject.HitResetDuration);
                player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuAttack);
            }
        }

        base.UpdateNode();
    }
    public override void Enter()
    {
        isKicking = false;
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
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
