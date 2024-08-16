using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : WeaponState
{
    private WeaponSingleton weaponSingleton { get; set; }
    private Animator ReloadAnimator { get; set; }
    private WeaponStateManager weaponStateManager { get; set; }
    private AmmoProuch ammoProuch { get; set; }
    private float reloadTime;
    private float tacTicalReloadTime;
    public enum ReloadType
    {
        ReloadMagOut,
        TacticalReload,
        ReloadFinished
    }
    ReloadType reloadType { get; set; }
    public Reload(WeaponSingleton weaponSingleton)
    {
        this.weaponSingleton = weaponSingleton;
        weaponStateManager = weaponSingleton.GetStateManager();
    }
    public override void EnterState()
    {
        if (ReloadAnimator == null)
        {
            ReloadAnimator = weaponSingleton.UserWeapon.GetComponent<Animator>();
            reloadTime = 3;
            tacTicalReloadTime = 3;
        }
        if (weaponSingleton.GetWeapon().Magazine_count == weaponSingleton.GetWeapon().Magazine_capacity)
        {
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
        else if (weaponSingleton.GetWeapon().Magazine_count > 0)
        {
            reloadType = ReloadType.TacticalReload;
            weaponSingleton.UserWeapon.Reloading(weaponSingleton.GetWeapon(), reloadType);
            weaponStateManager.StartCoroutine(Reloading());
        }
        else if (weaponSingleton.GetWeapon().Magazine_count <= 0)
        {
            reloadType = ReloadType.ReloadMagOut;
            weaponSingleton.UserWeapon.Reloading(weaponSingleton.GetWeapon(), reloadType);
            weaponStateManager.StartCoroutine(Reloading());
        }

        base.EnterState();
    }

    public override void ExitState()
    {
        reloadType = ReloadType.ReloadFinished;
        weaponSingleton.UserWeapon.Reloading(weaponSingleton.GetWeapon(), reloadType);
    }
    public override void WeaponStateUpdate(WeaponStateManager weaponStateManager)
    {
    }
    IEnumerator Reloading()
    {
        if(reloadType == ReloadType.ReloadMagOut)
        {
            yield return new WaitForSeconds(3);
            weaponSingleton.GetWeapon().Chamber_Count += 1;
            weaponSingleton.GetWeapon().Magazine_count -= 1;
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
        else
        {
            yield return new WaitForSeconds(3);
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
    }
    public override void WeaponStateFixedUpdate(WeaponStateManager weaponStateManager)
    {
       
    }

}
