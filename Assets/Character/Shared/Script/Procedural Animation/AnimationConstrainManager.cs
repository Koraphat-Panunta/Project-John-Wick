using UnityEngine;

public abstract class AnimationConstrainManager : MonoBehaviour,INodeManager
{
    protected INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public abstract INodeSelector startNodeSelector { get; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }

    public virtual void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public abstract void InitailizedNode();
    

    public virtual void UpdateNode()
    {
        nodeManagerBehavior.UpdateNode(this);
    }
    protected virtual void Awake()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
        InitailizedNode();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        this.UpdateNode();
    }
    protected virtual void FixedUpdate()
    {
        this.FixedUpdateNode();
    }

}
