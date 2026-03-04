using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStageNodeLeaf : WeaponManuverLeafNode, IReloadMagazineNode
{
  

    private bool isComplete;

    private float reloadTime => this.weaponAdvanceUser != null ? this.weaponAdvanceUser._ReloadDuration : this.weaponMag._weapon.reloadTime;
    public float _reloadTime => this.reloadTime;

    private MagazineType weaponMag ;

    protected BulletCapacity magazine => this.weaponMag._weapon.TryGetBulletCapacity(out BulletCapacity bulletCapacity) ? bulletCapacity : null;

    private float elaspeTime;


    private AmmoProuch ammoProuch => weaponAdvanceUser._weaponBelt.ammoProuch;
    protected TimelineTriggerEvent timelineTriggerEvent { get; set; }

    public float _startReloadStageNormalizedTime => 0;

    public float _endReloadStageNormalizedTime => 1;

    public TacticalReloadMagazineFullStageNodeLeaf(IWeaponAdvanceUser weaponUser, MagazineType magazineType,TimelineTriggerEventScriptableObject timelineTriggerEventSCRP, Func<bool> preCondition) : base(weaponUser, preCondition)
    {
        this.weaponMag = magazineType;
        this.timelineTriggerEvent = new TimelineTriggerEvent(magazineType._weapon.reloadTime, timelineTriggerEventSCRP.triggerEventDetail);

        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In.ToString(), this.PickUpMag_In);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.ReleaseMag.ToString(), this.ReleaseMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.InputMag.ToString(), this.InputMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.KeepMag_Out.ToString(), this.KeepMag_Out);
    }

    public override void Enter()
    {
        this.isComplete = false;
        this.curPhase = WeaponManuverLeafNodePhase.Enter;
        this.timelineTriggerEvent.Rewind();
        this.weaponMag._weapon.Notify<TacticalReloadMagazineFullStageNodeLeaf>(this.weaponMag._weapon, this);
        this.elaspeTime = 0;

        try
        {
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
                <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
        catch
        {

        }
    }

    public override void Exit()
    {
        this.isComplete = false;
        this.curPhase = WeaponManuverLeafNodePhase.Exit;
        this.weaponMag._weapon.Notify<TacticalReloadMagazineFullStageNodeLeaf>(this.weaponMag._weapon, this);
        this.elaspeTime = 0;

        try
        {
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
                <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
        catch
        {

        }
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
        if (weaponAdvanceUser == null)
            return true;

        if (IsComplete())
            return true;

        if (weaponAdvanceUser._weaponManuverManager.isReloadManuverAble == false)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        this.timelineTriggerEvent.UpdatePlay(Time.deltaTime);
        if(timelineTriggerEvent.IsPlayFinish())
            isComplete = true;
    }
    private void PickUpMag_In()
    {
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In);
    }

  

    private void ReleaseMag()
    {
        //Debug.Log("TacticalReload Release Mag " + timelineTriggerEvent.timerNormalized);

        this.magazine.UnLoadAllBullet(out int remainBullet);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.ForceAddAmmo(this.magazine.bullet.myType, remainBullet);
        this.weaponMag.ReleseMagazine();
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.ReleaseMag);
    }

   
    private void InputMag()
    {
        //Debug.Log("TacticalReload InputMag " + timelineTriggerEvent.timerNormalized);

        BulletCapacity newMagazine = new BulletCapacity(this.weaponMag._weapon.bullet, this.weaponMag._weapon.maxAmmoCapacity);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.GetAmmoOut(this.weaponMag._weapon.bullet.myType, newMagazine.maxCapacity, out int amoutAmmo);
        newMagazine.Load(this.weaponMag._weapon.bullet, amoutAmmo, out int overAmount);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.AddAmmo(this.weaponMag._weapon.bullet.myType, overAmount);
        this.weaponMag.InputMagazine(newMagazine);
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.InputMag);
    }
    private void KeepMag_Out()
    {
        //Debug.Log("TacticalReload KeepMag_Out " + timelineTriggerEvent.timerNormalized);
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.KeepMag_Out);
    }
    private void ReChamber()
    {
        //Debug.Log("TacticalReload ReChamber " + timelineTriggerEvent.timerNormalized);
        this.weaponMag.ReloadChamber();
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.ReChamber);
    }



}
