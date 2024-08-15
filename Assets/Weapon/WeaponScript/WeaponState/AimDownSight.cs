using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : WeaponStance
{
    [SerializeField] WeaponSingleton weaponSingleton;
    public AimDownSight(WeaponSingleton weaponSingleton)
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
        weaponSingleton.UserWeapon.Aiming(weaponSingleton.GetWeapon());
        weaponStanceManager.AimingWeight += weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        weaponStanceManager.AimingWeight = Mathf.Clamp(weaponStanceManager.AimingWeight, 0, 1);
    }
    public override void WeaponStanceFixedUpdate(WeaponStanceManager weaponStanceManager)
    {
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected float rotationSpeed = 5.0f;
   
}
