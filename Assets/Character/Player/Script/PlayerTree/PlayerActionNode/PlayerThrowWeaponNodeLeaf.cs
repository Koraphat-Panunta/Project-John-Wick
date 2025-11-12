using System;
using UnityEngine;

public class PlayerThrowWeaponNodeLeaf : PlayerStateNodeLeaf
{
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;
    private Weapon throwWeapon;
    private bool isThrowing;
    private Vector3 startCastPos => player.RayCastPos.position;
    private Vector3 castDir => (player._lookingPos - startCastPos).normalized;
    private float castDistance = 7;
    public PlayerThrowWeaponNodeLeaf(Player player,AnimationTriggerEventSCRP animationTriggerEventSCRP, Func<bool> preCondition) : base(player, preCondition)
    {
        animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
        animationTriggerEventPlayer.SubscribeEvent("Throwing", this.Throwing);
        this.targetMask = LayerMask.GetMask("Enemy");
    }
    public override void Enter()
    {
         if(CastFinding.FindObectInViewByComponent<IBeenThrewObjectAt>(
            this.startCastPos
            , this.castDir
            , this.castDistance
            , 1
            , this.targetMask
            , out IBeenThrewObjectAt beenThrewObjectAt))
        player.curBeenThrowObjectAt = beenThrewObjectAt;

        this.isThrowing = false;
        this.throwWeapon = player._currentWeapon;
        this.animationTriggerEventPlayer.Rewind();
        WeaponAttachingBehavior.Detach(this.throwWeapon, this.player);
        throwWeapon._weaponAttacherComponent.Attach(player.weaponAdvanceUser._mainHandSocket.weaponAttachingAbleTransform, throwWeapon._SecondHandGripTransform, Vector3.zero,
            Quaternion.FromToRotation(player.weaponAdvanceUser._mainHandSocket.weaponAttachingAbleTransform.forward, player.weaponAdvanceUser._mainHandSocket.weaponAttachingAbleTransform.forward * -1));
        base.Enter();
    }
    public override void Exit()
    {
        if(isThrowing == false)
        {
            throwWeapon._weaponAttacherComponent.Detach();
        }
        player.curBeenThrowObjectAt = null;
        base.Exit();
    }
    public override void UpdateNode()
    {
        if (player.curBeenThrowObjectAt != null)
            this.player._movementCompoent.RotateToDirWorld(
                (player.curBeenThrowObjectAt._beenThrowObjectAtPosition - player.transform.position).normalized
                , player.StandMoveRotateSpeed * 2
                );
        else
        {
            this.player._movementCompoent.RotateToDirWorld(
                (player._lookingPos - player.transform.position).normalized
                , player.StandMoveRotateSpeed * 2
                );
        }

        this.player._movementCompoent.MoveToDirWorld(
            Vector3.zero
            , this.player.breakDecelerate 
            , this.player.breakMaxSpeed 
            , MoveMode.MaintainMomentum);
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        base.UpdateNode();
    }
    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }
    public override bool IsReset()
    {
        if(this.IsComplete())
            return true;

        if(player.isDead)
            return true;

        if(player._triggerHitedGunFu)
            return true;

        return false;
    }
    private void Throwing()
    {
        this.isThrowing = true;
        throwWeapon._weaponAttacherComponent.Detach();
        if (player.curBeenThrowObjectAt != null)
            this.throwWeapon.Throw(player, player.curBeenThrowObjectAt,this.targetMask);
        else
            this.throwWeapon.Throw(player, player._lookingPos ,this.targetMask);
    }
    private LayerMask targetMask;
    
}
