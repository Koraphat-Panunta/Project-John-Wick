using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStageNodeLeaf : WeaponManuverLeafNode, IReloadMagazineNode
{
    public enum TacticalReloadStage
    {
        Enter,
        Reloading,

        Cancel
    }

    private bool isComplete;
    public TacticalReloadStage curReloadStage { get; private set; }

    private float reloadTime => weaponAdvanceUser._currentWeapon.reloadTime;

    private MagazineType weaponMag ;

    private float elaspeTime;

    protected override IWeaponAdvanceUser weaponAdvanceUser { get => weaponMag._weapon.userWeapon ;}
    private AmmoProuch ammoProuch => weaponAdvanceUser._weaponBelt.ammoProuch;
    public TacticalReloadMagazineFullStageNodeLeaf(IWeaponAdvanceUser weaponUser, MagazineType magazineType, Func<bool> preCondition) : base(weaponUser, preCondition)
    {
        this.weaponMag = magazineType;
    }

    public override void Enter()
    {
        try
        {
            this.isComplete = false;
            this.curReloadStage = TacticalReloadStage.Enter;
            weaponMag.ReloadMagazine(weaponMag, ammoProuch, this);
            weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
                <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

            elaspeTime = 0;
        }
        catch
        {

        }
    }

    public override void Exit()
    {
        try
        {
            isComplete = false;


            weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
               <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

            elaspeTime = 0;
        }
        catch
        {

        }
    }

    public override void FixedUpdateNode()
    {
        try
        {
            if (weaponAdvanceUser._isAimingCommand && weaponAdvanceUser._weaponManuverManager.isAimingManuverAble)
                weaponAdvanceUser._weaponManuverManager.aimingWeight
                    = Mathf.Clamp01(weaponAdvanceUser._weaponManuverManager.aimingWeight + Time.deltaTime * weaponAdvanceUser._currentWeapon.aimDownSight_speed);
            else
                weaponAdvanceUser._weaponManuverManager.aimingWeight
                    = Mathf.Clamp01(weaponAdvanceUser._weaponManuverManager.aimingWeight - Time.deltaTime * weaponAdvanceUser._currentWeapon.aimDownSight_speed);
        }
        catch { }
    }

    public override bool IsComplete()
    {
        return this.isComplete;
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;

        if (weaponAdvanceUser._weaponManuverManager.isReloadManuverAble == false)
            return true;

        if (weaponAdvanceUser == null)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        try
        {
            elaspeTime += Time.deltaTime;
            if (elaspeTime >= reloadTime)
            {
                this.isComplete = true;
                this.curReloadStage = TacticalReloadStage.Reloading;
                weaponMag.ReloadMagazine(weaponMag, ammoProuch, this);
            }
        }
        catch { }

    }



}
