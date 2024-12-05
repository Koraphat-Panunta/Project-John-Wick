using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStage : WeaponActionNode
{
    private MagazineType weaponMag;
    public TacticalReloadMagazineFullStage(Weapon weapon):base(weapon)
    {
        weaponMag = weapon as MagazineType;
    }



    public override void Enter()
    {
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.TacticalReload);
        Weapon.userWeapon.weaponAfterAction.Tactical_ReloadMagazine(Weapon);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        if(
            weaponMag.isMagIn == true
            && Weapon.bulletStore[BulletStackType.Magazine] == Weapon.Magazine_capacity
            )
            return true;
        else return false;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;
        else if(
            Weapon.isEquip == false
            ||Weapon.isCancelAction == true
            )
            return true;
        else return false ;
    }

    public override bool PreCondition()
    {
        bool IsMagIn = weaponMag.isMagIn;
        int MagCount = Weapon.bulletStore[BulletStackType.Magazine];
        if (
            IsMagIn == true
            && MagCount >= 0
            )
            return true;
        else 
            return false;
    }

    public override void Update()
    {
        
    }

   
}
