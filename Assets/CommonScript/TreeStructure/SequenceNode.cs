using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceNode : INode
{
    public abstract List<INode> children { get; set; }
    protected abstract Queue<ActionNode> actionNodes { get; set; }

    public abstract void FixedUpdate();


    public abstract bool IsReset();
    public void AddQueueActionNode(ActionNode actionNode)
    {
        this.actionNodes.Enqueue(actionNode);   
    }

    public bool PreCondition()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
    public void UpdateSequence()
    {
        ActionNode actionNode = actionNodes.Dequeue();
    }
}
