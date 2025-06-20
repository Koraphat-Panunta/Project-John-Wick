using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStageNodeLeaf : WeaponManuverLeafNode,IReloadMagazineNode/*,IReloadMagazineNodePhase*/
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

    private MagazineType weaponMag;
    protected override IWeaponAdvanceUser weaponAdvanceUser { get => weaponMag._weapon.userWeapon; }

    private float elaspeTime;

    private AmmoProuch ammoProuch => weaponAdvanceUser._weaponBelt.ammoProuch;

    public ReloadMagazineFullStageNodeLeaf(IWeaponAdvanceUser weaponUser, MagazineType weaponMag, Func<bool> preCondition) : base(weaponUser, preCondition)
    {
        this.weaponMag = weaponMag;
    }

    public override bool IsComplete()
    {
        return this.isComplete;
    }
    public override bool IsReset()
    {
        if (IsComplete())
            return true;

        if(weaponAdvanceUser._weaponManuverManager.isReloadManuverAble == false)
            return true;

        return false;
    }
    public override void Enter()
    {
        this.isComplete = false;
        this.curReloadStage = ReloadStage.Enter;
        weaponMag.ReloadMagazine(weaponMag, ammoProuch, this);
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
            <ReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        elaspeTime = 0;
    }
    public override void Exit()
    {
        isComplete = false;
        weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
           <ReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        elaspeTime = 0;
    }
    public override void UpdateNode()
    {

        elaspeTime += Time.deltaTime;
        if(elaspeTime >= reloadTime)
        {
            this.isComplete = true;
            this.curReloadStage = ReloadStage.Reloading; 
            weaponMag.ReloadMagazine(weaponMag, ammoProuch, this);
            Reloading();
        }
    }
    public override void FixedUpdateNode()
    {
        if (weaponAdvanceUser._isAimingCommand && weaponAdvanceUser._weaponManuverManager.isAimingManuverAble)
            weaponAdvanceUser._weaponManuverManager.aimingWeight
                = Mathf.Clamp01(weaponAdvanceUser._weaponManuverManager.aimingWeight + Time.deltaTime * weaponAdvanceUser._currentWeapon.aimDownSight_speed);
        else
            weaponAdvanceUser._weaponManuverManager.aimingWeight
                = Mathf.Clamp01(weaponAdvanceUser._weaponManuverManager.aimingWeight - Time.deltaTime * weaponAdvanceUser._currentWeapon.aimDownSight_speed);
    }
   

    private void Reloading()
    {
        weaponAdvanceUser._currentWeapon.bulletStore[BulletStackType.Chamber] += 1;
        weaponAdvanceUser._currentWeapon.bulletStore[BulletStackType.Magazine] -= 1;
        isComplete = true;
    }
   
}
