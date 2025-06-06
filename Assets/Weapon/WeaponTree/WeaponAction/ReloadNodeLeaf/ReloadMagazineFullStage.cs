using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStage : WeaponManuverLeafNode,IReloadMagazineNode/*,IReloadMagazineNodePhase*/
{
    public enum ReloadStage
    {
        Enter,
        Reloading,

        Cancel
    }
    private bool isComplete;
    public ReloadStage curReloadStage { get; private set; }

    private float reloadTime => weaponAdvanceUser._currentWeapon.reloadSpeed;

    private MagazineType weaponMag => weaponAdvanceUser._currentWeapon as MagazineType;

    private float elaspeTime;

    private AmmoProuch ammoProuch => weaponAdvanceUser.weaponBelt.ammoProuch;

    //public IReloadMagazineNodePhase.ReloadMagazinePhase curReloadPhase { get ; set ; }

    public ReloadMagazineFullStage(IWeaponAdvanceUser weaponUser, Func<bool> preCondition) : base(weaponUser, preCondition)
    {

    }

    public override void FixedUpdateNode()
    {
        
    }
    public override bool IsComplete()
    {
        return this.isComplete;
    }
    public override bool IsReset()
    {
        if (IsComplete())
            return true;
        if()
    }

    public override void UpdateNode()
    {

        elaspeTime += Time.deltaTime;
        if(elaspeTime >= reloadTime)
        {
            this.isComplete = true;
            weaponMag.ReloadMagzine(weaponMag, ammoProuch, this);
            Reloading();
        }
    }

    public override void Enter()
    {
        this.isComplete = false;
        //curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Enter;
        weaponMag.ReloadMagzine(weaponMag, ammoProuch, this);
        weaponAdvanceUser.weaponAfterAction.SendFeedBackWeaponAfterAction
            <ReloadMagazineFullStage>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);

        elaspeTime = 0;
    }
    private void Reloading()
    {
        weaponAdvanceUser._currentWeapon.bulletStore[BulletStackType.Chamber] += 1;
        weaponAdvanceUser._currentWeapon.bulletStore[BulletStackType.Magazine] -= 1;
        isComplete = true;

    }
    public override void Exit()
    {
        isComplete = false;
        
        //curReloadPhase = IReloadMagazineNodePhase.ReloadMagazinePhase.Exit;
        weaponAdvanceUser.weaponAfterAction.SendFeedBackWeaponAfterAction
           <ReloadMagazineFullStage>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        elaspeTime = 0;
    }
}
