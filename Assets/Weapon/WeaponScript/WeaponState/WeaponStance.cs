using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponStance 
{
    public Animator animator { get; protected set;}
    public WeaponStance()
    {

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
