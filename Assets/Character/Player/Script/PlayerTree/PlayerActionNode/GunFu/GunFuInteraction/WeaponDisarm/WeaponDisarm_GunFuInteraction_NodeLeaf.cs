using System;
using System.Collections;
using UnityEngine;

public class WeaponDisarm_GunFuInteraction_NodeLeaf : PlayerGunFu_Interaction_NodeLeaf
{

    public Weapon disarmedWeapon;

    private AnimationInteractScriptableObject animationInteractScriptableObject;

    public AnimationTriggerEventPlayer animationTriggerEventPlayer;

    private SubjectAnimationInteract subject_Disarmer;
    private SubjectAnimationInteract subject_Disarmed;

    public WeaponDisarm_GunFuInteraction_NodeLeaf(
        AnimationInteractScriptableObject animationInteractScriptableObject
        ,Player player
        , Func<bool> preCondition) : base(player, preCondition)
    {
        this.animationInteractScriptableObject = animationInteractScriptableObject;

        this.subject_Disarmer = new SubjectAnimationInteract(animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.animationInteractCharacterDetail[0]);
        this.subject_Disarmer.finishWarpEvent += this.Interact;

        this.subject_Disarmed = new SubjectAnimationInteract(animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.animationInteractCharacterDetail[1]);

        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationInteractScriptableObject.clip
            , animationInteractScriptableObject.enterNormalizedTime
            , animationInteractScriptableObject.endNormalizedTime
            , animationInteractScriptableObject.triggerEventDetail);

        this.animationTriggerEventPlayer.SubscribeEvent("Disarm",Disarm);
        this.animationTriggerEventPlayer.SubscribeEvent("TransitionAble", TransitionAble);
    }

    public override void Enter()
    {
        isComplete = false;
        gotGunFuAttackedAble = player.attackedAbleGunFu;
        disarmedWeapon = gotGunFuAttackedAble._weaponAdvanceUser._currentWeapon;
        this.subject_Disarmer.RestartSubject(gunFuAble._character, gotGunFuAttackedAble._character.transform.position, gotGunFuAttackedAble._character.transform.forward);
        this.subject_Disarmed.RestartSubject(gotGunFuAttackedAble._character, gotGunFuAttackedAble._character.transform.position, gotGunFuAttackedAble._character.transform.forward);
        this.animationTriggerEventPlayer.Rewind();
        player._movementCompoent.CancleMomentum();
        this.gotGunFuAttackedAble._character._movementCompoent.CancleMomentum();
        this.HoslterCurrentWeapon();
        base.Enter();
    }

    public override void Exit()
    {
        gotGunFuAttackedAble = null;
        gunFuAble._character.enableRootMotion = false;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {


        base.FixedUpdateNode();
    }

   
    
    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }

    public override bool IsReset()
    {
        if(player.isDead)
            return  true;

        if(player._triggerHitedGunFu)
            return true;

        return IsComplete();
    }

    public override void UpdateNode()
    {
        animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        subject_Disarmer.UpdateInteract(Time.deltaTime);
        subject_Disarmed.UpdateInteract(Time.deltaTime);
        base.UpdateNode();
    }

    private void HoslterCurrentWeapon()
    {
        if (player._currentWeapon != null)
        {
            if (player._currentWeapon == player._weaponBelt.myPrimaryWeapon as Weapon)
                WeaponAttachingBehavior.Attach(player._currentWeapon, player._weaponBelt.primaryWeaponSocket);
            else if (player._currentWeapon == player._weaponBelt.mySecondaryWeapon as Weapon)
                WeaponAttachingBehavior.Attach(player._currentWeapon, player._weaponBelt.secondaryWeaponSocket);
        }
    }
    private void Interact(Character character)
    {
        gunFuAble._character.enableRootMotion = true;
        gotGunFuAttackedAble.TakeGunFuAttacked(this, player);
    }
    private void Disarm()
    {



        WeaponAttachingBehavior.Detach(disarmedWeapon, disarmedWeapon.userWeapon);

        if (disarmedWeapon is PrimaryWeapon && player._weaponBelt.myPrimaryWeapon != null)
            WeaponAttachingBehavior.Detach(player._weaponBelt.myPrimaryWeapon as Weapon, player);

        if (disarmedWeapon is SecondaryWeapon && player._weaponBelt.mySecondaryWeapon != null)
            WeaponAttachingBehavior.Detach(player._weaponBelt.mySecondaryWeapon as Weapon, player);

       

        WeaponAttachingBehavior.Attach(disarmedWeapon, player._mainHandSocket);

        Debug.Log("Character : " + gunFuAble._character + " Disarm anchor Distance pos = "
       + Vector3.Distance(
                  gunFuAble._character.transform.position
                 , subject_Disarmer.anhorPosition)
             );
        Debug.Log("Character : " + gunFuAble._character + " Disarm anchor Distance rot = "
        + Quaternion.Angle(
             gunFuAble._character.transform.rotation
            , Quaternion.LookRotation(subject_Disarmer.anhorDir))
        );

        Debug.Log("Character : " + gotGunFuAttackedAble._character + " Disarm anchor Distance pos = "
        + Vector3.Distance(
                   gotGunFuAttackedAble._character.transform.position
                  , subject_Disarmed.anhorPosition)
              );
        Debug.Log("Character : " + gotGunFuAttackedAble._character + " Disarm anchor Distance rot = "
        + Quaternion.Angle(
             gotGunFuAttackedAble._character.transform.rotation
            , Quaternion.LookRotation(subject_Disarmed.anhorDir))
        );
        //else
        //    throw new Exception("WeaponDisarm");
    }
    private void TransitionAble() => nodeLeafTransitionBehavior.TransitionAbleAll(this);

}
