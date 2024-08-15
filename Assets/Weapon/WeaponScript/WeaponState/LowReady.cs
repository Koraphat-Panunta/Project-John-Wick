using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowReady : WeaponStance
{
    private WeaponSingleton weaponSingleton;
    public LowReady(WeaponSingleton weaponSingleton)
    {
        this.weaponSingleton = weaponSingleton;
    }
    public override void EnterState()
    {
        base.EnterState();

    }

    public override void ExitState()
    {
        base.ExitState();
    }
    public override void WeaponStanceUpdate(WeaponStanceManager weaponStanceManager)
    {
        weaponSingleton.UserWeapon.LowReadying(weaponSingleton.GetWeapon());
        weaponStanceManager.AimingWeight -= weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        weaponStanceManager.AimingWeight = Mathf.Clamp(weaponStanceManager.AimingWeight, 0, 1);
    }
    
    public override void WeaponStanceFixedUpdate(WeaponStanceManager weaponStanceManager)
    {
    }
    protected override void Start()
    {
        
        base.Start();
    }
   
}
