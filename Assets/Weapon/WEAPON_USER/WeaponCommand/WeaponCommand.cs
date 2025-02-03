using System.Collections;
using UnityEngine;

public class WeaponCommand
{
    private IWeaponAdvanceUser weaponUser;
    protected WeaponManuverManager weaponManuverManager;
    public WeaponCommand(IWeaponAdvanceUser weaponUser)
    {
        this.weaponUser = weaponUser;
        this.weaponManuverManager = weaponUser.weaponManuverManager;
    }
    public virtual void PullTrigger()
    {
        this.weaponManuverManager.isPullTriggerManuver = true;
    }
    public virtual void CancleTrigger()
    {
        this.weaponManuverManager.isPullTriggerManuver = false;
    }
    public virtual void Reload(AmmoProuch ammoProuch)
    {
        this.weaponManuverManager.isReloadManuver = true;
    }
    public virtual void LowReady()
    {
        weaponUser.weaponManuverManager.isAimingManuver = false;
    }
    public virtual void AimDownSight()
    {
        weaponUser.weaponManuverManager.isAimingManuver = true;    
    }
    public virtual void SwitchWeapon()
    {
       weaponManuverManager.isSwitchWeaponManuver = true;
    }
    public void DropWeapon()
    {

    }
    public void PickUpWeapon()
    {

    }
}
