using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public interface INodeLeafTransitionAble : INodeLeaf
{
    public INodeManager nodeManager { get; set; }
    public Dictionary<INodeLeaf, bool> transitionAbleNode { get; set; }
    public bool Transitioning();
    public void AddTransitionNode(INode node);
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
        foreach (INodeLeaf nodeLeaf in nodeLeafTransitionAble.transitionAbleNode.Keys)
        {
            nodeLeafTransitionAble.transitionAbleNode[nodeLeaf] = true;
        }
    }
    public void DisableTransitionAbleAll(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
        foreach (INodeLeaf nodeLeaf in nodeLeafTransitionAble.transitionAbleNode.Keys)
        {
            nodeLeafTransitionAble.transitionAbleNode[nodeLeaf] = false;
        }
    }
}
