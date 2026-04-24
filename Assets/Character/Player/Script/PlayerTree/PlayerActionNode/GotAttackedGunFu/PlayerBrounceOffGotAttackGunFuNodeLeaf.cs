using System;
using UnityEngine;

public class PlayerBrounceOffGotAttackGunFuNodeLeaf : PlayerStateNodeLeaf, IGotGunFuAttackNode
{
    public PlayerBrounceOffGotAttackGunFuNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public float _exitTime_Normalized { get ; set ; }
    public float _timer { get ; set ; }
    public AnimationClip _animationClip { get; set; }

    public override void Enter()
    {
        isComplete = false;
        _timer = 0;

        Vector3 rotateDir =    player.gunFuAbleAttacker._character.transform.position - player.transform.position ;
        player._movementCompoent.SetRotation(Quaternion.LookRotation(rotateDir));
        if (player._currentWeapon != null)
            WeaponAttachingBehavior.Detach(player._currentWeapon,player);
        base.Enter();
    }

    public override void Exit()
    {
        player.curAttackerGunFuNode = null;
        player.gunFuAbleAttacker = null;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if (_timer >= _animationClip.length * .5f)
            player._movementCompoent.UpdateMoveToDirWorld(Vector3.zero, 
                this.player.breakDecelerate,
                MoveMode.MaintainMomentumDirection);

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

        if(_timer >= this._animationClip.length)
            isComplete = true;

        base.UpdateNode();
    }
}



