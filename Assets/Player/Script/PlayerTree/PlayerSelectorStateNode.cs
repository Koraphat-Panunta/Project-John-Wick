using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectorStateNode : PlayerStateNode , INodeSelector
{
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public PlayerSelectorStateNode(Player player,Func<bool> preCondition) : base(player,preCondition)
    {
        nodeSelectorBehavior = new NodeSelectorBehavior();
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
    }


    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode, this);
    public void FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);
    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode, this);
}
