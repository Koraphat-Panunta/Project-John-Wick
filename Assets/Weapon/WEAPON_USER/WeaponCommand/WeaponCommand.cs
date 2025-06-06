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
        this.weaponManuverManager.isPullTriggerManuverAble = true;
    }
    public virtual void CancleTrigger()
    {
        this.weaponManuverManager.isPullTriggerManuverAble = false;
    }
    public virtual void Reload(AmmoProuch ammoProuch)
    {
        this.weaponManuverManager.isReloadManuverAble = true;
    }
    public virtual void LowReady()
    {
        weaponUser.weaponManuverManager.isAimingManuverAble = false;
    }
    public virtual void AimDownSight()
    {
        weaponUser.weaponManuverManager.isAimingManuverAble = true;    
    }
    public virtual void SwitchWeapon()
    {
       weaponManuverManager.isSwitchWeaponManuverAble = true;
    }
    public void DropWeapon()
    {

    }
    public void PickUpWeapon()
    {

    }
}
