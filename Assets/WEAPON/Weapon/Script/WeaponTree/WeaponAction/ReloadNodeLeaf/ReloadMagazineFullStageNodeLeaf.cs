using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStageNodeLeaf : WeaponManuverLeafNode,IReloadMagazineNode/*,IReloadMagazineNodePhase*/
{
    
    private bool isComplete;

    private float reloadTime => weaponAdvanceUser._currentWeapon.reloadTime;

    private MagazineType weaponMag;
    protected override IWeaponAdvanceUser weaponAdvanceUser { get => weaponMag._weapon.userWeapon; }
    protected TimelineTriggerEvent timelineTriggerEvent { get; set; }


    private AmmoProuch ammoProuch => weaponAdvanceUser._weaponBelt.ammoProuch;
    protected BulletCapacity magazine => weaponMag._weapon.bulletCap;

    public ReloadMagazineFullStageNodeLeaf
        (IWeaponAdvanceUser weaponUser
        , MagazineType weaponMag
        ,TimelineTriggerEventScriptableObject timelineTriggerEventScriptableObject
        , Func<bool> preCondition) : base(weaponUser, preCondition)
    {
        this.weaponMag = weaponMag;
        this.timelineTriggerEvent = new TimelineTriggerEvent(this.reloadTime, timelineTriggerEventScriptableObject.triggerEventDetail);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineEvent.ReleaseMag.ToString(), this.RelesesMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineEvent.InputMag.ToString(), this.InputMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineEvent.ReChamber.ToString(), this.ReloadChamber);
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

        if(weaponAdvanceUser == null)
            return true;

        return false;
    }
    public override void Enter()
    {
        try
        {
            this.isComplete = false;
            this.timelineTriggerEvent.Rewind();
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
                <ReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
            this.weaponMag._weapon.Notify<ReloadMagazineFullStageNodeLeaf>(this.weaponMag._weapon, this);
        }
        catch { }
    }
    public override void Exit()
    {
        try
        {
            isComplete = false;
            weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
               <ReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        }
        catch { }
    }
    public override void UpdateNode()
    {
        this.timelineTriggerEvent.UpdatePlay(Time.deltaTime);
    }

    private void RelesesMag() 
    {
        this.magazine.UnLoadAllBullet(out int remainBullet);
        this.weaponAdvanceUser._weaponBelt.ammoProuch.ForceAddAmmo(this.magazine.bullet.myType, remainBullet);
        this.weaponMag.ReleseMagazine(); 
    }
    private void InputMag() 
    {
        BulletCapacity newMagazine = new BulletCapacity(this.weaponMag._weapon.bullet,this.weaponMag._weapon.maxAmmoCapacity);
        this.weaponMag.InputMagazine(newMagazine);
    } 
    private void ReloadChamber() => this.weaponMag.ReloadChamber(); 

    public override void FixedUpdateNode()
    {
        
    }
}
