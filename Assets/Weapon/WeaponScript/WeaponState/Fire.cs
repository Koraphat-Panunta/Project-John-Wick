using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : WeaponState
{
    private WeaponSingleton weaponSingleton;
    private WeaponStateManager weaponStateManager;

    public Fire(WeaponSingleton weaponSingleton)
    {
        this.weaponSingleton = weaponSingleton;
        this.weaponStateManager = weaponSingleton.GetStateManager();
    }

    public event Action<Weapon> WeaponFire;
    public override void EnterState()
    {
       
        if (weaponSingleton.GetWeapon().Chamber_Count > 0)
        {
            weaponSingleton.FireEvent.Invoke(weaponSingleton.GetWeapon());
            weaponSingleton.UserWeapon.Firing(weaponSingleton.GetWeapon());
            weaponStateManager.StartCoroutine(AfterShoot());
        }
        else
        {
            weaponStateManager.ChangeState(weaponSingleton.GetStateManager().none);
        }
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

   
    public override void WeaponStateUpdate(WeaponStateManager weaponStateManager)
    {
       
    }
   
    public override void WeaponStateFixedUpdate(WeaponStateManager weaponStateManager)
    {
        
    }
    IEnumerator AfterShoot()
    {
        MinusBullet(weaponSingleton.GetWeapon());
        yield return new WaitForSeconds((float)(60/weaponSingleton.GetWeapon().rate_of_fire));
        weaponStateManager.ChangeState(weaponSingleton.GetStateManager().none); 
    }
    private void MinusBullet(Weapon weapon)
    {
        weapon.Chamber_Count -= 1;
        if (weapon.Magazine_count > 0)
        {
            weapon.Magazine_count -= 1;
            weapon.Chamber_Count += 1;
        }
    }

}
