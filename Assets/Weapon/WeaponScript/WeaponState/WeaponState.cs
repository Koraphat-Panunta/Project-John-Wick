using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponState 
{
    public WeaponState() 
    {
        
    }
    public virtual void EnterState()
    {
    }

    public virtual void ExitState()
    {
    }

    public virtual void WeaponStateUpdate(WeaponStateManager weaponStateManager)
    {
    }

    public virtual void WeaponStateFixedUpdate(WeaponStateManager weaponStateManager)
    {
    }
}
