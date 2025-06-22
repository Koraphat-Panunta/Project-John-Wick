using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeCombine : INodeCombine
{
    public Dictionary<INode, bool> combineNodeActivate { get ; set ; }
    public Dictionary<INode, INodeLeaf> combineNodeLeaf { get ; set ; }
    public NodeCombineBehavior nodeCombineBehavior { get; set; }
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    private bool isOverrideReset = false;
    private Func<bool> overriderReset;

    public NodeCombine(Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        combineNodeActivate = new Dictionary<INode, bool>();
        combineNodeLeaf = new Dictionary<INode, INodeLeaf>();
        nodeCombineBehavior = new NodeCombineBehavior();
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }
    public NodeCombine(Func<bool> preCondition, Func<bool> isReset):this(preCondition)
    {
        isOverrideReset = true;
        overriderReset = isReset;
    }

    public void AddCombineNode(INode addCombineNode) => nodeCombineBehavior.AddCombineNode(this,addCombineNode);


    public void Enter() => nodeCombineBehavior.Enter(this);

    public void Exit() => nodeCombineBehavior.Exit(this);

    public void FixedUpdateNode() => nodeCombineBehavior.FixedUpdate(this);
   
    public bool IsComplete() => true;
    

    public bool IsReset()
    {
        if (isOverrideReset)
            return overriderReset.Invoke();
        else
            return nodeLeafBehavior.IsReset(isReset);
    }

    public bool Precondition()
    {
        return preCondition.Invoke();
    }

    public void RemoveCombineNode(INode removeCombineNode) => nodeCombineBehavior.RemoveCombineNode(this,removeCombineNode);


    public void UpdateNode() => nodeCombineBehavior.Update(this);
  
}
