using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStage : WeaponLeafNode,IReloadMagazineNodePhase
{
    private float reloadTime;

    private MagazineType mag;

    private float elaspeTime;

    public IReloadMagazineNodePhase.ReloadMagazinePhase curReloadPhase { get ; set ; }

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
        return base.IsComplete();
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
    }

    public override void Enter()
    {

        isComplete = false;

        curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Enter;

        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.ReloadMagazineFullStage);
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, this);

        elaspeTime = 0;
    }
    private void Reloading()
    {
        new AmmoProchReload(Weapon.userWeapon.weaponBelt.ammoProuch).Performed(Weapon);
        Weapon.bulletStore[BulletStackType.Chamber] += 1;
        Weapon.bulletStore[BulletStackType.Magazine] -= 1;
        isComplete = true;

    }
    public override void Exit()
    {
        isComplete = false;

        curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Exit;
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, this);

        elaspeTime = 0;
    }
}
