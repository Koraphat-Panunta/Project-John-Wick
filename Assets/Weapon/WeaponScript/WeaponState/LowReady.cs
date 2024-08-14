using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowReady : WeaponStance
{
    private WeaponSingleton weaponSingleton;
    public LowReady(WeaponSingleton weaponSingleton)
    {
        this.weaponSingleton = weaponSingleton;
        base.animator = this.weaponSingleton.animator;
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

        weaponSingleton.LowReady.Invoke(weaponSingleton.GetWeapon());
        base.animator.SetLayerWeight(1, weaponSingleton.GetStanceManager().AimingWeight);
    }
    
    public override void WeaponStanceFixedUpdate(WeaponStanceManager weaponStanceManager)
    {
    }
    protected override void Start()
    {
        
        base.Start();
    }
   
}
