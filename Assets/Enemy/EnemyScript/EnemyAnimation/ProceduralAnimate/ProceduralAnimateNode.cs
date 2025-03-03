using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProceduralAnimateNode : INode
{
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    public ProceduralAnimateNode(Func<bool> preCondition)
    {
        this.preCondition = preCondition;   
    }
    public bool Precondition()
    {
       return preCondition.Invoke();
    }
    
}
public class SelectorProceduralAnimateNode : ProceduralAnimateNode, INodeSelector
{
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }


    public SelectorProceduralAnimateNode(Func<bool> preCondition) : base(preCondition)
    {
        this.childNode = new List<INode>();
        this.nodePrecondition = new Dictionary<INode, Func<bool>>();
        this.nodeSelectorBehavior = new NodeSelectorBehavior();
    }

    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode,this);

    public void FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);
    
    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode,this);  
}

public abstract class ProceduralAnimateNodeLeaf : ProceduralAnimateNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    protected bool isComplete { get; set; }

    public ProceduralAnimateNodeLeaf(Func<bool> preCondition) : base(preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }
 
    public virtual void Enter()
    {
       this.isComplete = false;
    }

    public virtual void Exit()
    {
       
    }
    public virtual void UpdateNode()
    {

    }
    public virtual void FixedUpdateNode()
    {

    }

    public virtual bool IsComplete()
    {
        return isComplete;
    }

    public virtual bool IsReset() => nodeLeafBehavior.IsReset(this.isReset);
   
}
public class RestProceduralAnimateNodeLeaf : ProceduralAnimateNodeLeaf
{
    public RestProceduralAnimateNodeLeaf(Func<bool> preCondition) : base(preCondition)
    {
    }
}
