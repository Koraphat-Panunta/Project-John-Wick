using System;
using UnityEngine;

public interface MagazineType 
{
    public Weapon _weapon { get; set; }
    public ReloadMagazineLogic _reloadMagazineLogic { get; set; }
    public NodeSelector _reloadStageSelector { get; set; }
    public ReloadMagazineFullStageNodeLeaf _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagazineFullStage { get; set; }
    public bool _isMagIn { get; set; }
    public void InitailizedReloadStageSelector();
    public void ReloadMagazine(MagazineType magazineWeapon, AmmoProuch ammoProuch,IReloadMagazineNode reloadMagazineNode);
   
}
public class ReloadMagazineLogic
{
    public void ReloadMagazine(MagazineType magazineWeapon, AmmoProuch ammoProuch, IReloadMagazineNode reloadMagazineNode)
    {
        //Debug.Log("Reload finish");
        INodeLeaf node = reloadMagazineNode as WeaponManuverLeafNode;
        Action enter = node.Enter;

        Weapon weapon = magazineWeapon._weapon;

        switch (reloadMagazineNode)
        {
            case ReloadMagazineFullStageNodeLeaf _reloadMagFullStage:
                {
                    if (_reloadMagFullStage.curReloadStage == ReloadMagazineFullStageNodeLeaf.ReloadStage.Enter)
                        weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.ReloadMagazineFullStage);
                    else if(_reloadMagFullStage.curReloadStage == ReloadMagazineFullStageNodeLeaf.ReloadStage.Reloading)
                        this.RefillAmmo(weapon, ammoProuch);
                    break;
                }
        }
       
    }
    public void InitailizedReloadStageSelector(MagazineType magazineType)
    {

        Weapon weapon = magazineType._weapon;
        IWeaponAdvanceUser userWeapon = weapon.userWeapon;
        Bullet bullet = weapon.bullet;

        NodeSelector _reloadStageSelector = magazineType._reloadStageSelector;
        ReloadMagazineFullStageNodeLeaf _reloadMagazineFullStage = magazineType._reloadMagazineFullStage;
        TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagazineFullStage = magazineType._tacticalReloadMagazineFullStage;

        _reloadStageSelector = new NodeSelector(
           () => {
               if (userWeapon.isReloadCommand
              && userWeapon.weaponBelt.ammoProuch.amountOf_ammo[bullet.myType] > 0
              &&weapon.bulletStore[BulletStackType.Magazine] <weapon.bulletCapacity)
                   return true;
               else
                   return false;
           }
           );

        _reloadMagazineFullStage = new ReloadMagazineFullStageNodeLeaf(
            userWeapon, () =>
            {
                int chamberCount = weapon.bulletStore[BulletStackType.Chamber];
                int magCount = weapon.bulletStore[BulletStackType.Magazine];
                bool isMagIn = magazineType._isMagIn;

                if
                    (
                     isMagIn == true
                    && chamberCount == 0
                    && magCount == 0
                    )
                    return true;
                else
                    return false;
            });

        _tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStageNodeLeaf(userWeapon
            ,
            () =>
            {
                bool IsMagIn = magazineType._isMagIn;
                int MagCount = weapon.bulletStore[BulletStackType.Magazine];
                if (
                    IsMagIn == true
                    && MagCount >= 0
                    )
                    return true;
                else
                    return false;
            }
            );

        _reloadStageSelector.AddtoChildNode(_reloadMagazineFullStage);
        _reloadStageSelector.AddtoChildNode(_tacticalReloadMagazineFullStage);
    }
    private void RefillAmmo(Weapon weapon,AmmoProuch ammoProuch)
    {
        BulletType bulletType = weapon.bullet.myType;
        int magCount = weapon.bulletStore[BulletStackType.Magazine];
        int magCapacity = weapon.bulletCapacity;
        if (ammoProuch.amountOf_ammo[bulletType] > 0)
        {
            int fillamout = magCapacity - magCount;
            if (ammoProuch.amountOf_ammo[bulletType] - fillamout < 0)
            {
                int minusAmmo = ammoProuch.amountOf_ammo[bulletType] -= fillamout;
                ammoProuch.amountOf_ammo[bulletType] = 0;
                weapon.bulletStore[BulletStackType.Magazine] += fillamout + minusAmmo;
            }
            else
            {
                ammoProuch.amountOf_ammo[bulletType] -= fillamout;
                weapon.bulletStore[BulletStackType.Magazine] += fillamout;
            }

        }
    }
}
