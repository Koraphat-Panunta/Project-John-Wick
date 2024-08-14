using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : WeaponState
{
    private WeaponSingleton weaponSingleton { get; set; }
    private Animator ReloadAnimator { get; set; }
    private WeaponStateManager weaponStateManager { get; set; }
    private AmmoProuch ammoProuch { get; set; }
    public enum ReloadType
    {
        ReloadMagOut,
        TacticalReload
    }
    ReloadType reloadType { get; set; }
    public Reload(WeaponSingleton weaponSingleton) 
    {
        this.weaponSingleton = weaponSingleton;
        weaponStateManager = weaponSingleton.GetStateManager();
    }
    public override void EnterState()
    {
        if(ReloadAnimator == null)
        {
            ReloadAnimator = weaponSingleton.UserWeapon.GetComponent<Animator>();
        }
        if(weaponSingleton.GetWeapon().Magazine_count == weaponSingleton.GetWeapon().Magazine_capacity)
        {
            weaponStateManager.ChangeState(weaponStateManager.none);
        }
        else if (weaponSingleton.GetWeapon().Magazine_count > 0)
        {
            reloadType = ReloadType.TacticalReload;
            ReloadAnimator.SetTrigger("TacticalReload");
            ReloadAnimator.SetLayerWeight(2, 1);
        }
        else if(weaponSingleton.GetWeapon().Magazine_count <= 0)
        {
            reloadType=ReloadType.ReloadMagOut;
            ReloadAnimator.SetTrigger("Reloading");
            ReloadAnimator.SetLayerWeight(2, 1);
        }
       
        base.EnterState();
    }

    public override void ExitState()
    {
        ReloadAnimator.SetLayerWeight(2, 0);
    }
    public override void WeaponStateUpdate(WeaponStateManager weaponStateManager)
    {
        if (reloadType == ReloadType.ReloadMagOut)
        {
            if (ReloadAnimator.GetCurrentAnimatorStateInfo(2).IsName("Reloading") && ReloadAnimator.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.95f)
            {
                weaponSingleton.GetWeapon().Chamber_Count += 1;
                weaponSingleton.GetWeapon().Magazine_count -= 1;
                weaponStateManager.ChangeState(weaponStateManager.none);
            }
        }
        else
        {
            if (ReloadAnimator.GetCurrentAnimatorStateInfo(2).IsName("TacticalReload") && ReloadAnimator.GetCurrentAnimatorStateInfo(2).normalizedTime >= 0.95f)
            {
                weaponStateManager.ChangeState(weaponStateManager.none);
            }
        }
    }
    public override void WeaponStateFixedUpdate(WeaponStateManager weaponStateManager)
    {
       
    }

}
