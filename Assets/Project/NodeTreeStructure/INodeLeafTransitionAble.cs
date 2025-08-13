using UnityEngine;
using System.Collections.Generic;
using System;

public interface INodeLeafTransitionAble : INodeLeaf
{
    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public bool TransitioningCheck();
    public void AddTransitionNode(INode node);
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
   
}
public class NodeLeafTransitionBehavior 
{
    public bool TransitioningCheck(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
       Dictionary<INode, bool> transitionAbleNode = nodeLeafTransitionAble.transitionAbleNode;

        foreach(INode node in transitionAbleNode.Keys)
        {
            if (transitionAbleNode[node] &&
                node.Precondition())
            {
                if(node is INodeLeaf nodeLeaf)
                {
                    nodeLeafTransitionAble.nodeManager.GetCurNodeLeaf().Exit();
                    nodeLeafTransitionAble.nodeManager.SetCurNodeLeaf(nodeLeaf);
                    nodeLeafTransitionAble.nodeManager.GetCurNodeLeaf().Enter();
                }
                else if(node is INodeSelector nodeSelector)
                {
                    nodeSelector.FindingNode(out INodeLeaf _nodeLeaf);
                    nodeLeafTransitionAble.nodeManager.GetCurNodeLeaf().Exit();
                    nodeLeafTransitionAble.nodeManager.SetCurNodeLeaf(_nodeLeaf);
                    nodeLeafTransitionAble.nodeManager.GetCurNodeLeaf().Enter();
                }

                return true;
            }
        }
        return false;
    }

    public void AddTransistionNode(INodeLeafTransitionAble nodeLeafTransitionAble,INode addNode)
    {
        if (addNode is INodeLeaf nodeLeaf)
        {
            nodeLeaf.isReset.Add(() => !nodeLeaf.preCondition.Invoke());
            foreach (Func<bool> parentIsReset in nodeLeafTransitionAble.isReset)
            {
                nodeLeaf.isReset.Add(parentIsReset);
            }
        }
        nodeLeafTransitionAble.transitionAbleNode.Add(addNode, false);

    }
    public void TransitionAbleAll(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
        if (nodeLeafTransitionAble.transitionAbleNode.Count <= 0)
            return;
        // Create a temporary list of keys to iterate over safely
        List<INode> keys = new List<INode>(nodeLeafTransitionAble.transitionAbleNode.Keys);

        // Modify dictionary using the stored keys
        foreach (INode node in keys)
        {
            nodeLeafTransitionAble.transitionAbleNode[node] = true;
        }
    }
    public void DisableTransitionAbleAll(INodeLeafTransitionAble nodeLeafTransitionAble)
    {
        if (nodeLeafTransitionAble.transitionAbleNode.Count <= 0)
            return;
        // Create a temporary list of keys to iterate over safely
        List<INode> keys = new List<INode>(nodeLeafTransitionAble.transitionAbleNode.Keys);

        // Modify dictionary using the stored keys
        foreach (INode node in keys)
        {
            nodeLeafTransitionAble.transitionAbleNode[node] = false;
        }
    }
}
