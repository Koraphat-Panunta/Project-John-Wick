using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class WeaponActionNode : WeaponNode
{
    public override List<WeaponNode> childNode { get ; set ; }
    protected override Func<bool> preCondidtion { get; set; }
    public abstract void Enter();
    public abstract void Exit();
    public WeaponActionNode(Weapon weapon) : base(weapon)
    {
       
    }
    public abstract bool IsComplete();
    //protected bool FindSubNode(out WeaponNode node)
    //{
    //    node = null;
    //    foreach (WeaponNode n in childNode)
    //    {
    //        if (n.PreCondition() == true)
    //            node = n;
    //        break;
    //    }
    //    return node != null;
    //}


}
