using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStage : WeaponManuverLeafNode/* ,IReloadMagazineNodePhase*/
{
    private MagazineType weaponMag => base.weaponAdvanceUser._currentWeapon as MagazineType;
    private float elaspeTime;
    private AmmoProuch ammoProuch => base.weaponAdvanceUser.weaponBelt.ammoProuch;

    //public IReloadMagazineNodePhase.ReloadMagazinePhase curReloadPhase { get; set; }
    public TacticalReloadMagazineFullStage(IWeaponAdvanceUser weaponUser, Func<bool> preCondition) : base(weaponUser, preCondition)
    {

    }
    public override void Enter()
    {
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.TacticalReloadMagazineFullStage);
        //curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Enter;
        Weapon.userWeapon.weaponAfterAction.SendFeedBackWeaponAfterAction
            <TacticalReloadMagazineFullStage>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);

        elaspeTime = 0;
        isComplete = false;
    }

    public override void Exit()
    {

        //curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Exit;
        Weapon.userWeapon.weaponAfterAction.SendFeedBackWeaponAfterAction
            <TacticalReloadMagazineFullStage>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

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
            weaponMag.Performed(weaponMag,ammoProuch);
            isComplete = true;
        }

    }
    
    
   
}
