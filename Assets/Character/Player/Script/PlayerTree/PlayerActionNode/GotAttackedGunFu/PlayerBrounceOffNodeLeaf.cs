using System;
using UnityEngine;

public class PlayerBrounceOffNodeLeaf : PlayerStateNodeLeaf
{
    public PlayerBrounceOffNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public float _exitTime_Normalized { get ; set ; }
    public float _timer { get ; set ; }

    public float downTime = 1;

    public override void Enter()
    {
        isComplete = false;
        _timer = 0;
        this.player.enableRootMotion = true;

        base.Enter();
    }

    public override void Exit()
    {
        Vector3 proneDir = this.player.humanoidBone._headBone.transform.position - this.player.humanoidBone.hips.position;
        proneDir = new Vector3(proneDir.x,0,proneDir.z).normalized;

        this.player.playerMovement.SetProneDir(proneDir);
        this.player.enableRootMotion = false;
        player.curAttackerGunFuNode = null;
        player.gunFuAbleAttacker = null;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        //player._movementCompoent.UpdateMovement();
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {
        if(player.isDead)
            return true;

        if(IsComplete())
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        //player._movementCompoent.UpdateMoveToDirWorld(Vector3.zero,
        // this.player.breakDecelerate,
        // MoveMode.MaintainMomentumDirection);

        if (_timer >= this.downTime)
            isComplete = true;

        base.UpdateNode();
    }
}



