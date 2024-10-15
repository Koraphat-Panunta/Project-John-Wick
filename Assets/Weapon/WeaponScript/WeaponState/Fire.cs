using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : WeaponState
{
   
    private WeaponStateManager weaponStateManager;
    public Fire(Weapon weapon) : base(weapon)
    {
    }

    public event Action<Weapon> WeaponFire;
    public override void EnterState()
    {
       if(weaponStateManager == null)
        {
            weaponStateManager = base._weapon.weapon_stateManager;
        }
        if (base._weapon.Chamber_Count > 0)
        {
            base._weapon.bulletSpawner.SpawnBullet(_weapon);
            _weapon.StartCoroutine(AfterShoot());
            base._weapon.userWeapon.Firing(base._weapon);
        }
        else
        {
            weaponStateManager.ChangeState(weaponStateManager.none);
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
        MinusBullet(base._weapon);
        yield return new WaitForSeconds((float)(60/base._weapon.rate_of_fire));
        weaponStateManager.ChangeState(weaponStateManager.none); 
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
