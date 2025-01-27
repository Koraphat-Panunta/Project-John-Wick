using System;
using UnityEngine;

public abstract class WeaponManuverNode 
{
    public Func<bool> preCpndition { get; protected set; }
    protected IWeaponAdvanceUser weaponAdvanceUser;
    public WeaponManuverNode(IWeaponAdvanceUser weaponAdvanceUser,Func<bool> preCondition)
    {
        this.preCpndition = preCondition;
        this.weaponAdvanceUser = weaponAdvanceUser;
    }
    public virtual bool Precondition()
    {
        if(preCpndition != null)
            return preCpndition.Invoke();
        return false;
    }
}
