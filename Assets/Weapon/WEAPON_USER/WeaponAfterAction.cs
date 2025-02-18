using UnityEngine;

public abstract class WeaponAfterAction 
{
    public abstract void LowReady(Weapon weapon);
    public abstract void AimDownSight(Weapon weapon);
    public abstract void Firing(Weapon weapon);
    public abstract void AfterFiringSingleAction(Weapon weapon);
    public abstract void Reload(Weapon weapon, IReloadNode reloadNode);
    public abstract void SwitchingWeapon(Weapon weapon,IWeaponTransitionNodeLeaf weaponTransitionNodeLeaf);
    public abstract void Resting(Weapon weapon);
    public abstract void HitDamageAble(IBulletDamageAble bulletDamageAble);
}
