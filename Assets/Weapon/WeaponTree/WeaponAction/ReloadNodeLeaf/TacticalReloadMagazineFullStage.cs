using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStage : WeaponLeafNode ,IReloadMagazineNodePhase
{
    private MagazineType weaponMag;
    private float elaspeTime;

    public IReloadMagazineNodePhase.ReloadMagazinePhase curReloadPhase { get; set; }
    public TacticalReloadMagazineFullStage(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
        weaponMag = weapon as MagazineType;
    }

   

    public override void Enter()
    {
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.TacticalReloadMagazineFullStage);
        curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Enter;
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon,this);

        elaspeTime = 0;
        isComplete = false;
    }

    public override void Exit()
    {

        curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Exit;
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, this);

        elaspeTime = 0;
        isComplete = false;
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
        if(IsComplete())
            return true;

        if(Weapon.isEquiped == false)
            return true;

        return false;
    }

    public override void UpdateNode()
    {

        float reloadTime = Weapon.reloadSpeed;
        elaspeTime += Time.deltaTime;

        if (elaspeTime >= reloadTime)
        {
            new AmmoProchReload(Weapon.userWeapon.weaponBelt.ammoProuch).Performed(Weapon);
            isComplete = true;
        }

    }
    
    
   
}
