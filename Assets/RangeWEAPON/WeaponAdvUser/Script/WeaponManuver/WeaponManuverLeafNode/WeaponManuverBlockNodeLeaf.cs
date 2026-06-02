using System;
using UnityEngine;

public class WeaponManuverBlockNodeLeaf : WeaponManuverLeafNode
{
    protected Transform anhorCast;

    protected float castLenght;

    protected Vector3 castDir => (weaponAdvanceUser._pointingPos - anhorCast.transform.position).normalized;

    public WeaponManuverBlockNodeLeaf(IRangeWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
    }

    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override void UpdateNode()
    {
        
    }

    public bool IsBlock()
    {
        return false;
    }
}
