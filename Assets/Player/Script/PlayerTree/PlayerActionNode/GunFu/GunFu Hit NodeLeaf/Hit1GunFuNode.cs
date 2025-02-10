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

        Debug.Log("GunFu Hit 1 Enter");

        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuExit);

        Debug.Log("GunFu Hit 1 Exit");
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
                (player.playerStateNodeManager as PlayerStateNodeManager).
               ChangeNode((player.playerStateNodeManager as PlayerStateNodeManager).Hit2GunFuNode);

                (player.playerStateNodeManager as PlayerStateNodeManager).Hit2GunFuNode.gunFuDamagedAble = gunFuDamagedAble;
            }

            if (player.isAimingCommand && gunFuDamagedAble != null)
            {

                (player.playerStateNodeManager as PlayerStateNodeManager).
              ChangeNode(humanShield_GunFuInteraction_NodeLeaf);
            }
        }
        Debug.Log("GunFu Hit 1 Update");

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
