using System;
using UnityEngine;

public interface MagazineType 
{
    public Weapon _weapon { get; set; }
    public ReloadMagazineLogic _reloadMagazineLogic { get; set; }
    public NodeSelector _reloadStageSelector { get; set; }
    public TimelineTriggerEventScriptableObject _reload_timelineTriggerEventSCRP { get; }
    public TimelineTriggerEventScriptableObject _tacticalReload_timelineTriggerEventSCRP { get; }

    public ReloadMagazineFullStageNodeLeaf _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagazineFullStage { get; set; }
    public ReloadMagazineFullStageNodeLeaf _magInputLoadBarrelReloadMagazineStage { get; set; }
    public ReloadMagazineFullStageNodeLeaf _magInputReloadMagazineStage { get; set; }
    public ReloadMagazineFullStageNodeLeaf _barrelLoadReloadMagazineStage { get; set; }

    
    public bool isMagin => _weapon.TryGetBulletCapacity(out BulletCapacity bulletCapacity);

    public void InitailizedReloadStageSelector();
    public void ReleseMagazine();
    public void InputMagazine(BulletCapacity magazine);
    public void ReloadChamber();
   
}
public class ReloadMagazineLogic
{

    public void InitailizedReloadStageSelector(MagazineType magazineType)
    {

        Weapon weapon = magazineType._weapon;

        magazineType._reloadStageSelector = new NodeSelector(
           () => {
               if (weapon.userWeapon != null
               && weapon.userWeapon._isReloadCommand
               && weapon.userWeapon._weaponManuverManager.isReloadManuverAble
              && weapon.userWeapon._weaponBelt.ammoProuch.CheckAmmo(weapon.bullet.myType) > 0
              && (weapon.curBulletCapacity < weapon.maxAmmoCapacity || weapon.chamber.isLoad == false))
                   return true;
               else
                   return false;
           }
           );

        magazineType._reloadMagazineFullStage = new ReloadMagazineFullStageNodeLeaf(
            weapon.userWeapon, 
            magazineType,
            magazineType._reload_timelineTriggerEventSCRP,
            () =>
            {
                if
                    (
                     magazineType.isMagin
                    && weapon.chamber.isLoad == false
                    && weapon.curBulletCapacity <= 0
                    )
                    return true;
                else
                    return false;
            });

        magazineType._tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStageNodeLeaf(
            weapon.userWeapon,
            magazineType,
            magazineType._tacticalReload_timelineTriggerEventSCRP,
            () =>
            {

                if (
                    magazineType.isMagin
                    && weapon.chamber.isLoad
                    && weapon.curBulletCapacity >= 0
                    )
                    return true;
                else
                    return false;
            }
            );
        magazineType._magInputLoadBarrelReloadMagazineStage = new ReloadMagazineFullStageNodeLeaf(
            weapon.userWeapon
            , magazineType
            ,IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In
            ,IReloadMagazineNode.ReloadMagazineStage.ReChamber
            , magazineType._reload_timelineTriggerEventSCRP
            , () => magazineType.isMagin == false 
            && magazineType._weapon.chamber.isLoad == false
            );

        magazineType._magInputReloadMagazineStage = new ReloadMagazineFullStageNodeLeaf(
           weapon.userWeapon
           , magazineType
           , IReloadMagazineNode.ReloadMagazineStage.PickUpMag_In
           , IReloadMagazineNode.ReloadMagazineStage.InputMag
           , magazineType._reload_timelineTriggerEventSCRP
           , () => magazineType.isMagin == false
           );

        magazineType._barrelLoadReloadMagazineStage = new ReloadMagazineFullStageNodeLeaf(
           weapon.userWeapon
           , magazineType
           , IReloadMagazineNode.ReloadMagazineStage.InputMag
           , IReloadMagazineNode.ReloadMagazineStage.ReChamber
           , magazineType._reload_timelineTriggerEventSCRP
           , () => true);

        magazineType._reloadStageSelector.AddtoChildNode(magazineType._reloadMagazineFullStage);
        magazineType._reloadStageSelector.AddtoChildNode(magazineType._tacticalReloadMagazineFullStage);
        magazineType._reloadStageSelector.AddtoChildNode(magazineType._magInputLoadBarrelReloadMagazineStage);
        magazineType._reloadStageSelector.AddtoChildNode(magazineType._magInputReloadMagazineStage);
        magazineType._reloadStageSelector.AddtoChildNode(magazineType._barrelLoadReloadMagazineStage);
    }
    
}
