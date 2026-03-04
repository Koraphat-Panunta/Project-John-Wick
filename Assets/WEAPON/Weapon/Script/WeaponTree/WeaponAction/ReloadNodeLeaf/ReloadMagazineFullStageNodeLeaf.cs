using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStageNodeLeaf : WeaponManuverLeafNode,IReloadMagazineNode/*,IReloadMagazineNodePhase*/
{
    
    private bool isComplete;

    private float reloadTime => this.weaponAdvanceUser != null?this.weaponAdvanceUser._ReloadDuration:this.weaponMag._weapon.reloadTime;
    public float _reloadTime => this.reloadTime ;

    private MagazineType weaponMag;
    protected TimelineTriggerEvent timelineTriggerEvent { get; set; }


    private AmmoProuch ammoProuch => weaponAdvanceUser._weaponBelt.ammoProuch;
    protected BulletCapacity magazine => weaponMag._weapon.TryGetBulletCapacity(out BulletCapacity bulletCapacity)?bulletCapacity:null;

    public float _startReloadStageNormalizedTime => this.startReloadStageNormalizedTime;

    public float _endReloadStageNormalizedTime => this.endReloadStageNormalizedTime;

    protected float startReloadStageNormalizedTime;
    protected float endReloadStageNormalizedTime;

    public ReloadMagazineFullStageNodeLeaf
        (IWeaponAdvanceUser weaponUser
        , MagazineType weaponMag
        ,TimelineTriggerEventScriptableObject timelineTriggerEventScriptableObject
        , Func<bool> preCondition) :
       base(
           weaponUser
           , preCondition
            )
    {
        this.weaponMag = weaponMag;
        this.timelineTriggerEvent = new TimelineTriggerEvent(this.reloadTime, timelineTriggerEventScriptableObject.triggerEventDetail);
        this.startReloadStageNormalizedTime = 0;
        this.endReloadStageNormalizedTime = 1;

        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.ReleaseMag.ToString(), this.ReleaseMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In.ToString(), this.PickUpMag_In);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.InputMag.ToString(), this.InputMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.ReChamber.ToString(), this.ReChamber);
    }

    

    public ReloadMagazineFullStageNodeLeaf
        (IWeaponAdvanceUser weaponUser
        , MagazineType weaponMag
        , IReloadMagazineNode.ReloadMagazineStage startReloadStage
        , IReloadMagazineNode.ReloadMagazineStage endReloadStage
        , TimelineTriggerEventScriptableObject timelineTriggerEventScriptableObject
        , Func<bool> preCondition) : base(weaponUser, preCondition)
    {
        this.weaponMag = weaponMag;
        this.timelineTriggerEvent = new TimelineTriggerEvent(this.reloadTime, timelineTriggerEventScriptableObject.triggerEventDetail);
        this.startReloadStageNormalizedTime = this.timelineTriggerEvent.GetEventNormalizedTime(startReloadStage.ToString());
        this.endReloadStageNormalizedTime = Mathf.Clamp01(this.timelineTriggerEvent.GetEventNormalizedTime(endReloadStage.ToString()) + .1f);

        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.ReleaseMag.ToString(), this.ReleaseMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In.ToString(), this.PickUpMag_In);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.InputMag.ToString(), this.InputMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineStage.ReChamber.ToString(), this.ReChamber);
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

        if(weaponAdvanceUser._weaponManuverManager.isReloadManuverAble == false)
            return true;

        
        return false;
    }
    public override void Enter()
    {

        curPhase = WeaponManuverLeafNodePhase.Enter;
        this.isComplete = false;
        this.timelineTriggerEvent.RewindAt(this.startReloadStageNormalizedTime * this.timelineTriggerEvent.timeDuration);
        base.Enter();
        this.weaponMag._weapon.Notify<ReloadMagazineFullStageNodeLeaf>(this.weaponMag._weapon, this);
        try
        {
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
                <ReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
        catch { }
    }
    public override void Exit()
    {
        isComplete = false;
        base.Exit();
        this.weaponMag._weapon.Notify<ReloadMagazineFullStageNodeLeaf>(this.weaponMag._weapon, this);
        try
        {
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
               <ReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
            
        }
        catch { }
    }
    public override void UpdateNode()
    {
        this.timelineTriggerEvent.UpdatePlay(Time.deltaTime);

        //Debug.Log("ReloadTimer = " + this.timelineTriggerEvent.timer);
        //Debug.Log("ReloadTimerNormal = " + this.timelineTriggerEvent.timerNormalized);
        if (this.timelineTriggerEvent.IsPlayFinish(this.endReloadStageNormalizedTime))
            isComplete = true;
    }

    private void PickUpMag_In()
    {
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In);
    }

   

    private void ReleaseMag() 
    {
        //Debug.Log("Reload Release Mag");

        if(this.magazine == null)
            return;

        this.magazine.UnLoadAllBullet(out int remainBullet);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.ForceAddAmmo(this.magazine.bullet.myType, remainBullet);
        this.weaponMag.ReleseMagazine();
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.ReleaseMag);
    }

   
    private void InputMag() 
    {
        //Debug.Log("Reload InputMag");

        BulletCapacity newMagazine = new BulletCapacity(this.weaponMag._weapon.bullet, this.weaponMag._weapon.maxAmmoCapacity);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.GetAmmoOut(this.weaponMag._weapon.bullet.myType, newMagazine.maxCapacity, out int amoutAmmo);
        newMagazine.Load(this.weaponMag._weapon.bullet, amoutAmmo, out int overAmount);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.AddAmmo(this.weaponMag._weapon.bullet.myType, overAmount);
        this.weaponMag.InputMagazine(newMagazine);
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.InputMag);

    } 
    private void KeepMag_Out()
    {
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.KeepMag_Out);
    }
    private void ReChamber() 
    {
        this.weaponMag.ReloadChamber();
        this.weaponMag._weapon.Notify(this.weaponMag._weapon, IReloadMagazineNode.ReloadMagazineStage.ReChamber);
    } 

    public override void FixedUpdateNode()
    {
        
    }
}
