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

    public WeaponDisarm_GunFuInteraction_NodeLeaf(WeaponDisarmGunFuScriptableObject weaponDisarmGunFuScriptableObject,Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        this.weaponDisarmGunFuScriptableObject = weaponDisarmGunFuScriptableObject;
    }

    public override void Enter()
    {
        isComplete = true;
        attackedAbleGunFu = player.attackedAbleGunFu;
        curPhase = WeaponDisarmPhase.Pulling;
        elapesTime = 0;
        disarmedWeapon = attackedAbleGunFu._weaponAdvanceUser._currentWeapon;
        isDisarmWeapon = false;
        isTransitionAbleAlready = false;
        playerEnterPos = player.transform.position;
        player._movementCompoent.CancleMomentum();

        base.Enter();
    }

    public override void Exit()
    {
        curPhase = WeaponDisarmPhase.None;
        attackedAbleGunFu = null;
        player.animator.applyRootMotion = false;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        switch (curPhase)
        {
            case WeaponDisarmPhase.Pulling:
                {
                    Pull(elapesTime / pullTime);
                    Debug.Log("t disarm = " + elapesTime / pullTime);
                    if (elapesTime >= pullTime)
                    {
                        attackedAbleGunFu.TakeGunFuAttacked(this, player);
                        player.animator.applyRootMotion = true;
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
                        player.NotifyObserver(player,SubjectPlayer.NotifyEvent.PickUpWeapon);
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

    private void Disarm()
    {
        WeaponAttachingBehavior weaponAttachingBehavior = new WeaponAttachingBehavior();

        weaponAttachingBehavior.Detach(disarmedWeapon, disarmedWeapon.userWeapon);
       
        if (player.weaponAdvanceUser._currentWeapon == null)
        {
            weaponAttachingBehavior.Attach(disarmedWeapon, player._mainHandSocket);
        }
        else if (player.weaponAdvanceUser._currentWeapon != player.weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
            && player.weaponAdvanceUser._currentWeapon != player.weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
        {
            weaponAttachingBehavior.Detach(player._currentWeapon, player);
            weaponAttachingBehavior.Attach(disarmedWeapon, player._mainHandSocket);
        }
        else if (player.weaponAdvanceUser._currentWeapon != null
            && player.weaponAdvanceUser._currentWeapon is PrimaryWeapon)
        {
            weaponAttachingBehavior.Attach(player._currentWeapon, player._weaponBelt.primaryWeaponSocket);
            weaponAttachingBehavior.Attach(disarmedWeapon, player._mainHandSocket);

        }
        else if (player.weaponAdvanceUser._currentWeapon != null
            && player.weaponAdvanceUser._currentWeapon is SecondaryWeapon)
        {
            weaponAttachingBehavior.Attach(player._currentWeapon, player._weaponBelt.secondaryWeaponSocket);
            weaponAttachingBehavior.Attach(disarmedWeapon, player._mainHandSocket);
        }
        //else
        //    throw new Exception("WeaponDisarm");
    }
    private Vector3 playerEnterPos;
    private void Pull(float t)
    {


        Vector3 opponentLook = (player.transform.position - attackedAbleGunFu._gunFuAttackedAble.position).normalized;
        opponentLook = new Vector3(opponentLook.x, 0, opponentLook.z);

        attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(
            attackedAbleGunFu._gunFuAttackedAble.rotation,
            Quaternion.LookRotation(opponentLook, Vector3.up),
            t);

        attackedAbleGunFu._movementCompoent.CancleMomentum();

        Vector3 opponentMovePos = targetAdjustTransform.position 
            + targetAdjustTransform.forward * weaponDisarmGunFuScriptableObject.OffsetTargerAdjust.z
            + targetAdjustTransform.right * weaponDisarmGunFuScriptableObject.OffsetTargerAdjust.x
            + targetAdjustTransform.up * weaponDisarmGunFuScriptableObject.OffsetTargerAdjust.y;

        attackedAbleGunFu._gunFuAttackedAble.position = Vector3.Lerp(
                       attackedAbleGunFu._gunFuAttackedAble.position,
                      opponentMovePos,
                       t
                       );


        player.NotifyObserver(player, SubjectPlayer.NotifyEvent.GunFuInteract);
    }
}
