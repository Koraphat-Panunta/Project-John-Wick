using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStage : WeaponLeafNode
{
    private float reloadTime;

    private ReloadType reloadStage;
    private MagazineType mag;

    private float elaspeTime;

    public ReloadMagazineFullStage(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
        this.reloadTime = weapon.reloadSpeed;
        mag = weapon as MagazineType;
    }

    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        if(reloadStage == ReloadType.MAGAZINE_RELOAD_SUCCESS)
        {
            return true;
        }
        else
            return false;   

    }
    public override bool IsReset()
    {
        if (IsComplete())
            return true;
        else if (Weapon.isEquiped == false)
        {
            return true;
        } 
        else return false;
    }

    //public override bool Precondition()
    //{
    //    int chamberCount = Weapon.bulletStore[BulletStackType.Chamber];
    //    int magCount = Weapon.bulletStore[BulletStackType.Magazine];
    //    bool isMagIn = mag.isMagIn;
       
    //    if
    //        (
    //         isMagIn == true 
    //        && chamberCount ==0
    //        && magCount == 0
    //        )
    //        return true;
    //    else
    //        return false;
    //}

    public override void UpdateNode()
    {
        int chamberCount = Weapon.bulletStore[BulletStackType.Chamber];
        int magCount = Weapon.bulletStore[BulletStackType.Magazine];
        bool isMagIn = mag.isMagIn;

        elaspeTime += Time.deltaTime;
        if(elaspeTime >= reloadTime)
        {
            Reloading();
        }

        if (
            chamberCount != 0
            && magCount != 0
            && isMagIn == true
            )
            reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
    }

    public override void Enter()
    {
        reloadStage = ReloadType.MAGAZINE_RELOAD;
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.ReloadMagazineFullStage);
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, reloadStage);

        elaspeTime = 0;
    }
    private void Reloading()
    {
        reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, reloadStage);
        //Weapon.bulletStore[BulletStackType.Magazine] = Weapon.userWeapon.weaponBelt.ammoProuch.
        new AmmoProchReload(Weapon.userWeapon.weaponBelt.ammoProuch).Performed(Weapon);
        Weapon.bulletStore[BulletStackType.Chamber] += 1;
        Weapon.bulletStore[BulletStackType.Magazine] -= 1;
    }
    public override void Exit()
    {
        elaspeTime = 0;
    }
}
