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

    protected BulletCapacity magazine => weaponMag._weapon.bulletCap;

    private float elaspeTime;

    protected override IWeaponAdvanceUser weaponAdvanceUser { get => weaponMag._weapon.userWeapon ;}
    private AmmoProuch ammoProuch => weaponAdvanceUser._weaponBelt.ammoProuch;
    protected TimelineTriggerEvent timelineTriggerEvent { get; set; }
    public TacticalReloadMagazineFullStageNodeLeaf(IWeaponAdvanceUser weaponUser, MagazineType magazineType,TimelineTriggerEventScriptableObject timelineTriggerEventSCRP, Func<bool> preCondition) : base(weaponUser, preCondition)
    {
        this.weaponMag = magazineType;
        this.timelineTriggerEvent = new TimelineTriggerEvent(magazineType._weapon.reloadTime, timelineTriggerEventSCRP.triggerEventDetail);

        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineEvent.ReleaseMag.ToString(), this.RelesesMag);
        this.timelineTriggerEvent.SubscribeEvent(IReloadMagazineNode.ReloadMagazineEvent.InputMag.ToString(), this.InputMag);
    }

    public override void Enter()
    {
        try
        {
            this.isComplete = false;
            this.curReloadStage = TacticalReloadStage.Enter;
            this.timelineTriggerEvent.Rewind();
            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
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


            this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction
               <TacticalReloadMagazineFullStageNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

            elaspeTime = 0;
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
        BulletCapacity newMagazine = new BulletCapacity(this.weaponMag._weapon.bullet, this.weaponMag._weapon.maxAmmoCapacity);
        this.weaponMag.InputMagazine(newMagazine);
    }


}
