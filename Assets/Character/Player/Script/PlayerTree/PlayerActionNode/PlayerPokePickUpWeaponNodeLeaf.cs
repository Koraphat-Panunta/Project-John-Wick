using System;
using UnityEngine;

public class PlayerPokePickUpWeaponNodeLeaf : PlayerStateNodeLeaf
{
    Transform rightFoots;
    Weapon pickedUpWeapon;
    public AnimationTriggerEventSCRP animationTriggerEventSCRP;
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;

    private bool isWarpingWeapon;
    public PlayerPokePickUpWeaponNodeLeaf(Player player
        , AnimationTriggerEventSCRP animationTriggerEventSCRP
        ,Transform rightFootPos
        , Func<bool> preCondition) : base(player, preCondition)
    {
        this.rightFoots = rightFootPos;
        this.animationTriggerEventSCRP = animationTriggerEventSCRP;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(this.animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent(animationTriggerEventSCRP.triggerEventDetail[0].eventName, Attaching);
    }
    
    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();  
        this.isWarpingWeapon = true;
        this.pickedUpWeapon = player.currentInteractable as Weapon;
        this.isComplete = false;

        Vector3 pushDir = this.pickedUpWeapon.transform.position - player.transform.position;
        pushDir = new Vector3(pushDir.x, 0, pushDir.z).normalized;
        (player._movementCompoent as IMotionImplusePushAble).AddForcePush(pushDir * 1.2f + (Vector3.up*2), IMotionImplusePushAble.PushMode.InstanlyMaintainMomentum);

        base.Enter();
    }
    public override void UpdateNode()
    {

        player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate * .7f, player.breakMaxSpeed * .7f, MoveMode.MaintainMomentum);

        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        if(isWarpingWeapon)
            this.pickedUpWeapon._weaponAttacherComponent.Hold(
                this.rightFoots.position + (Vector3.up * 0.1f)
                , Quaternion.LookRotation(rightFoots.right)
                , 1
                );
        
       

        base.UpdateNode();
    }
    private void Attaching()
    {
        isWarpingWeapon = false;
        player.weaponAdvanceUser._findingWeaponBehavior.SetWeaponFindingSelecting(this.pickedUpWeapon);
        player._isPickingUpWeaponCommand = true;
    }
    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }
    public override bool IsReset()
    {
        if(player.isDead)
            return true;

        if(player._triggerHitedGunFu)
            return true;

        return IsComplete();
    }
}
