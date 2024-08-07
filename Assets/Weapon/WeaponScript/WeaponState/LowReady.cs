using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowReady : WeaponStance
{
    [SerializeField] private WeaponSingleton weaponSingleton;
    public LowReady()
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.LowReady, weaponSingleton.GetWeapon());
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    public override void WeaponStanceUpdate(WeaponStanceManager weaponStanceManager)
    {

        WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.LowReady, weaponSingleton.GetWeapon());
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
