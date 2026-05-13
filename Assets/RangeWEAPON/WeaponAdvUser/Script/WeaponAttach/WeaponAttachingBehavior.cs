using UnityEngine;
using UnityEngine.Animations;

public static class WeaponAttachingBehavior
{
    public static void Attach(RangeWeapon weapon,IGrabRangeWeaponAble weaponAttachingAble,float attatchingDuration)
    {
        Attach(weapon,weaponAttachingAble,Vector3.zero,Quaternion.identity,attatchingDuration);
    }
    public static void Attach(
        RangeWeapon weapon
        , IGrabRangeWeaponAble weaponAttachingAble
        ,Vector3 additionalOffsetPosition
        ,Quaternion additionalOffsetRotation
        , float attatchingDuration)
    {

        //Register 
        if (weapon is PrimaryWeapon)
        {
            if (weaponAttachingAble.weaponAdvanceUser._weaponBelt.myPrimaryWeapon == null)
                weaponAttachingAble.weaponAdvanceUser._weaponBelt.myPrimaryWeapon = weapon as PrimaryWeapon;
        }
        else if (weapon is SecondaryWeapon)
        {
            if (weaponAttachingAble.weaponAdvanceUser._weaponBelt.mySecondaryWeapon == null)
                weaponAttachingAble.weaponAdvanceUser._weaponBelt.mySecondaryWeapon = weapon as SecondaryWeapon;
        }

        if (weapon.curAttatch != null)
            weapon.curAttatch.GrabDetach();

        weaponAttachingAble.GrabAttach(weapon,additionalOffsetPosition,additionalOffsetRotation,attatchingDuration);

       
    }
    public static void Attach(
       MeleeWeapon weapon
       , IGrabMeleeWeaponAble weaponAttachingAble
       , Vector3 additionalOffsetPosition
       , Quaternion additionalOffsetRotation
       , float attatchingDuration)
    {

        if (weapon._currentGrabbedAt != null)
            weapon._currentGrabbedAt.GrabDetach();

        weaponAttachingAble.GrabAttach(weapon, additionalOffsetPosition, additionalOffsetRotation, attatchingDuration);

    }
    public static void Attach(
       MeleeWeapon weapon
       , IGrabMeleeWeaponAble weaponAttachingAble
       , float attatchingDuration) => Attach(weapon, weaponAttachingAble, Vector3.zero, Quaternion.identity, attatchingDuration);
   
    public static void Detach(RangeWeapon weapon, IRangeWeaponAdvanceUser weaponAdvanceUser)
    {
        if(weapon.curAttatch != null)
            weapon.curAttatch.GrabDetach();

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

    }

    public static void Attach(Weapon weapon
       , IGrabAbleObject weaponAttachingAble
       , Vector3 additionalOffsetPosition
       , Quaternion additionalOffsetRotation
       , float attatchingDuration)
    {
        switch (weapon)
        {
            case RangeWeapon rangeWeapon when weaponAttachingAble is IGrabRangeWeaponAble rangeSocket:
                Attach(rangeWeapon, rangeSocket, additionalOffsetPosition, additionalOffsetRotation, attatchingDuration);
                break;
            case MeleeWeapon meleeWeapon when weaponAttachingAble is IGrabMeleeWeaponAble meleeSocket:
                Attach(meleeWeapon, meleeSocket, additionalOffsetPosition, additionalOffsetRotation, attatchingDuration);
                break;
            default:
                UnityEngine.Debug.LogWarning($"WeaponAttachingBehavior: socket/weapon type mismatch — {weapon?.GetType().Name}");
                break;
        }
    }
    public static void Attach(Weapon weapon
       , IGrabAbleObject weaponAttachingAble
       , float attatchingDuration)
    {
        Attach(weapon, weaponAttachingAble, Vector3.zero, Quaternion.identity, attatchingDuration);
    }

}
