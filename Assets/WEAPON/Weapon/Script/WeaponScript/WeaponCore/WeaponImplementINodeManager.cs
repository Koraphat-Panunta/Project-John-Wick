using System.Collections.Generic;
using UnityEngine;

public partial class Weapon : INodeManager
{

    public NodeManagerBehavior _nodeManagerBehavior { get => this.nodeManagerBehavior; set => this.nodeManagerBehavior = value; }
    private NodeManagerBehavior nodeManagerBehavior = new NodeManagerBehavior();
    public List<INodeManager> _parallelNodeManahger { get => this.parallelNodeManahger; set => this.parallelNodeManahger = value; }
    public List<INodeManager> parallelNodeManahger = new List<INodeManager>();

    public abstract INodeSelector startNodeSelector { get; set; }
    public abstract WeaponRestNodeLeaf restNode { get; set; }
    public abstract NodeSelector _reloadSelecotrOverriden { get; }

    public NodeComponentManager nodeComponentManager;

    private INodeLeaf curNodeLeaf { get; set; }
    INodeLeaf INodeManager._curNodeLeaf { get => this.curNodeLeaf; set  => this.curNodeLeaf = value; }


    public virtual void InitailizedNode()
    {
        this.nodeComponentManager = new NodeComponentManager();

        this._objectIsBeenThrowNodeLeaf = new ObjectIsBeenThrowNodeLeaf(()=> _isTriggerThrow,this);

        nodeComponentManager.AddNode(this._objectIsBeenThrowNodeLeaf);
    }

    public virtual void FixedUpdateNode()
    {
        this._nodeManagerBehavior.FixedUpdateNode(this);
        this.nodeComponentManager.FixedUpdate();
    }

   

    public virtual void UpdateNode()
    {
        this._nodeManagerBehavior.UpdateNode(this);
        this.nodeComponentManager.Update();
    }
}
