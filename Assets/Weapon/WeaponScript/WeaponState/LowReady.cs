using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowReady : WeaponStance
{
    public LowReady(Weapon weapon) : base(weapon)
    {
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
        base._weapon.userWeapon.LowReadying(base._weapon);
        weaponStanceManager.AimingWeight -= base._weapon.aimDownSight_speed * Time.deltaTime;
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
