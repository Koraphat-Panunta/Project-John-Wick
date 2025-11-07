using System;
using UnityEngine;

public class PlayerPokePickUpWeaponNodeLeaf : PlayerStateNodeLeaf
{
    Transform rightFoots;
    Weapon pickedUpWeapon;
    private AnimationClip clip;
    float warpingNormalized;
    float exitNormalized;
    float timer;
    private bool isPickUp;
    public PlayerPokePickUpWeaponNodeLeaf(Player player
        ,AnimationClip pokePickUuClip
        ,float warpingNormalized
        ,float exitNormalized
        ,Transform rightFootPos
        , Func<bool> preCondition) : base(player, preCondition)
    {
        this.clip = pokePickUuClip;
        this.warpingNormalized = warpingNormalized;
        this.exitNormalized = exitNormalized;
        this.rightFoots = rightFootPos;

    }
    
    public override void Enter()
    {
        this.timer = 0;
        this.pickedUpWeapon = player.currentInteractable as Weapon;
        this.isComplete = false;
        this.isPickUp = false;

        Vector3 pushDir = this.pickedUpWeapon.transform.position - player.transform.position;
        pushDir = new Vector3(pushDir.x, 0, pushDir.z).normalized;
        (player._movementCompoent as IMotionImplusePushAble).AddForcePush(pushDir * 1.2f, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);

        base.Enter();
    }
    public override void UpdateNode()
    {

        player._movementCompoent.MoveToDirWorld(Vector3.zero, player.breakDecelerate, player.breakMaxSpeed, MoveMode.MaintainMomentum);

        this.timer += Time.deltaTime;
        if(this.timer < this.clip.length * warpingNormalized)
        {
            this.pickedUpWeapon._weaponAttacherComponent.Hold(this.rightFoots.position, Quaternion.LookRotation(rightFoots.right), timer / (this.clip.length * warpingNormalized));
        }
        else if(isPickUp == false)
        {
            player.weaponAdvanceUser._findingWeaponBehavior.SetWeaponFindingSelecting(this.pickedUpWeapon);
            player._isPickingUpWeaponCommand = true;
            isPickUp = true;
        }

        if(this.timer >= this.clip.length * exitNormalized)
            isComplete = true;

        base.UpdateNode();
    }

    public override bool IsComplete()
    {
        return isComplete;
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
