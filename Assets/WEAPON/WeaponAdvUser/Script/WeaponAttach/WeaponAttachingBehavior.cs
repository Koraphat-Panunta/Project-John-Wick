using UnityEngine;
using UnityEngine.Animations;

public static class WeaponAttachingBehavior
{
    public static void Attach(Weapon weapon,IWeaponAttachingAble weaponAttachingAble,float attatchingDuration)
    {
        Attach(weapon,weaponAttachingAble,Vector3.zero,Quaternion.identity,attatchingDuration);
    }
    public static void Attach(
        Weapon weapon
        , IWeaponAttachingAble weaponAttachingAble
        ,Vector3 additionalOffsetPosition
        ,Quaternion additionalOffsetRotation
        , float attatchingDuration)
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
                        ,additionalOffsetRotation
                        ,attatchingDuration);

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
                        ,attatchingDuration
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
                        ,attatchingDuration
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
                        ,attatchingDuration
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
