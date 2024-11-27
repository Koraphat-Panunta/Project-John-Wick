using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponNode 
{
    public WeaponNode(WeaponTreeManager weaponTree)
    {
        this.weaponTree = weaponTree;
    }

    protected abstract WeaponTreeManager weaponTree { get; set; }
    protected abstract WeaponBlackBoard blackBoard { get; set; }
    public abstract List<WeaponNode> childNode { get ; set ; }
    protected abstract Func<bool> preCondidtion { get; set; }

    public abstract void FixedUpdate();
    public abstract void Update();

    public abstract bool IsReset();


    public abstract bool PreCondition();
    

    
}
