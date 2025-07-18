using System;
using UnityEngine;

public class PlayerBrounceOffGotAttackGunFuNodeLeaf : PlayerStateNodeLeaf, IGotGunFuAttackNode
{
    public float _exitTime_Normalized { get ; set ; }
    public float _timer { get ; set ; }
    public AnimationClip _animationClip { get => brounceOffGotAttackGunFuScriptableObject.animationClip ; set => brounceOffGotAttackGunFuScriptableObject.animationClip = value; }
    private PlayerBrounceOffGotAttackGunFuScriptableObject brounceOffGotAttackGunFuScriptableObject;
    public PlayerBrounceOffGotAttackGunFuNodeLeaf(PlayerBrounceOffGotAttackGunFuScriptableObject playerBrounceOffGotAttackGunFuScriptableObject,Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        this.brounceOffGotAttackGunFuScriptableObject = playerBrounceOffGotAttackGunFuScriptableObject;
    }

    public override void Enter()
    {
        isComplete = false;
        _timer = 0;
        player.playerStance = Player.PlayerStance.prone;

        Vector3 rotateDir =    player.gunFuAbleAttacker._character.transform.position - player.transform.position ;
        player._movementCompoent.SetRotation(Quaternion.LookRotation(rotateDir));
        if (player._currentWeapon != null)
            new WeaponAttachingBehavior().Detach(player._currentWeapon,player);
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
        if (_timer >= _animationClip.length * brounceOffGotAttackGunFuScriptableObject.onGroundNormalized)
            player._movementCompoent.MoveToDirWorld(Vector3.zero, 
                brounceOffGotAttackGunFuScriptableObject.breakForcingOnGround,
                brounceOffGotAttackGunFuScriptableObject.breakForcingOnGround,
                MoveMode.MaintainMomentum);

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



