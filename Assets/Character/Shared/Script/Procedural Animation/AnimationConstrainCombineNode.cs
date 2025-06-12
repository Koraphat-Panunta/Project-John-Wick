using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConstrainCombineNode : AnimationConstrainNodeLeaf, INodeCombine
{
    public AnimationConstrainCombineNode(Func<bool> precondition) : base(precondition)
    {
        combineNodes = new List<INodeLeaf>();
        nodeCombineBehavior = new NodeCombineBehavior();
    }

    public List<INodeLeaf> combineNodes { get ; set ; }
    public NodeCombineBehavior nodeCombineBehavior { get ; set ; }

    

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



    public void AddCombineNode(INodeLeaf addCombineNode) => nodeCombineBehavior.AddCombineNode(this, addCombineNode);
    public void RemoveCombineNode(INodeLeaf removeCombineNode) => nodeCombineBehavior.RemoveCombineNode(this, removeCombineNode);
   


}
