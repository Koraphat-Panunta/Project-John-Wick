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

    private float reloadTime => weaponAdvanceUser._currentWeapon.reloadSpeed;

    private MagazineType weaponMag => weaponAdvanceUser._currentWeapon as MagazineType;

    private float elaspeTime;

    private AmmoProuch ammoProuch => weaponAdvanceUser.weaponBelt.ammoProuch;
    public TacticalReloadMagazineFullStageNodeLeaf(IWeaponAdvanceUser weaponUser, Func<bool> preCondition) : base(weaponUser, preCondition)
    {

    }

    public override void Enter()
    {
        this.isComplete = false;
        this.curReloadStage = TacticalReloadStage.Enter;
        weaponMag.ReloadMagazine(weaponMag, ammoProuch, this);
        weaponAdvanceUser.weaponAfterAction.SendFeedBackWeaponAfterAction
            <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        elaspeTime = 0;
    }

    public override void Exit()
    {
        isComplete = false;
        weaponAdvanceUser.weaponAfterAction.SendFeedBackWeaponAfterAction
           <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        elaspeTime = 0;
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

        if (weaponAdvanceUser.weaponManuverManager.isReloadManuverAble == false)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        elaspeTime += Time.deltaTime;
        if (elaspeTime >= reloadTime)
        {
            this.isComplete = true;
            this.curReloadStage = TacticalReloadStage.Reloading;
            weaponMag.ReloadMagazine(weaponMag, ammoProuch, this);
        }
    }



}
