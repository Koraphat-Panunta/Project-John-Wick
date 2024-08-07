using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponStance 
{
    protected WeaponSocket weaponSocket;
    public Animator animator { get; private set;}
    public WeaponStance()
    {

    }
    protected virtual void Start()
    {
        animator = weaponSocket.GetAnimator();
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
