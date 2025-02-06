using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManuverSelectorNode : WeaponManuverNode,INodeSelector
{

    public List<INode> childNode { get ; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public WeaponManuverSelectorNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>(); 
        nodeSelectorBehavior = new NodeSelectorBehavior();
    }

    
    public void FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf,this);
 
    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode, this);

    public void AddtoChildNode(INode childNode )=> nodeSelectorBehavior.AddtoChildNode(childNode as INode, this);

}
