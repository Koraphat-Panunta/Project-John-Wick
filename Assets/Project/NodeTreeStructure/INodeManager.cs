using System.Collections.Generic;
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
    protected INodeLeaf _curNodeLeaf { get; set; } 
    public INodeSelector startNodeSelector { get; set; }
    NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public List<INodeManager> _parallelNodeManahger { get; set; }
    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
    public INodeLeaf GetCurNodeLeaf() => _curNodeLeaf;
    public void AddParallelNodeManager(INodeManager addNodeManager) => _nodeManagerBehavior.AddParallelNodeManager(this, addNodeManager);
    public bool TryGetCurNodeLeaf<T>() where T : INodeLeaf => _nodeManagerBehavior.TryGetCurNodeLeafAs<T>(this);
    public bool TryGetCurNodeLeaf<T>(out T nodeLeaf) where T : INodeLeaf => _nodeManagerBehavior.TryGetCurNodeLeafAs<T>(out nodeLeaf, this);
    public void SetCurNodeLeaf(INodeLeaf nodeLeaf) => _curNodeLeaf = nodeLeaf;   
}

public class NodeManagerBehavior
{
    
    public INode errorNode { get;private set; }
    public void UpdateNode(INodeManager nodeManager) 
    {
        if (nodeManager.GetCurNodeLeaf().IsReset())
            SearchingNewNode(nodeManager);
        
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
            if (nodeManager.GetCurNodeLeaf() != null)
                nodeManager.GetCurNodeLeaf().Exit();
            nodeManager.SetCurNodeLeaf(nodeLeaf);
            nodeManager.GetCurNodeLeaf().Enter();
        }
        else
        {
            nodeManager.startNodeSelector.nodeSelectorBehavior.FindingNodeLast(out INode node, nodeManager.startNodeSelector);
            errorNode = node;
        }

    }

    public bool TryGetCurNodeLeafAs<T>(INodeManager nodeManager) where T : INodeLeaf
    {
        return this.TryGetCurNodeLeafAs<T>(out T node, nodeManager);
    }
    public bool TryGetCurNodeLeafAs<T>(out T nodeLeaf,INodeManager nodeManager) where T : INodeLeaf
    {
        nodeLeaf = default(T);

        if (nodeManager.GetCurNodeLeaf() is T curNode)
        {
            nodeLeaf = curNode;
            return true;
        }
        else
        {
            if(nodeManager._parallelNodeManahger.Count <= 0)
                return false;

            for (int i = 0; i < nodeManager._parallelNodeManahger.Count; i++)
            {
                if (nodeManager._parallelNodeManahger[i].GetCurNodeLeaf() is T curParallelNodeLeaf)
                {
                    nodeLeaf = curParallelNodeLeaf;
                    return true;
                }
            }
        }
        return false;
    }

    public void AddParallelNodeManager(INodeManager nodeManager,INodeManager addNodeManager)
    {
        nodeManager._parallelNodeManahger.Add(addNodeManager);
    }

}
