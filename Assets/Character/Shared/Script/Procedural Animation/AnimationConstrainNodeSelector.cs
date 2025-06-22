using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConstrainNodeSelector : AnimationConstrainNode, INodeSelector
{
    public List<INode> childNode { get ; set ; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get ; set ; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }
    public INodeLeaf curNodeLeaf { get ; set ; }

    public AnimationConstrainNodeSelector(Func<bool> precondition) : base(precondition)
    {
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
        nodeSelectorBehavior = new NodeSelectorBehavior();
    }

    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode,this);
    

    public bool FindingNode(out INodeLeaf nodeLeaf)=>nodeSelectorBehavior.FindingNode(out nodeLeaf,this);
   

    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode,this);
   
}
