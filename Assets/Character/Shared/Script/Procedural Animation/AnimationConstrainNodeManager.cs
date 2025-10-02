using UnityEngine;
using UnityEngine.Animations.Rigging;

public abstract class AnimationConstrainNodeManager : MonoBehaviour,INodeManager,IInitializedAble
{
    protected INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public abstract INodeSelector startNodeSelector { get; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }

    public void Initialized()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
        InitailizedNode();
    }
    public virtual void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public abstract void InitailizedNode();
    

    public virtual void UpdateNode()
    {
        nodeManagerBehavior.UpdateNode(this);
    }
  
    protected virtual void Update()
    {
        this.UpdateNode();
    }
    protected virtual void FixedUpdate()
    {
        this.FixedUpdateNode();
    }
   


}
