using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeManagerPortable : INodeManager
{
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => this.curNodeLeaf; set => this.curNodeLeaf = value; }
    public List<INodeManager> parallelNodeManahger { get; set ; }

    public RestNodeLeaf restNodeLeaf;

    public NodeManagerPortable()
    {
        startNodeSelector = new NodeSelector(() => true);

        nodeManagerBehavior = new NodeManagerBehavior();
        this.parallelNodeManahger = new List<INodeManager>();
    }

    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);
    public void UpdateNode() => nodeManagerBehavior.UpdateNode(this);

    public void InitialzedOuterNode(Action initialzedOuterNode)
    {
        initialzedOuterNode.Invoke();
        this.InitailizedNode();
    }
    public void InitailizedNode()
    {
        restNodeLeaf = new RestNodeLeaf(() => true);
        startNodeSelector.AddtoChildNode(restNodeLeaf);
        try
        {
            nodeManagerBehavior.SearchingNewNode(this);
        }
        catch
        {
            throw new System.Exception("NodeManagerPortable Initialzed Been  Corrupt");
        }
    }
}
