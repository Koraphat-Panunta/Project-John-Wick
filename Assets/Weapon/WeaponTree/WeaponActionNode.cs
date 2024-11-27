using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class WeaponActionNode : WeaponNode
{
    public override List<WeaponNode> childNode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    protected override WeaponTreeManager weaponTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    protected override WeaponBlackBoard blackBoard { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    protected override Func<bool> preCondidtion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public abstract List<WeaponNode> SubNode { get; set; }
    public abstract void Enter();
    public abstract void Exit();
    public WeaponActionNode(WeaponTreeManager weaponTree) : base(weaponTree)
    {
        this.weaponTree = weaponTree;

    }
    public abstract bool IsComplete();
    protected bool FindSubNode(out WeaponNode node)
    {
        node = null;
        foreach (WeaponNode n in SubNode)
        {
            if (n.PreCondition() == true)
                node = n;
            break;
        }
        return node != null;
    }


}
