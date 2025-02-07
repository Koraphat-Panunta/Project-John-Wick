using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit1GunFuNode : GunFuHitNodeLeaf
{

    private bool isHiting;

    private bool gunFuTriggerBuufer;

    private HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf;

    public Hit1GunFuNode(Player player,Func<bool> preCondition , GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {
        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFuInteraction_NodeLeaf(player, () => { return player.isAimingCommand; });
    }

    public override void Enter()
    {
        player._triggerGunFu = false;
        _timer = 0;
        gunFuDamagedAble = null;
        isHiting = false;
        gunFuTriggerBuufer = false;
        isDetectTarget = false;

        isDetectTarget = DetectTarget();

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuEnter);

        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);
        base.Exit();
    }
  
    public override void UpdateNode()
    {
        
        if(_timer >= _animationClip.length * 0f 
            && player._triggerGunFu)
            gunFuTriggerBuufer = true;

       

        if(_timer>=_animationClip.length*hitAbleTime_Normalized && _timer <= _animationClip.length * endHitableTime_Normalized
            && isHiting == false)
        {
            if (gunFuDamagedAble != null)
                gunFuDamagedAble.TakeGunFuAttacked(this,player);
            isHiting = true;
        }

        if (_isTransitionAble)
        {
            if ((player._triggerGunFu || gunFuTriggerBuufer )&&gunFuDamagedAble != null)
            {
                player.ChangeNode(player.Hit2GunFuNode);
                player.Hit2GunFuNode.gunFuDamagedAble = gunFuDamagedAble;
            }

            if (player.isAimingCommand && gunFuDamagedAble != null)
            {

                humanShield_GunFuInteraction_NodeLeaf.gunFuAttackedAble = gunFuDamagedAble;
                player.ChangeNode(humanShield_GunFuInteraction_NodeLeaf);
            }
        }

        
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        player.playerMovement.MoveToDirWorld(Vector3.zero, 6, 6, IMovementCompoent.MoveMode.MaintainMomentum);

        if (isDetectTarget)
            LerpingToTargetPos();
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {
        if (_isExit)
        {
            if (player.inputMoveDir_Local.magnitude > 0)
                return true;
        }

        if (_isExit)
            return true;

        return false;
    }
}
