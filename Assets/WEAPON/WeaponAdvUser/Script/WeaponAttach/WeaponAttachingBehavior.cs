using UnityEngine;
using UnityEngine.Animations;

public class WeaponAttachingBehavior
{
    public void Attach(Weapon weapon,IWeaponAttachingAble weaponAttachingAble)
    {
        switch (weaponAttachingAble)
        {
            case MainHandSocket mainHandSocket:
                {
                    //SetWeapon Property
                    weapon.isEquiped = true;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weaponAttachingAble.weaponAdvanceUser._currentWeapon = weapon;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;

                    //Set Parent Constraint
                    this.SetParentConstrain(weapon, mainHandSocket.weaponAttachingAbleTransform);

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
                    weapon.isEquiped = false;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;

                    ConstraintSource source = new ConstraintSource();
                    source.sourceTransform = secondHandSocket.weaponAttachingAbleTransform;
                    source.weight = 1;
                    if (weapon.parentConstraint.sourceCount > 0)
                    {
                        weapon.parentConstraint.RemoveSource(0);
                    }
                    weapon.parentConstraint.AddSource(source);

                    Vector3 translationOffset = weapon.transform.InverseTransformPoint(weapon._SecondHandGripTransform.position);
                    Quaternion rotationOffsetQuat = Quaternion.Inverse(weapon.transform.rotation) * weapon._SecondHandGripTransform.rotation;
                    Vector3 rotationOffset = rotationOffsetQuat.eulerAngles;

                    weapon.parentConstraint.SetTranslationOffset(0, -translationOffset);
                    weapon.parentConstraint.SetRotationOffset(0, rotationOffset);
                    weapon.parentConstraint.constraintActive = true;
                    weapon.parentConstraint.translationAtRest = Vector3.zero;
                    weapon.parentConstraint.rotationAtRest = Vector3.zero;

                    weapon.parentConstraint.constraintActive = true;
                    weapon.parentConstraint.weight = 1;
                    break; 
                }
            case PrimaryWeaponSocket primaryWeaponSocket: 
                {
                    weapon.isEquiped = false;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;
                    this.SetParentConstrain(weapon, primaryWeaponSocket.weaponAttachingAbleTransform);
                    if(primaryWeaponSocket.weaponAdvanceUser._currentWeapon == weapon)
                        primaryWeaponSocket.weaponAdvanceUser._currentWeapon = null;
                    break; 
                }
            case SecondaryWeaponSocket secondaryWeaponSocket: 
                {
                    weapon.isEquiped = false;
                    weapon.userWeapon = weaponAttachingAble.weaponAdvanceUser;
                    weapon.rb.isKinematic = true;
                    weapon._collider.isTrigger = true;
                    this.SetParentConstrain(weapon, secondaryWeaponSocket.weaponAttachingAbleTransform);
                    if(secondaryWeaponSocket.weaponAdvanceUser._currentWeapon == weapon)
                        secondaryWeaponSocket.weaponAdvanceUser._currentWeapon = null;
                    break; 
                }
        }
    }
    public void Detach(Weapon weapon, IWeaponAdvanceUser weaponAdvanceUser)
    {
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

    private void SetParentConstrain(Weapon weapon, Transform transform)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = transform;
        source.weight = 1;
        if (weapon.parentConstraint.sourceCount > 0)
        {
            weapon.parentConstraint.RemoveSource(0);
        }
        weapon.parentConstraint.AddSource(source);

        Vector3 translationOffset = weapon.transform.InverseTransformPoint(weapon._mainHandGripTransform.position);
        Quaternion rotationOffsetQuat = Quaternion.Inverse(weapon.transform.rotation) * weapon._mainHandGripTransform.rotation;
        Vector3 rotationOffset = rotationOffsetQuat.eulerAngles;

        weapon.parentConstraint.SetTranslationOffset(0, - translationOffset);
        weapon.parentConstraint.SetRotationOffset(0, rotationOffset);
        weapon.parentConstraint.constraintActive = true;
        weapon.parentConstraint.translationAtRest = Vector3.zero;
        weapon.parentConstraint.rotationAtRest = Vector3.zero;

        weapon.parentConstraint.constraintActive = true;
        weapon.parentConstraint.weight = 1;
    }
    private void SetAnimatorOverride(Weapon weapon,IWeaponAdvanceUser weaponAdvanceUser)
    {
        Animator animator = weaponAdvanceUser._weaponUserAnimator;
        AnimatorOverrideController animatorOverrideController = weaponAdvanceUser._animatorWeaponAdvanceUserOverride;
        WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject = weapon.weaponAnimationStateOverrideScriptableObject;

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animatorStateNormalizedTime = animatorStateInfo.normalizedTime;

        animatorOverrideController["Idle_LowReady_Overriden"] = weaponAnimationStateOverrideScriptableObject.idleLowReady;
        animatorOverrideController["Move_LowReady_Overriden"] = weaponAnimationStateOverrideScriptableObject.moveLowReady;
        animatorOverrideController["ADS_20_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_20;
        animatorOverrideController["C.A.R_20_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_20;
        animatorOverrideController["ADS_40_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_40;
        animatorOverrideController["C.A.R_40_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_40;
        animatorOverrideController["ADS_60_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_60;
        animatorOverrideController["C.A.R_60_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_60;
        animatorOverrideController["ADS_80_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_80;
        animatorOverrideController["C.A.R_80_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_80;
        animatorOverrideController["ADS_100_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_100;
        animatorOverrideController["C.A.R_100_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_100;
        animatorOverrideController["RecoildKickBack_ADS_Overriden"] = weaponAnimationStateOverrideScriptableObject.ADS_RecoilKick;
        animatorOverrideController["RecoildKickBack_C.A.R_Overriden"] = weaponAnimationStateOverrideScriptableObject.CAR_RecoilKick;

        animatorOverrideController["LowReady_Sprint_Sway_Overriden"] = weaponAnimationStateOverrideScriptableObject.TacticalSprint_LowReadySway;
        animatorOverrideController["HighReady_Sprint_Sway_Overriden"] = weaponAnimationStateOverrideScriptableObject.TacticalSprint_HighReadySway;

        //OverrideLocoMotion


        switch (weapon)
        {
            case MagazineType magazineType:
                {
                    animatorOverrideController["ReloadMagazineFull Override"] = magazineType.magazineWeaponAnimationStateOverrideScriptableObject.Reload;
                    animatorOverrideController["TacReloadFull Override"] = magazineType.magazineWeaponAnimationStateOverrideScriptableObject.TacticalReload;
                    break;
                }
        }

        animator.runtimeAnimatorController = animatorOverrideController;
        if (animator.IsInTransition(0))
        {
            animator.Play(animator.GetAnimatorTransitionInfo(0).fullPathHash, 0, animatorStateNormalizedTime);
        }
        else
        animator.Play(animatorStateInfo.fullPathHash, 0, animatorStateNormalizedTime);
    }
    private void SetWeaponAdvacneUserProperty(Weapon weapon, IWeaponAdvanceUser weaponAdvanceUser)
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
