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
                        player.NotifyObserver(player,SubjectPlayer.PlayerAction.PickUpWeapon);
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
        disarmedWeapon.DropWeapon();
       
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
        //else
        //    throw new Exception("WeaponDisarm");
    }
    private Vector3 playerEnterPos;
    private void Pull(float t)
    {

        //Vector3 playerMovePos = attackedAbleGunFu._gunFuAttackedAble.position + (player.transform.position - attackedAbleGunFu._gunFuAttackedAble.position).normalized * 0.75f;

        //player.transform.position = Vector3.Lerp(
        //    playerEnterPos,
        //    playerMovePos,
        //    t);

        Vector3 opponentLook = (player.transform.position - attackedAbleGunFu._gunFuAttackedAble.position).normalized;
        opponentLook = new Vector3(opponentLook.x, 0, opponentLook.z);

        attackedAbleGunFu._gunFuAttackedAble.rotation = Quaternion.Lerp(
            attackedAbleGunFu._gunFuAttackedAble.rotation,
            Quaternion.LookRotation(opponentLook, Vector3.up),
            t);

        //Vector3 playerLookDir = (attackedAbleGunFu._gunFuAttackedAble.position - player.transform.position).normalized;
        //playerLookDir = new Vector3(playerLookDir.x, 0, playerLookDir.z);
        //Debug.DrawLine(player.transform.position, player.transform.position + playerLookDir, Color.red);

        //player.transform.rotation = Quaternion.Lerp(
        //    player.transform.rotation,
        //    Quaternion.LookRotation(playerLookDir, Vector3.up),
        //    t);

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


        player.NotifyObserver(player, SubjectPlayer.PlayerAction.GunFuInteract);
    }
}
