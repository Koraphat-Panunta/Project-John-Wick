using UnityEngine;

public abstract class WeaponAfterAction 
{
    public abstract void LowReady(Weapon weapon);
    public abstract void AimDownSight(Weapon weapon);
    public abstract void Firing(Weapon weapon);
    public abstract void AfterFiringSingleAction(Weapon weapon);
    public abstract void ReloadingMagazine(Weapon weapon);
    public abstract void Tactical_ReloadMagazine(Weapon weapon);
    public abstract void PreLoad(Weapon weapon);
    public abstract void Reload_ChamberAction(Weapon weapon);
    public abstract void Reload_SingleAction(Weapon weapon);
    public abstract void Reload(Weapon weapon, ReloadType reloadType);
    public abstract void HitDamageAble(IBulletDamageAble bulletDamageAble);
}
