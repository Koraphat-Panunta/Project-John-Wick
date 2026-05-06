using UnityEngine;
using UnityEngine.Animations;

public static class WeaponAttachingBehavior
{
    public static void Attach(Weapon weapon,IGrabWeaponAble weaponAttachingAble,float attatchingDuration)
    {
        Attach(weapon,weaponAttachingAble,Vector3.zero,Quaternion.identity,attatchingDuration);
    }
    public static void Attach(
        Weapon weapon
        , IGrabWeaponAble weaponAttachingAble
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
            weapon.curAttatch.Detach();

        

        weaponAttachingAble.Attatch(weapon,additionalOffsetPosition,additionalOffsetRotation,attatchingDuration);

       
    }
    public static void Detach(Weapon weapon, IWeaponAdvanceUser weaponAdvanceUser)
    {
        if(weapon.curAttatch != null)
            weapon.curAttatch.Detach();

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
   
 
    
}
