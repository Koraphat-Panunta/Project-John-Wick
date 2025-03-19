using UnityEngine;
using System.Collections.Generic;
using System;

public interface INodeLeafTransitionAble : INodeLeaf
{
    public INodeManager nodeManager { get; set; }
    public Dictionary<INodeLeaf, bool> transitionAbleNode { get; set; }
    public bool Transitioning();
    public void AddTransitionNode(INodeLeaf nodeLeaf);
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
   
}
public class NodeLeafTransitionBehavior 
{
    public bool Transitioning(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
       Dictionary<INodeLeaf, bool> transitionAbleNode = nodeLeafTransitionAble.transitionAbleNode;

        foreach(INodeLeaf node in transitionAbleNode.Keys)
        {
            if (transitionAbleNode[node] &&
                node.Precondition())
            {
                nodeLeafTransitionAble.nodeManager.curNodeLeaf.Exit();
                nodeLeafTransitionAble.nodeManager.curNodeLeaf = node;
                nodeLeafTransitionAble.nodeManager.curNodeLeaf.Enter();
                return true;
            }
        }
        return false;
    }

    public void AddTransistionNode(INodeLeafTransitionAble nodeLeafTransitionAble,INodeLeaf addNode)
    {
        addNode.isReset.Add(() => !addNode.preCondition.Invoke());
        foreach (Func<bool> parentIsReset in nodeLeafTransitionAble.isReset)
        {
            addNode.isReset.Add(parentIsReset);
        }
        nodeLeafTransitionAble.transitionAbleNode.Add(addNode, false);

    }
    public void TransitionAbleAll(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
        if (nodeLeafTransitionAble.transitionAbleNode.Count <= 0)
            return;
        // Create a temporary list of keys to iterate over safely
        List<INodeLeaf> keys = new List<INodeLeaf>(nodeLeafTransitionAble.transitionAbleNode.Keys);

        // Modify dictionary using the stored keys
        foreach (INodeLeaf nodeLeaf in keys)
        {
            nodeLeafTransitionAble.transitionAbleNode[nodeLeaf] = true;
        }
    }
    public void DisableTransitionAbleAll(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
        if (nodeLeafTransitionAble.transitionAbleNode.Count <= 0)
            return;
        // Create a temporary list of keys to iterate over safely
        List<INodeLeaf> keys = new List<INodeLeaf>(nodeLeafTransitionAble.transitionAbleNode.Keys);

        // Modify dictionary using the stored keys
        foreach (INodeLeaf nodeLeaf in keys)
        {
            nodeLeafTransitionAble.transitionAbleNode[nodeLeaf] = false;
        }
    }
}
