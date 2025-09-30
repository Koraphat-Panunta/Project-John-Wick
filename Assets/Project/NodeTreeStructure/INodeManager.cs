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
    NodeManagerBehavior nodeManagerBehavior { get; set; }

    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
    public INodeLeaf GetCurNodeLeaf() => _curNodeLeaf;
    public bool TryGetCurNodeLeaf<T>() where T : INodeLeaf => nodeManagerBehavior.TryGetCurNodeLeafAs<T>(this);
    public bool TryGetCurNodeLeaf<T>(out T nodeLeaf) where T : INodeLeaf => nodeManagerBehavior.TryGetCurNodeLeafAs<T>(out nodeLeaf, this);
    public T GetCurNodeLeafAs<T>() where T : INodeLeaf => nodeManagerBehavior.GetCurNodeLeafAs<T>(this);
    public void SetCurNodeLeaf(INodeLeaf nodeLeaf) => _curNodeLeaf = nodeLeaf;   
}

public class NodeManagerBehavior
{
    public INode errorNode { get;private set; }
    public void UpdateNode(INodeManager nodeManager) 
    {
        if (nodeManager.GetCurNodeLeaf().IsReset())
        {
            //nodeManager.GetCurNodeLeaf().Exit();
            //nodeManager.SetCurNodeLeaf(null);
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
    public void ChangeNodeManual(INodeManager nodeManager,INodeLeaf nexNode)
    {
        nodeManager.GetCurNodeLeaf().Exit();
        nodeManager.SetCurNodeLeaf(nexNode);
        nodeManager.GetCurNodeLeaf().Enter();
    }
    public bool TryGetCurNodeLeafAs<T>(INodeManager nodeManager) where T : INodeLeaf
    {

        if (nodeManager.GetCurNodeLeaf() == null)
            return false;

        if (NodeBehavior.TryGetRootNodeAs<T>(nodeManager.GetCurNodeLeaf(), out T rootNodeLeaf))
            return true;

        return false;
    }
    public bool TryGetCurNodeLeafAs<T>(out T nodeLeaf,INodeManager nodeManager) where T : INodeLeaf
    {
        nodeLeaf = default(T);

        if (nodeManager.GetCurNodeLeaf() == null)
            return false;

        if(NodeBehavior.TryGetRootNodeAs<T>(nodeManager.GetCurNodeLeaf(),out nodeLeaf))
            return true;
        return false;

    }
    public T GetCurNodeLeafAs<T>(INodeManager nodeManager) where T : INodeLeaf
    {
       if(this.TryGetCurNodeLeafAs<T>(out T nodeLeaf,nodeManager))
        {
            return nodeLeaf;
        }
        return nodeLeaf;
    }

}
