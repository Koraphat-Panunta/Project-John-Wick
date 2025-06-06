using System;
using UnityEngine;

public interface MagazineType 
{
    public Weapon _weapon { get; set; }
    public ReloadMagazineLogic _reloadMagazineLogic { get; set; }
    public NodeSelector _reloadStageSelector { get; set; }
    public ReloadMagazineFullStage _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage _tacticalReloadMagazineFullStage { get; set; }
    public bool _isMagIn { get; set; }
    public void InitailizedReloadStageSelector();
    public void ReloadMagzine(MagazineType magazineWeapon, AmmoProuch ammoProuch,IReloadMagazineNode reloadMagazineNode);
   
}
public class ReloadMagazineLogic
{
    public void ReloadMagzine(MagazineType magazineWeapon, AmmoProuch ammoProuch, IReloadMagazineNode reloadMagazineNode)
    {
        //Debug.Log("Reload finish");
        INodeLeaf node = reloadMagazineNode as WeaponManuverLeafNode;
        Action enter = node.Enter;

        Weapon weapon = magazineWeapon._weapon;

        if (action.Method == enter.Method)
        {
            if (reloadMagazineNode is TacticalReloadMagazineFullStage)
                weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.TacticalReloadMagazineFullStage);
            else if (reloadMagazineNode is ReloadMagazineFullStage)
                weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.ReloadMagazineFullStage);
        }

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
