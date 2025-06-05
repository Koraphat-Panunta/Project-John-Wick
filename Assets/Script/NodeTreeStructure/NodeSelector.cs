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
    public string nodeName { get; private set; }

    //public NodeSelector() : this("None declare name SelectorNode") { }
    //public NodeSelector(string nodeName)
    //{
    //    this.preCondition = () => true;
    //    this.nodeSelectorBehavior = new NodeSelectorBehavior();
    //    this.nodePrecondition = new Dictionary<INode, Func<bool>>();
    //    this.childNode = new List<INode>();
    //    this.nodeName = nodeName;
    //}
    public NodeSelector(Func<bool> preCondition) : this(preCondition, "None declare name SelectorNode") { }
    public NodeSelector(Func<bool> preCondition, string nodeName)
    {
        this.preCondition = preCondition;
        this.nodeSelectorBehavior = new NodeSelectorBehavior();
        this.nodePrecondition = new Dictionary<INode, Func<bool>>();
        this.childNode = new List<INode>();
        this.nodeName = nodeName;
    }

    public virtual void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode, this);

    public bool FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf,this);
    
    public bool Precondition() => preCondition.Invoke();
    
    public virtual void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode,this);    
   
}
