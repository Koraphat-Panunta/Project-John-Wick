using System;
using UnityEngine;

public abstract class WeaponManuverNode : INode
{

    protected IWeaponAdvanceUser weaponAdvanceUser;
    public Func<bool> preCondition { get ; set ; }
    public INode parentNode { get ; set ; }
    public WeaponManuverNode(IWeaponAdvanceUser weaponAdvanceUser,Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        this.weaponAdvanceUser = weaponAdvanceUser;
    }

    public virtual bool Precondition() => preCondition.Invoke();

}
