using System;
using UnityEngine;

public class KnockDown_GunFuNode : GunFuHitNodeLeaf
{
    public KnockDown_GunFuNode(Player player,Func<bool> preCondition, GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {
    }

    public override void UpdateNode()
    {
        if (_timer >= _animationClip.length * hitAbleTime_Normalized && _timer <= _animationClip.length * endHitableTime_Normalized)
        {
            attackedAbleGunFu.TakeGunFuAttacked(this, player);
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

        if (player.playerMovement.isGround == false)
            return true;

        return false;
    }

    
   
   
    
}
