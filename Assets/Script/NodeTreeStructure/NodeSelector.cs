using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : INodeSelector
{
    public List<INode> childNode { get; set ; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get ; set ; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    public NodeSelector(Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        this.nodeSelectorBehavior = new NodeSelectorBehavior();
        this.nodePrecondition = new Dictionary<INode, Func<bool>>();
        this.childNode = new List<INode>();
    }

    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode, this);

    public void FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf,this);
    
    public bool Precondition() => preCondition.Invoke();
    
    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode,this);    
   
}
