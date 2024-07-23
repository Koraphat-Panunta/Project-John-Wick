using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : WeaponState
{
    float Recover_Time;
    [SerializeField] WeaponSingleton weaponSingleton;

    public override void EnterState()
    {
        if(weaponSingleton.CurState != this)
        {
            Debug.Log("Fire");
            weaponSingleton.GetCrosshair().ShootSpread(weaponSingleton.GetWeapon().Recoil);
            weaponSingleton.Camera.transform.Rotate(-10,0,0);
            Recover_Time = (float)60/weaponSingleton.GetWeapon().rate_of_fire;
        }
        
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState()
    {
        Recover_Time -= Time.deltaTime;
        Recover_Time = Mathf.Clamp(Recover_Time, 0, 15);
        if(Recover_Time <= 0)
        {
            weaponSingleton.GetStateManager().ChangeState(weaponSingleton.GetStateManager().none);
        }
        base.FrameUpdateState();
    }

    public override void PhysicUpdateState()
    {
        base.PhysicUpdateState();
    }
}
