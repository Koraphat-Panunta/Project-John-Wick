using System;
using UnityEngine;

public interface INodeManager<NodeLeafType, NodeSelectorType>

    where NodeLeafType : INodeLeaf
    where NodeSelectorType : INodeSelector
{
    public NodeLeafType curNodeLeaf { get; set; }
    public NodeSelectorType startNodeSelector { get; set; }
    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
}

public interface INodeManager
{
    protected INodeLeaf curNodeLeaf { get; set; } 
    public INodeSelector startNodeSelector { get; set; }
    NodeManagerBehavior nodeManagerBehavior { get; set; }

    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
    public INodeLeaf GetCurNodeLeaf() => curNodeLeaf;
    public bool TryGetCurNodeLeaf<T>() where T : INodeLeaf => nodeManagerBehavior.TryGetCurNodeLeafAs<T>(this);
    public bool TryGetCurNodeLeaf<T>(out T nodeLeaf) where T : INodeLeaf => nodeManagerBehavior.TryGetCurNodeLeafAs<T>(out nodeLeaf, this);
    public void SetCurNodeLeaf(INodeLeaf nodeLeaf) => curNodeLeaf = nodeLeaf;   
}

public class NodeManagerBehavior
{
    public INode errorNode { get;private set; }
    public void UpdateNode(INodeManager nodeManager) 
    {
        if (nodeManager.GetCurNodeLeaf().IsReset())
        {
            nodeManager.GetCurNodeLeaf().Exit();
            nodeManager.SetCurNodeLeaf(null);
            SearchingNewNode(nodeManager);
        }

        if (nodeManager.GetCurNodeLeaf() != null)
            nodeManager.GetCurNodeLeaf().UpdateNode();
    }
    public void FixedUpdateNode(INodeManager nodeManager)
    {
        if(nodeManager.GetCurNodeLeaf() != null)
            nodeManager.GetCurNodeLeaf().FixedUpdateNode();
    }
    public void SearchingNewNode(INodeManager nodeManager)
    {
       if(nodeManager.startNodeSelector.FindingNode(out INodeLeaf nodeLeaf))
        {
            nodeManager.SetCurNodeLeaf(nodeLeaf);
            nodeManager.GetCurNodeLeaf().Enter();
        }
        else
        {
            nodeManager.startNodeSelector.nodeSelectorBehavior.FindingNodeLast(out INode node, nodeManager.startNodeSelector);
            errorNode = node;
        }

    }
    public void ChangeNodeManual(INodeManager nodeManager,INodeLeaf nexNode)
    {
        nodeManager.GetCurNodeLeaf().Exit();
        nodeManager.SetCurNodeLeaf(nexNode);
        nodeManager.GetCurNodeLeaf().Enter();
    }
    public bool TryGetCurNodeLeafAs<T>(INodeManager nodeManager) where T : INodeLeaf
    {
        if(nodeManager.GetCurNodeLeaf() == null)
            return false;

        if (nodeManager.GetCurNodeLeaf() is INodeCombine nodeCombine)
        {
            if (nodeCombine is T)
                return true;
            

            foreach (INode childNodeCombine in nodeCombine.combineNodeActivate.Keys)
            {
                if (nodeCombine.combineNodeActivate[childNodeCombine] == false)
                    continue;

                if (childNodeCombine is INodeLeaf childNodeCombineLeaf
                    && childNodeCombineLeaf is T)
                    return true;
                

                if (childNodeCombine is INodeSelector childNodeCombineSelector
                    && childNodeCombineSelector.curNodeLeaf is T)
                    return true;
                
            }


        }
        else if (nodeManager.GetCurNodeLeaf() is INodeLeaf outNodeLeaf)
        {
            if (outNodeLeaf is T)
                return true;
            
        }
        return false;
    }
    public bool TryGetCurNodeLeafAs<T>(out T nodeLeaf,INodeManager nodeManager) where T : INodeLeaf
    {
        nodeLeaf = default(T);

        if (nodeManager.GetCurNodeLeaf() == null)
            return false;

        if (nodeManager.GetCurNodeLeaf() is INodeCombine nodeCombine)
        {
            if(nodeCombine is T)
            {
                nodeLeaf = (T)nodeCombine;
                return true;
            }

            foreach(INode childNodeCombine in nodeCombine.combineNodeActivate.Keys)
            {
                if (nodeCombine.combineNodeActivate[childNodeCombine] == false)
                    continue;

                if (childNodeCombine is INodeLeaf childNodeCombineLeaf
                    && childNodeCombineLeaf is T)
                {
                    nodeLeaf = (T)childNodeCombineLeaf;
                    return true;
                }

                if (childNodeCombine is INodeSelector childNodeCombineSelector
                    && childNodeCombineSelector.curNodeLeaf is T)
                {
                    nodeLeaf = (T)childNodeCombineSelector.curNodeLeaf;
                    return true;
                }
            }

           
        }
        else if(nodeManager.GetCurNodeLeaf() is INodeLeaf outNodeLeaf)
        {
            if (outNodeLeaf is T)
            {
                nodeLeaf = (T)outNodeLeaf;
                return true;
            }
        }
        return false;
    }

}
