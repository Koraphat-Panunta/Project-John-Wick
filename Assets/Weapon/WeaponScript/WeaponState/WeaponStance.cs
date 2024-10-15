using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponStance 
{
    protected Weapon _weapon;
    public WeaponStance(Weapon weapon)
    {
        this._weapon = weapon;
    }
    protected virtual void Start()
    {
    }
    public virtual void EnterState()
    {

    }

    public virtual void ExitState()
    {
    }

    public virtual void WeaponStanceFixedUpdate(WeaponStanceManager weaponStanceManager)
    {

    }

    public virtual void WeaponStanceUpdate(WeaponStanceManager weaponStanceManager)
    {

    }
}
