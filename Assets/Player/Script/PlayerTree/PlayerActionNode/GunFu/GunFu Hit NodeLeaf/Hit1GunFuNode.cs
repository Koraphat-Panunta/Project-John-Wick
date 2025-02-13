using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit1GunFuNode : GunFuHitNodeLeaf
{



    public Hit1GunFuNode(Player player,Func<bool> preCondition , GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {

    }
    public override void UpdateNode()
    {
        if(_timer>=_animationClip.length*hitAbleTime_Normalized && _timer <= _animationClip.length * endHitableTime_Normalized
            && isHiting == false)
        {
            if (attackedAbleGunFu != null)
                attackedAbleGunFu.TakeGunFuAttacked(this,player);
            isHiting = true;
        }

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        player.playerMovement.MoveToDirWorld(Vector3.zero, 6, 6, IMovementCompoent.MoveMode.MaintainMomentum);

            LerpingToTargetPos();
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {
        if (isComplete)
        {
            if (player.inputMoveDir_Local.magnitude > 0)
                return true;
        }

        if (IsComplete())
            return true;

        return false;
    }
    
}
