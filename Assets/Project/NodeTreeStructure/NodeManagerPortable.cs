using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeManagerPortable : INodeManager
{
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => this.curNodeLeaf; set => this.curNodeLeaf = value; }
    public List<INodeManager> _parallelNodeManahger { get; set ; }

    public RestNodeLeaf restNodeLeaf;

    public NodeManagerPortable()
    {
        startNodeSelector = new NodeSelector(() => true);

        _nodeManagerBehavior = new NodeManagerBehavior();
        this._parallelNodeManahger = new List<INodeManager>();
    }

    public void FixedUpdateNode() => _nodeManagerBehavior.FixedUpdateNode(this);
    public void UpdateNode() => _nodeManagerBehavior.UpdateNode(this);

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
            _nodeManagerBehavior.SearchingNewNode(this);
        }
        catch
        {
            throw new System.Exception("NodeManagerPortable Initialzed Been  Corrupt");
        }
    }
}
