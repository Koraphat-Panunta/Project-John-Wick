using UnityEngine;

public interface INodeManager <NodeLeafType ,NodeSelectorType > 

    where NodeLeafType:INodeLeaf 
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
    public INodeManager startNodeSelector { get; set; }
    NodeManagerBehavior nodeManagerBehavior { get; set; }

    public void UpdateNode();
    public void FixedUpdateNode();
    public void InitailizedNode();
}
public class NodeManagerBehavior
{
    public void UpdateNode(INodeLeaf curNodeLeaf,INodeSelector startNodeSelector) 
    {
        if (curNodeLeaf.IsReset())
        {
            Debug.Log(curNodeLeaf + "isReset");

            curNodeLeaf.Exit();
            curNodeLeaf = null;
            startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
            curNodeLeaf = nodeLeaf;
            curNodeLeaf.Enter();
        }

        if (curNodeLeaf != null)
            curNodeLeaf.UpdateNode();
    }
    public void FixedUpdateNode(INodeLeaf curNodeLeaf)
    {
        if(curNodeLeaf != null)
            curNodeLeaf.FixedUpdateNode();
    }
    public void ChangeNodeManual(INodeLeaf curNodeLeaf,INodeLeaf nexNode)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = nexNode;
        nexNode.Enter();
    }
}
