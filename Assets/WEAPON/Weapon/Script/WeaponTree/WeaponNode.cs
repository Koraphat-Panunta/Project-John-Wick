using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class WeaponNode : INode
{
    public  Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    protected Weapon Weapon { get; set; }

    public WeaponNode(Weapon weapon,Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        this.Weapon = weapon;
    }

  

    public bool Precondition() => preCondition.Invoke();
   
    

    
}
