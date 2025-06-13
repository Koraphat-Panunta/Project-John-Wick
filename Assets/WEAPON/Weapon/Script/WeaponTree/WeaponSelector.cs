using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : WeaponNode, INodeSelector
{
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public WeaponSelector(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
        childNode = new List<INode>();
        nodeSelectorBehavior = new NodeSelectorBehavior();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
    }

    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode, this);

    public bool FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);
 
    public void RemoveNode(INode childNode)=> nodeSelectorBehavior.RemoveChildNode(childNode, this);
    
}
