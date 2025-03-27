using System;
using System.Collections;
using UnityEngine;

public class WeaponDisarm_GunFuInteraction_NodeLeaf : GunFu_Interaction_NodeLeaf
{
    private float pull = 0.14f;
    private float duration = 0.5f;

    private float elapesTime;

    public Weapon disarmedWeapon;

    private bool isDisarmWeapon;


    public enum WeaponDisarmPhase
    {
        None,
        Pull,
        Disarmed,
        AfterDisarmed
    }

    public WeaponDisarmPhase curPhase;

    public WeaponDisarm_GunFuInteraction_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("WeaponDisarm_GunFuInteraction_NodeLeaf Enter");
        isComplete = true;
        attackedAbleGunFu = player.attackedAbleGunFu;
        curPhase = WeaponDisarmPhase.None;
        elapesTime = 0;
        disarmedWeapon = attackedAbleGunFu._weaponAdvanceUser._currentWeapon;
        attackedAbleGunFu.TakeGunFuAttacked(this, player);
        isDisarmWeapon = false;

        base.Enter();
    }

    public override void Exit()
    {
        curPhase = WeaponDisarmPhase.None;
        attackedAbleGunFu = null;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        Debug.Log("WeaponDisarm_GunFuInteraction_NodeLeaf FixedUpdate");
        if(elapesTime <= pull)
        {
            curPhase = WeaponDisarmPhase.Pull;
            attackedAbleGunFu._gunFuAttackedAble.position = Vector3.Lerp(
                       attackedAbleGunFu._gunFuAttackedAble.position,
                       targetAdjustTransform.position,
                       elapesTime / pull
                       );
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
            attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(attackedAbleGunFu._gunFuAttackedAble.rotation, Quaternion.LookRotation(targetAdjustTransform.forward*-1,Vector3.up), elapesTime / pull);

        }
        
        if(elapesTime > pull && isDisarmWeapon == false)
        {
            disarmedWeapon.DropWeapon();
            
            player.StartCoroutine(SlowMotion());
            curPhase = WeaponDisarmPhase.Disarmed;
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
            isDisarmWeapon = true;
            (attackedAbleGunFu._movementCompoent as IMotionImplusePushAble).AddForcePush(player.transform.forward * 2.5f,IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
            if (player.weaponAdvanceUser._currentWeapon == null)
            {
                disarmedWeapon.AttatchWeaponToNoneOverrideAnimator(player.weaponAdvanceUser);
            }
            else if (player.weaponAdvanceUser._currentWeapon != player.weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon
                && player.weaponAdvanceUser._currentWeapon != player.weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon)
            {
                player.weaponAdvanceUser._currentWeapon.DropWeapon();
                disarmedWeapon.AttatchWeaponToNoneOverrideAnimator(player.weaponAdvanceUser);
            }
            else if (player.weaponAdvanceUser._currentWeapon != null
                && player.weaponAdvanceUser._currentWeapon is PrimaryWeapon)
            {
                player.weaponAdvanceUser._currentWeapon.AttachWeaponToSocketNoneAnimatorOverride(player.weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
                disarmedWeapon.AttatchWeaponToNoneOverrideAnimator(player.weaponAdvanceUser);

            }
            else if (player.weaponAdvanceUser._currentWeapon != null
                && player.weaponAdvanceUser._currentWeapon is SecondaryWeapon)
            {
                player.weaponAdvanceUser._currentWeapon.AttachWeaponToSocketNoneAnimatorOverride(player.weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
                disarmedWeapon.AttatchWeaponToNoneOverrideAnimator(player.weaponAdvanceUser);
            }
            else
                throw new Exception("WeaponDisarm");
            curPhase = WeaponDisarmPhase.AfterDisarmed;
        }
       
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);

        if (elapesTime >= duration)
        {
            disarmedWeapon.AttatchWeaponTo(player.weaponAdvanceUser);
            this.isComplete = true;
        }
        
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return this.isComplete;
    }

    public override bool IsReset()
    {
        if(player.isDead)
            return  true;

        return IsComplete();
    }

    public override void UpdateNode()
    {
        elapesTime += Time.deltaTime;
        base.UpdateNode();
    }

    IEnumerator SlowMotion()
    {
        yield return new WaitForSecondsRealtime(duration*0.5f);

        Time.timeScale = 0.25f; // Set slow motion (25% speed)
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics for smoothness

        yield return new WaitForSecondsRealtime(1f); // Use unscaled time so the delay isn't affected by slow motion

        Time.timeScale = 1f; // Reset to normal speed
        Time.fixedDeltaTime = 0.02f; // Restore original physics update rate
    }
}
