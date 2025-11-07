using UnityEngine;
using UnityEngine.Animations;

public static class WeaponAttachingBehavior
{
    public static void Attach(Weapon weapon,IWeaponAttachingAble weaponAttachingAble)
    {
        Debug.Log("Attach");
        Attach(weapon,weaponAttachingAble,Vector3.zero,Quaternion.identity);
    }
    public static void Attach(
        Weapon weapon
        , IWeaponAttachingAble weaponAttachingAble
        ,Vector3 additionalOffsetPosition
        ,Quaternion additionalOffsetRotation)
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

                    weapon._weaponAttacherComponent.Attach(
                        mainHandSocket.weaponAttachingAbleTransform
                        ,weapon._mainHandGripTransform
                        ,additionalOffsetPosition
                        ,additionalOffsetRotation);

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

                    weapon._weaponAttacherComponent.Attach(
                        secondHandSocket.weaponAttachingAbleTransform
                        ,weapon._SecondHandGripTransform
                        ,additionalOffsetPosition
                        ,additionalOffsetRotation
                        );
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
                    weapon._weaponAttacherComponent.Attach(
                        primaryWeaponSocket.weaponAttachingAbleTransform
                        , weapon._mainHandGripTransform
                        ,additionalOffsetPosition
                        ,additionalOffsetRotation
                        );
                    if (primaryWeaponSocket.weaponAdvanceUser._currentWeapon == weapon)
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
                    weapon._weaponAttacherComponent.Attach(
                        secondaryWeaponSocket.weaponAttachingAbleTransform
                        ,weapon._mainHandGripTransform
                        ,additionalOffsetPosition
                        ,additionalOffsetRotation
                        );
                    if (secondaryWeaponSocket.weaponAdvanceUser._currentWeapon == weapon)
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
        
        weapon._weaponAttacherComponent.Detach();

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
