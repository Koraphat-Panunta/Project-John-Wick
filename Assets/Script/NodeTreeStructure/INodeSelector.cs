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
    public bool FindingNode(out INodeLeaf nodeLeaf);

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

            //Debug.Log("Add isReset form Node " + Node + " to " + nodePosition);

            nodeLeafReset.Add(Node.preCondition);
        }

        nodeLeafReset.Add(() => !nodePosition.preCondition());

        //Debug.Log("Add isReset form Node " + nodePosition);

        if (nodeSelector.parentNode == null)
            return;

        if(nodeSelector.parentNode is INodeSelector selectorNode)
        {
            selectorNode.nodeSelectorBehavior.PopulatePrecondition(out List<Func<bool>> parentLeafReset, nodeSelector, selectorNode);

            foreach (Func<bool> pReset in parentLeafReset)
            {
                nodeLeafReset.Add((Func<bool>)pReset);
            }
        }

      
    }

    public virtual void AddtoChildNode(INode childNode,INodeSelector nodeSelector)
    {
        Dictionary<INode, Func<bool>> nodePrecondition = nodeSelector.nodePrecondition;
        List<INode> childNodes = nodeSelector.childNode;

        childNodes.Add(childNode);
        nodePrecondition.Add(childNode, childNode.preCondition);

        if (childNode is INodeSelector Selector)
            Selector.parentNode = nodeSelector;

        if (childNode is INodeLeaf leafNode)
        {
            childNode.parentNode = nodeSelector;
            PopulatePrecondition(out List<Func<bool>> isReset, childNode,nodeSelector);
            isReset.ForEach(resetCon => leafNode.isReset.Add(resetCon));
        }

    }
    public virtual void RemoveChildNode(INode childNode,INodeSelector nodeSelector)
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
                continue;
            }

            //Debug.Log("Node " + nodeSelector + " -> " + node);

            if (node is INodeLeaf)
            {
                //Debug.Log("Node " + node + " isNodeLeaf ");
                leafNode = node as INodeLeaf;
                return true;
            }
            else if (node is INodeSelector SelectorNode)
            {
                //Debug.Log("Node " + node + " isNodeSelector ");
                SelectorNode.nodeSelectorBehavior.FindingNode(out leafNode,SelectorNode);
                return true;
            }
            //Debug.Log("Node " + node + " not both ");
        }
        return false;

    }
    public void FindingNodeLast(out INode node,INodeSelector nodeSelector) // Call for debug from the end path
    {
        node = null;

        Dictionary<INode, Func<bool>> nodePrecondition = nodeSelector.nodePrecondition;
        List<INode> childNodes = nodeSelector.childNode;

        node = null;

        foreach (INode Node in nodeSelector.childNode)
        {
            if (Node.Precondition() == false)
            {
                continue;
            }

            //Debug.Log("Node " + nodeSelector + " -> " + node);

            if (Node is INodeLeaf)
            {
                //Debug.Log("Node " + node + " isNodeLeaf ");
                node = Node as INodeLeaf;
                return;
            }
            else if (Node is INodeSelector SelectorNode)
            {
                //Debug.Log("Node " + node + " isNodeSelector ");
                SelectorNode.nodeSelectorBehavior.FindingNodeLast(out node, SelectorNode);
                return;
            }
            //Debug.Log("Node " + node + " not both ");
        }
        return;
    }
   
}

