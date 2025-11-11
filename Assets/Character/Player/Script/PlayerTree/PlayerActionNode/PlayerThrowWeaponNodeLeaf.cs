using System;
using UnityEngine;

public class PlayerThrowWeaponNodeLeaf : PlayerStateNodeLeaf
{
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;
    private Weapon throwWeapon;
    private bool isThrowing;
    public PlayerThrowWeaponNodeLeaf(Player player,AnimationTriggerEventSCRP animationTriggerEventSCRP, Func<bool> preCondition) : base(player, preCondition)
    {
        animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
        animationTriggerEventPlayer.SubscribeEvent("Throwing", this.Throwing);
    }
    public override void Enter()
    {
        (player._movementCompoent as IMotionImplusePushAble).AddForcePush(player.transform.forward * 1.2f, IMotionImplusePushAble.PushMode.InstanlyMaintainMomentum);
        this.isThrowing = false;
        this.throwWeapon = player._currentWeapon;
        animationTriggerEventPlayer.Rewind();
        WeaponAttachingBehavior.Detach(this.throwWeapon, this.player);
        throwWeapon._weaponAttacherComponent.Attach(player.weaponAdvanceUser._mainHandSocket.weaponAttachingAbleTransform, throwWeapon._SecondHandGripTransform, Vector3.zero, Quaternion.identity);
        base.Enter();
    }
    public override void Exit()
    {
        if(isThrowing == false)
        {
            throwWeapon._weaponAttacherComponent.Detach();
        }
        base.Exit();
    }
    public override void UpdateNode()
    {
        player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate * .7f, player.breakMaxSpeed * .7f, MoveMode.MaintainMomentum);
        animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
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
            this.throwWeapon.Throw(player, player.curBeenThrowObjectAt);
        else
            this.throwWeapon.Throw(player, player.transform.position + (player.transform.forward * 5));
    }
}
