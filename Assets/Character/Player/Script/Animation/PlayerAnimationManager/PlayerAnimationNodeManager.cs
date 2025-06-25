using UnityEngine;

public partial class PlayerAnimationManager : INodeManager
{
    public INodeSelector startNodeSelector { get; set; }
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value ; }
    public NodeManagerBehavior nodeManagerBehavior { get ; set; }
    public NodeCombine upper_based_LayerCombineNode { get; set; }
    public NodeSelector upperLayerNodeSelector { get; set; }
    public NodeSelector basedLayerNodeSelector { get; set; }

    public void Awake()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
    }
    public void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=> true);

        upper_based_LayerCombineNode = new NodeCombine(()=> true);
        basedLayerNodeSelector = new NodeSelector(()=> true);
    }

    public void UpdateNode()
    {
       nodeManagerBehavior.UpdateNode(this);
    }
}
