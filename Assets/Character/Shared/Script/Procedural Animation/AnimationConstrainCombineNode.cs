using System;
using System.Collections.Generic;

public class AnimationConstrainCombineNode : AnimationConstrainNodeLeaf, INodeCombine
{
    public AnimationConstrainCombineNode(Func<bool> precondition) : base(precondition)
    {
        combineNodeActivate = new Dictionary<INode, bool>();
        combineNodeLeaf = new Dictionary<INode, INodeLeaf>();
        nodeCombineBehavior = new NodeCombineBehavior();
    }

    public NodeCombineBehavior nodeCombineBehavior { get ; set ; }
    public Dictionary<INode, bool> combineNodeActivate { get ; set ; }
    public Dictionary<INode, INodeLeaf> combineNodeLeaf { get ; set ; }

    public override void Enter()
    {
        nodeCombineBehavior.Enter(this);
        base.Enter();
    }

    public override void UpdateNode()
    {
        nodeCombineBehavior.Update(this);
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        nodeCombineBehavior.FixedUpdate(this);
        base.FixedUpdateNode();
    }


    public override void Exit()
    {
        nodeCombineBehavior.Exit(this);
        base.Exit();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }
    public void AddCombineNode(INode addCombineNode) => nodeCombineBehavior.AddCombineNode(this, addCombineNode);
    public void RemoveCombineNode(INode removeCombineNode) => nodeCombineBehavior.RemoveCombineNode(this, removeCombineNode);
   
}
