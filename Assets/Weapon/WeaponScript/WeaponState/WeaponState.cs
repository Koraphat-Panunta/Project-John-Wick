using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponState 
{
    protected Weapon _weapon;
    public WeaponState(Weapon weapon) 
    {
        this._weapon = weapon;
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
