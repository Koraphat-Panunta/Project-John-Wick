using System;
using System.Collections;
using UnityEngine;

public class WeaponDisarm_GunFuInteraction_NodeLeaf : PlayerGunFu_Interaction_NodeLeaf
{
    private float pullTime => weaponDisarmGunFuScriptableObject.animationClip.length 
        * weaponDisarmGunFuScriptableObject.pullTimeNormalized;
    private float disarmTime => weaponDisarmGunFuScriptableObject.animationClip.length 
        * weaponDisarmGunFuScriptableObject.disarmTimeNormalized;
    private float transitionAbleTime => weaponDisarmGunFuScriptableObject.animationClip.length 
        * weaponDisarmGunFuScriptableObject.transitionAbleTimeNormalized;
    private float duration => weaponDisarmGunFuScriptableObject.animationClip.length 
        * weaponDisarmGunFuScriptableObject.exitTimeNormalized;

    private WeaponDisarmGunFuScriptableObject weaponDisarmGunFuScriptableObject;

    private float elapesTime;

    public Weapon disarmedWeapon;

    private bool isDisarmWeapon;
    private bool isTransitionAbleAlready;

    public enum WeaponDisarmPhase
    {
        None,
        Pulling,
        Disarming
    }

    public WeaponDisarmPhase curPhase { get; private set; }

    public WeaponDisarm_GunFuInteraction_NodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        
    }

    public override void Enter()
    {
        isComplete = true;
        attackedAbleGunFu = player.attackedAbleGunFu;
        curPhase = WeaponDisarmPhase.Pulling;
        elapesTime = 0;
        disarmedWeapon = attackedAbleGunFu._weaponAdvanceUser._currentWeapon;
        attackedAbleGunFu.TakeGunFuAttacked(this, player);
        isDisarmWeapon = false;
        isTransitionAbleAlready = false;

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
        switch (curPhase)
        {
            case WeaponDisarmPhase.Pulling:
                {
                    if(elapesTime >= pullTime)
                    {
                        Pull(elapesTime / pullTime);
                        curPhase = WeaponDisarmPhase.Disarming;
                    }
                }
                break;
            case WeaponDisarmPhase.Disarming: 
                { 
                    if(isDisarmWeapon == false 
                        && elapesTime >= disarmTime)
                    {
                        Disarm();
                        isDisarmWeapon = true;
                    }
                    if (isTransitionAbleAlready == false &&
                        elapesTime >= transitionAbleTime)
                    {
                        nodeLeafTransitionBehavior.TransitionAbleAll(this);
                        isTransitionAbleAlready = true;
                    }
                    if(elapesTime >= duration)
                        isComplete = true;
                        
                }
                break;
        }

        //if(elapesTime <= pull)
        //{
        //    curPhase = WeaponDisarmPhase.Pulling;
        //    Pull(elapesTime / this.pull);
        //}
        
        //if(elapesTime > pull && isDisarmWeapon == false)
        //{
        //    curPhase = WeaponDisarmPhase.Disarming;
        //    Disarm();
        //    isDisarmWeapon = true;
        //    player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
        //}
       
        //player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);

        //if (elapesTime >= duration)
        //{
        //    this.isComplete = true;
        //}
        
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
        float originalFixdeltaTime = Time.fixedDeltaTime;

        yield return new WaitForSecondsRealtime(duration*0.5f);

        Time.timeScale = 0.25f; // Set slow motion (25% speed)
        Time.fixedDeltaTime *= Time.timeScale; // Adjust physics for smoothness

        yield return new WaitForSecondsRealtime(1f); // Use unscaled time so the delay isn't affected by slow motion

        Time.timeScale = 1f; // Reset to normal speed
        Time.fixedDeltaTime = originalFixdeltaTime; // Restore original physics update rate
    }
    private void Disarm()
    {
        disarmedWeapon.DropWeapon();
       

        (attackedAbleGunFu._movementCompoent as IMotionImplusePushAble).AddForcePush(player.transform.forward * 2.5f, IMotionImplusePushAble.PushMode.InstanlyIgnoreMomentum);
        if (player.weaponAdvanceUser._currentWeapon == null)
        {
            disarmedWeapon.AttatchWeaponTo(player.weaponAdvanceUser);
        }
        else if (player.weaponAdvanceUser._currentWeapon != player.weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon
            && player.weaponAdvanceUser._currentWeapon != player.weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon)
        {
            player.weaponAdvanceUser._currentWeapon.DropWeapon();
            disarmedWeapon.AttatchWeaponTo(player.weaponAdvanceUser);
        }
        else if (player.weaponAdvanceUser._currentWeapon != null
            && player.weaponAdvanceUser._currentWeapon is PrimaryWeapon)
        {
            player.weaponAdvanceUser._currentWeapon.AttachWeaponToSocketNoneAnimatorOverride(player.weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
            disarmedWeapon.AttatchWeaponTo(player.weaponAdvanceUser);

        }
        else if (player.weaponAdvanceUser._currentWeapon != null
            && player.weaponAdvanceUser._currentWeapon is SecondaryWeapon)
        {
            player.weaponAdvanceUser._currentWeapon.AttachWeaponToSocketNoneAnimatorOverride(player.weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);
            disarmedWeapon.AttatchWeaponTo(player.weaponAdvanceUser);
        }
        else
            throw new Exception("WeaponDisarm");
    }
    private void Pull(float t)
    {
        //attackedAbleGunFu._gunFuAttackedAble.position = Vector3.Lerp(
        //               attackedAbleGunFu._gunFuAttackedAble.position,
        //               targetAdjustTransform.position,
        //               t
        //               );
        player.transform.position = Vector3.Lerp(
            player.transform.position,
            Vector3.MoveTowards(attackedAbleGunFu.attackedPos, player.transform.position, 1),
            t);

        Vector3 playerLookDir = (attackedAbleGunFu._gunFuAttackedAble.position - player.transform.position).normalized;
        playerLookDir = new Vector3(playerLookDir.x, 0, playerLookDir.z);

        player.transform.rotation = Quaternion.Lerp(
            player.transform.rotation,
            Quaternion.LookRotation(playerLookDir, Vector3.up),
            t);

        attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(
            attackedAbleGunFu._gunFuAttackedAble.rotation, 
            Quaternion.LookRotation(targetAdjustTransform.forward * -1, Vector3.up),
            t);

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
    }
}
