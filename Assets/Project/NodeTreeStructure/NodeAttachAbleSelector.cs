using System;
using System.Collections.Generic;

public class NodeAttachAbleSelector : INodeSelector
{
    public List<INode> childNode { get ; set ; }
    public INode attachingNode => childNode[0];
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }
    public Func<bool> preCondition
    {
        get
        {
            return () =>
            {
                if (this.childNode.Count <= 0)
                    return false;

                return this.childNode[0].Precondition();
            };
        }
        set { }
    }
    public INode parentNode { get; set; }
    public INodeLeaf curNodeLeaf { get; set ; }

    public NodeAttachAbleSelector()
    {
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
        nodeSelectorBehavior = new NodeSelectorBehavior();
    }

    public void AddtoChildNode(INode childNode)
    {
        if(this.childNode.Count > 0)
        {
            this.RemoveNode(this.childNode[0]);
        }

        if(this.childNode.Count > 0)
        {
            throw new Exception("Cannot Attach chlidNode "+childNode+" to this node "+this);
        }
        nodeSelectorBehavior.AddtoChildNode(childNode,this);
    }

    public bool FindingNode(out INodeLeaf nodeLeaf)
    {
        return nodeSelectorBehavior.FindingNode(out nodeLeaf,this);
    }

    public bool Precondition()
    {
        return preCondition.Invoke();
    }

    public void RemoveNode(INode childNode)
    {
       nodeSelectorBehavior.RemoveChildNode(childNode,this);
    }
}
