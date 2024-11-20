using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : WeaponState
{
    private Animator ReloadAnimator { get; set; }
    private WeaponStateManager weaponStateManager { get; set; }
    private AmmoProuch ammoProuch { get; set; }
    private float reloadTime;
    private float tacTicalReloadTime;
    private Coroutine currentReload;
    public Reload(Weapon weapon,float reloadTime) : base(weapon)
    {
        this.reloadTime =  reloadTime;
        this.tacTicalReloadTime = reloadTime;
    }
    
    public ReloadType reloadType { get; set; }
    
    public override void EnterState()
    {
        if(weaponStateManager == null)
        {
            weaponStateManager = base._weapon.weapon_stateManager;
        }
        if (ReloadAnimator == null)
        {
            ReloadAnimator = base._weapon.userWeapon.animator;
        }
        if (base._weapon.Magazine_count == base._weapon.Magazine_capacity)
        {
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
        else if (base._weapon.Magazine_count > 0)
        {
            reloadType = ReloadType.TacticalReload;
            base._weapon.Notify(base._weapon, WeaponSubject.WeaponNotifyType.TacticalReload);
            base._weapon.userWeapon.Reloading(base._weapon, reloadType);
            currentReload = base._weapon.StartCoroutine(Reloading());
        }
        else if (base._weapon.Magazine_count <= 0)
        {
            reloadType = ReloadType.ReloadMagOut;
            base._weapon.Notify(base._weapon, WeaponSubject.WeaponNotifyType.Reloading);
            base._weapon.userWeapon.Reloading(base._weapon, reloadType);
            currentReload = this._weapon.StartCoroutine(Reloading());
        }

        base.EnterState();
    }

    public override void ExitState()
    {
        if(currentReload != null)
        {
            this._weapon.StopCoroutine(currentReload);
        }
    }
    public override void WeaponStateUpdate(WeaponStateManager weaponStateManager)
    {
    }
    IEnumerator Reloading()
    {
        if(reloadType == ReloadType.ReloadMagOut)
        {
            yield return new WaitForSeconds(reloadTime);
            reloadType = ReloadType.ReloadFinished;
            base._weapon.userWeapon.Reloading(base._weapon, reloadType);
            base._weapon.Chamber_Count += 1;
            base._weapon.Magazine_count -= 1;
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
        else
        {
            yield return new WaitForSeconds(tacTicalReloadTime);
            reloadType = ReloadType.ReloadFinished;
            base._weapon.userWeapon.Reloading(base._weapon, reloadType);
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
    }
    public override void WeaponStateFixedUpdate(WeaponStateManager weaponStateManager)
    {
       
    }

}
