using System;
using System.Collections.Generic;
using UnityEngine;

public interface INodeSelector : INode
{
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public void AddtoChildNode(INode childNode);
    public void RemoveNode(INode childNode);
    public void FindingNode(out INodeLeaf nodeLeaf);
    
}
public class NodeSelectorBehavior
{

    private void PopulatePrecondition(out List<Func<bool>> nodeLeafReset, INode nodePosition,INodeSelector nodeSelector)
    {
        nodeLeafReset = new List<Func<bool>>();

        foreach (INode Node in nodeSelector.nodePrecondition.Keys)
        {
            if (Node == nodePosition)
                break;

            nodeLeafReset.Add(Node.preCondition);
        }

        nodeLeafReset.Add(() => !nodePosition.preCondition());

        if (nodePosition.parentNode == null)
            return;

        if(nodePosition.parentNode is INodeSelector selectorNode)
        {
            selectorNode.nodeSelectorBehavior.PopulatePrecondition(out List<Func<bool>> parentLeafReset, nodeSelector,nodeSelector);

            foreach (Func<bool> pReset in parentLeafReset)
            {
                nodeLeafReset.Add((Func<bool>)pReset);
            }
        }

      
    }

    public void AddtoChildNode(INode childNode,INodeSelector nodeSelector)
    {
        Dictionary<INode, Func<bool>> nodePrecondition = nodeSelector.nodePrecondition;
        List<INode> childNodes = nodeSelector.childNode;

        childNodes.Add(childNode);
        nodePrecondition.Add(childNode, childNode.preCondition);

        if (childNode is INodeSelector Selector)
            Selector.parentNode = nodeSelector;

        if (childNode is INodeLeaf leafNode)
        {
            PopulatePrecondition(out List<Func<bool>> isReset, childNode,nodeSelector);
            isReset.ForEach(resetCon => leafNode.isReset.Add(resetCon));
        }

    }
    public void RemoveChildNode(INode childNode,INodeSelector nodeSelector)
    {
        Dictionary<INode, Func<bool>> nodePrecondition = nodeSelector.nodePrecondition;
        List<INode> childNodes = nodeSelector.childNode;

        childNodes.Remove(childNode);
        nodePrecondition.Remove(childNode);
    }
    public bool FindingNode(out INodeLeaf leafNode,INodeSelector nodeSelector)
    {
        Dictionary<INode, Func<bool>> nodePrecondition = nodeSelector.nodePrecondition;
        List<INode> childNodes = nodeSelector.childNode;

        leafNode = null;

        foreach (INode node in nodeSelector.childNode)
        {
            if (node.Precondition() == false)
            {
                Debug.Log("Node " + nodeSelector + " -> " + node + " is false");
                continue;
            }


            Debug.Log("Node " + nodeSelector + " -> " + node);

            if (node is INodeLeaf)
            {
                leafNode = node as INodeLeaf;
                return true;
            }

            else if (node is INodeSelector SelectorNode)
            {
                SelectorNode.nodeSelectorBehavior.FindingNode(out leafNode,SelectorNode);
                return true;
            }
        }
        return false;

    }
}

