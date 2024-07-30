using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : WeaponState
{
    float Recover_Time;
    [SerializeField] WeaponSingleton weaponSingleton;
    

    public event Action<Weapon> WeaponFire;
    public override void EnterState()
    {
        if(weaponSingleton.CurState != this)
        {
            if (weaponSingleton.GetWeapon().Magazine_count > 0)
            {
                WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.Fire, weaponSingleton.GetWeapon());
                weaponSingleton.FireEvent.Invoke(weaponSingleton.GetWeapon());
            }
            else
            {
                weaponSingleton.GetStateManager().ChangeState(weaponSingleton.GetStateManager().none);
            }
        }        
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState(StateManager stateManager)
    {
        Recover_Time -= Time.deltaTime;
        Recover_Time = Mathf.Clamp(Recover_Time, 0, 15);
        if(Recover_Time <= 0)
        {
            stateManager.ChangeState(weaponSingleton.GetStateManager().none);
        }
        base.FrameUpdateState(stateManager);
    }

    public override void PhysicUpdateState(StateManager stateManager)
    {
        base.PhysicUpdateState(stateManager);
    }
    private void SetRateoffire(Weapon weapon)
    {    
         Recover_Time = (float)60 / weaponSingleton.GetWeapon().rate_of_fire;
         MinusBullet(weapon);
    }
    private void MinusBullet(Weapon weapon)
    {
        weapon.Magazine_count -= 1;
    }
    private void OnEnable()
    {
        weaponSingleton.FireEvent += SetRateoffire;
    }
}
