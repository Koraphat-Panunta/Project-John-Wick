using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeSequence : INodeSequence
{
    public INodeLeaf curNodeLeaf { get; set; }
    public List<INodeLeaf> childNode { get ; set ; }
    public int curNodeIndex { get; set; }
    public NodeSequenceBehavior nodeSequenceBehavior { get; set; }
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public NodeSequence(Func<bool> preCondition)
    {
        childNode = new List<INodeLeaf>();
        nodeSequenceBehavior = new NodeSequenceBehavior();
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
        this.preCondition = preCondition;
    }
    public void AddChildNode(INodeLeaf nodeLeaf)
    {
        nodeSequenceBehavior.AddNode(this, nodeLeaf);
    }

    public void Enter()
    {
        nodeSequenceBehavior.Enter(this);
    }

    public void Exit()
    {
        nodeSequenceBehavior.Exit(this);
    }

    public void FixedUpdateNode()
    {
        nodeSequenceBehavior.FixedUpdateNodeSequencee(this);
    }

    public bool IsComplete()
    {
       return  nodeSequenceBehavior.IsComplete(this);
    }

    public bool IsReset()
    {
        return nodeSequenceBehavior.IsReset(this);
    }

    public bool Precondition()
    {
        return preCondition.Invoke();
    }

    public void UpdateNode()
    {
        nodeSequenceBehavior.UpdateNodeSequence(this);
    }
}
