using UnityEngine;
using UnityEngine.Animations;

public static class WeaponAttachingBehavior
{
    public static void Attach(Weapon weapon,IWeaponAttachingAble weaponAttachingAble)
    {
        switch (weaponAttachingAble)
        {
            case MainHandSocket mainHandSocket:
                {
                    //Detach form other weaponsocket
                    if (weapon.userWeapon != null)
                    {
                        if (weapon.userWeapon._secondHandSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._secondHandSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket = null;
                    }

                    mainHandSocket.curWeaponAtSocket = weapon;

                    //SetWeapon Property
                    weapon.isEquiped = true;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weaponAttachingAble.weaponAdvanceUser._currentWeapon = weapon;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;

                    //Set Parent Constraint
                    SetParentConstrain(weapon, mainHandSocket.weaponAttachingAbleTransform,weapon._mainHandGripTransform);

                    //Set AnimationState Override
                    AnimatorOverrideController animatorOverrideController = weaponAttachingAble.weaponAdvanceUser._animatorWeaponAdvanceUserOverride;
                    SetAnimatorOverride(weapon, weaponAttachingAble.weaponAdvanceUser);

                    //Set reloadNodeAttachAble
                    weaponAttachingAble.weaponAdvanceUser._weaponManuverManager.reloadNodeAttachAbleSelector.AddtoChildNode(weapon._reloadSelecotrOverriden);

                    SetWeaponAdvacneUserProperty(weapon, weaponAttachingAble.weaponAdvanceUser);
                    break;
                }
            case SecondHandSocket secondHandSocket: 
                {
                    //Detach form other weaponsocket
                    if (weapon.userWeapon != null)
                    {
                        if (weapon.userWeapon._mainHandSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._mainHandSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket = null;
                    }
                    secondHandSocket.curWeaponAtSocket = weapon;

                    weapon.isEquiped = false;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;

                    ParentConstraintAttachBehavior.Attach(weapon.parentConstraint, secondHandSocket.weaponAttachingAbleTransform
                        ,weapon._SecondHandGripTransform.localPosition,weapon._SecondHandGripTransform.localRotation);
                    break; 
                }
            case PrimaryWeaponSocket primaryWeaponSocket: 
                {
                    //Detach form other weaponsocket
                    if (weapon.userWeapon != null)
                    {
                        if (weapon.userWeapon._mainHandSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._mainHandSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._secondHandSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._secondHandSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket = null;
                    }

                    primaryWeaponSocket.curWeaponAtSocket = weapon;

                    weapon.isEquiped = false;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;
                    SetParentConstrain(weapon, primaryWeaponSocket.weaponAttachingAbleTransform,weapon._mainHandGripTransform);
                    if(primaryWeaponSocket.weaponAdvanceUser._currentWeapon == weapon)
                        primaryWeaponSocket.weaponAdvanceUser._currentWeapon = null;
                    break; 
                }
            case SecondaryWeaponSocket secondaryWeaponSocket: 
                {
                    //Detach form other weaponsocket
                    if (weapon.userWeapon != null)
                    {
                        if (weapon.userWeapon._mainHandSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._mainHandSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._secondHandSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._secondHandSocket.curWeaponAtSocket = null;
                        else if (weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket == weapon)
                            weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket = null;
                    }

                    secondaryWeaponSocket.curWeaponAtSocket = weapon;

                    weapon.isEquiped = false;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;
                    SetParentConstrain(weapon, secondaryWeaponSocket.weaponAttachingAbleTransform, weapon._mainHandGripTransform);
                    if(secondaryWeaponSocket.weaponAdvanceUser._currentWeapon == weapon)
                        secondaryWeaponSocket.weaponAdvanceUser._currentWeapon = null;
                    break; 
                }
        }
    }
    public static void Detach(Weapon weapon, IWeaponAdvanceUser weaponAdvanceUser)
    {
        if (weapon.userWeapon != null)
        {
            if (weapon.userWeapon._mainHandSocket.curWeaponAtSocket == weapon)
                weapon.userWeapon._mainHandSocket.curWeaponAtSocket = null;
            else if (weapon.userWeapon._secondHandSocket.curWeaponAtSocket == weapon)
                weapon.userWeapon._secondHandSocket.curWeaponAtSocket = null;
            else if (weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket == weapon)
                weapon.userWeapon._weaponBelt.primaryWeaponSocket.curWeaponAtSocket = null;
            else if(weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket == weapon)
                weapon.userWeapon._weaponBelt.secondaryWeaponSocket.curWeaponAtSocket= null;
        }

        weapon.isEquiped = false;
        weapon.rb.isKinematic = false;
        weapon._collider.isTrigger = false;
        if (weapon.parentConstraint.sourceCount > 0)
        {
            weapon.parentConstraint.RemoveSource(0);
            weapon.parentConstraint.constraintActive = true;
            weapon.parentConstraint.constraintActive = true;
            weapon.parentConstraint.weight = 1;
        }
        if (weaponAdvanceUser._currentWeapon == weapon)
        {
            weaponAdvanceUser._weaponManuverManager.reloadNodeAttachAbleSelector.RemoveNode(weapon._reloadSelecotrOverriden);
            weaponAdvanceUser._currentWeapon = null;
        }
        if (weapon is PrimaryWeapon)
        {
            if (weaponAdvanceUser._weaponBelt.myPrimaryWeapon == weapon as PrimaryWeapon)
                weaponAdvanceUser._weaponBelt.myPrimaryWeapon = null;
        }
        else if (weapon is SecondaryWeapon)
        {
            if (weaponAdvanceUser._weaponBelt.mySecondaryWeapon == weapon as SecondaryWeapon)
                weaponAdvanceUser._weaponBelt.mySecondaryWeapon = null;
        }

       weapon.userWeapon = null;

    }
    private static void SetParentConstrain(Weapon weapon, Transform transform,Transform weaponGrip)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = transform;
        source.weight = 1;
        if (weapon.parentConstraint.sourceCount > 0)
        {
            weapon.parentConstraint.RemoveSource(0);
        }
        weapon.parentConstraint.AddSource(source);

        Vector3 translationOffset = weapon.transform.InverseTransformPoint(weaponGrip.position);
        Quaternion rotationOffsetQuat = Quaternion.Inverse(weapon.transform.rotation) * weaponGrip.rotation;
        Vector3 rotationOffset = rotationOffsetQuat.eulerAngles;

        weapon.parentConstraint.SetRotationOffset(0, rotationOffset);
        weapon.parentConstraint.SetTranslationOffset(0, - translationOffset);

        //weapon.parentConstraint.constraintActive = true;
        weapon.parentConstraint.translationAtRest = Vector3.zero;
        weapon.parentConstraint.rotationAtRest = Vector3.zero;

        weapon.parentConstraint.constraintActive = true;
        weapon.parentConstraint.weight = 1;
    }
    private static void SetAnimatorOverride(Weapon weapon,IWeaponAdvanceUser weaponAdvanceUser)
    {
        Animator animator = weaponAdvanceUser._weaponUserAnimator;

        AnimatorOverrideController overrideController = new AnimatorOverrideController(weaponAdvanceUser._animatorWeaponAdvanceUserOverride);
        WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject = weapon.weaponAnimationStateOverrideScriptableObject;

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animatorStateNormalizedTime = animatorStateInfo.normalizedTime;

        overrideController["Idle_LowReady_Overriden"] = weaponAnimationStateOverrideScriptableObject.idleLowReady;
        overrideController["Move_LowReady_Overriden"] = weaponAnimationStateOverrideScriptableObject.moveLowReady;
        overrideController["ADS_20_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_20;
        overrideController["C.A.R_20_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_20;
        overrideController["ADS_40_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_40;
        overrideController["C.A.R_40_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_40;
        overrideController["ADS_60_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_60;
        overrideController["C.A.R_60_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_60;
        overrideController["ADS_80_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_80;
        overrideController["C.A.R_80_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_80;
        overrideController["ADS_100_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_100;
        overrideController["C.A.R_100_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_100;
        overrideController["RecoildKickBack_ADS_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_RecoilKick;
        overrideController["RecoildKickBack_C.A.R_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_RecoilKick;

        overrideController["LowReady_Sprint_Sway_Overriden"] = weaponAnimationStateOverrideScriptableObject.TacticalSprint_LowReadySway;
        overrideController["HighReady_Sprint_Sway_Overriden"] = weaponAnimationStateOverrideScriptableObject.TacticalSprint_HighReadySway;

        //OverrideLocoMotion


        switch (weapon)
        {
            case MagazineType magazineType:
                {
                    overrideController["ReloadMagazineFull Override"] = magazineType.magazineWeaponAnimationStateOverrideScriptableObject.Reload;
                    overrideController["TacReloadFull Override"] = magazineType.magazineWeaponAnimationStateOverrideScriptableObject.TacticalReload;
                    break;
                }
        }

        animator.runtimeAnimatorController = overrideController;
        if (animator.IsInTransition(0))
        {
            animator.Play(animator.GetAnimatorTransitionInfo(0).fullPathHash, 0, animatorStateNormalizedTime);
        }
        else
        animator.Play(animatorStateInfo.fullPathHash, 0, animatorStateNormalizedTime);
    }
    private static void SetWeaponAdvacneUserProperty(Weapon weapon, IWeaponAdvanceUser weaponAdvanceUser)
    {
        if (weapon is PrimaryWeapon)
        {
            if (weaponAdvanceUser._weaponBelt.myPrimaryWeapon == null)
                weaponAdvanceUser._weaponBelt.myPrimaryWeapon = weapon as PrimaryWeapon;
        }
        else if (weapon is SecondaryWeapon)
        {
            if (weaponAdvanceUser._weaponBelt.mySecondaryWeapon == null)
                weaponAdvanceUser._weaponBelt.mySecondaryWeapon = weapon as SecondaryWeapon;
        }
    }
    
}
