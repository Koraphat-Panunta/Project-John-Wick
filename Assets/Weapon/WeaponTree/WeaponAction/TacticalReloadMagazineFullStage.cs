using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStage : WeaponActionNode
{
    private MagazineType weaponMag;
    private float elaspeTime;
    private bool isComplete;
    public TacticalReloadMagazineFullStage(Weapon weapon):base(weapon)
    {
        weaponMag = weapon as MagazineType;
    }



    public override void Enter()
    {
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.TacticalReloadMagazineFullStage);
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon,ReloadType.MAGAZINE_TACTICAL_RELOAD);
        elaspeTime = 0;
        isComplete = false;
    }

    public override void Exit()
    {
        elaspeTime = 0;
        isComplete = false;
     
    }

    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        if(weaponMag.isMagIn == true && Weapon.bulletStore[BulletStackType.Magazine] == Weapon.bulletCapacity)
            return true;
        else return false;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;
        else if(
            Weapon.isEquiped == false
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

        float reloadTime = Weapon.reloadSpeed;
        ReloadType reloadStage;
        elaspeTime += Time.deltaTime;

        if (elaspeTime >= reloadTime)
        {
            reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
            Weapon.userWeapon.weaponAfterAction.Reload(Weapon, reloadStage);
            new AmmoProchReload(Weapon.userWeapon.weaponBelt.ammoProuch).Performed(Weapon);
            isComplete = true;
        }

    }
    
    
   
}
