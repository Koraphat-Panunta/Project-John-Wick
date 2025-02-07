using UnityEngine;

public interface INodeManager<NodeLeafType, NodeSelectorType>

    where NodeLeafType : INodeLeaf
    where NodeSelectorType : INodeSelector
{
    public NodeLeafType curNodeLeaf { get; set; }
    public NodeSelectorType startNodeSelector { get; set; }
    NodeManagerBehavior nodeManagerBehavior { get; set; }

    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
}

public interface INodeManager
{
    public INodeLeaf curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get; set; }
    NodeManagerBehavior nodeManagerBehavior { get; set; }

    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
}

public class NodeManagerBehavior
{

    public void UpdateNode(INodeManager nodeManager) 
    {
        if (nodeManager.curNodeLeaf.IsReset())
        {

            nodeManager.curNodeLeaf.Exit();
            nodeManager.curNodeLeaf = null;
            nodeManager.startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
            nodeManager.curNodeLeaf = nodeLeaf;
            nodeManager.curNodeLeaf.Enter();
        }

        if (nodeManager.curNodeLeaf != null)
            nodeManager.curNodeLeaf.UpdateNode();
    }
    public void FixedUpdateNode(INodeManager nodeManager)
    {
        if(nodeManager.curNodeLeaf != null)
            nodeManager.curNodeLeaf.FixedUpdateNode();
    }
    public void ChangeNodeManual(INodeLeaf curNodeLeaf,INodeLeaf nexNode)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = nexNode;
        nexNode.Enter();
    }
}
