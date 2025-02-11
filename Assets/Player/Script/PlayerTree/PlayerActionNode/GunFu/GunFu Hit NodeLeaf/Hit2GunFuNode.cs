using System;
using UnityEngine;

public class Hit2GunFuNode : GunFuHitNodeLeaf
{

    private bool isHiting;

    private bool gunFuTriggerBuufer;
    public Hit2GunFuNode(Player player, Func<bool> preCondition, GunFuHitNodeScriptableObject gunFuNodeScriptableObject) : base(player,preCondition, gunFuNodeScriptableObject)
    {
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
        player.playerMovement.MoveToDirWorld(Vector3.zero, 6, 6, IMovementCompoent.MoveMode.MaintainMomentum);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        player.playerMovement.MoveToDirWorld(Vector3.zero, 6, 6, IMovementCompoent.MoveMode.MaintainMomentum);

        if(isDetectTarget)
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

        if (isComplete)
            return true;

        return false;
    }

    public override void UpdateNode()
    {

        if (_timer > _animationClip.length * 0.2f
           && player._triggerGunFu)
            gunFuTriggerBuufer = true;

        if (_timer >= _animationClip.length * hitAbleTime_Normalized && _timer <= _animationClip.length * endHitableTime_Normalized
           && isHiting == false)
        {
            if (gunFuDamagedAble != null)
                gunFuDamagedAble.TakeGunFuAttacked(this, player);
            isHiting = true;
        }


        if (_timer >= _animationClip.length * _transitionAbleTime_Nornalized)
        {

        }
        if (_isTransitionAble &&
            (player._triggerGunFu || gunFuTriggerBuufer)
            && gunFuDamagedAble != null)
            (player.playerStateNodeManager as PlayerStateNodeManager).
                ChangeNode((player.playerStateNodeManager as PlayerStateNodeManager).knockDown_GunFuNode);

        base.UpdateNode();
    }
    

   
}
