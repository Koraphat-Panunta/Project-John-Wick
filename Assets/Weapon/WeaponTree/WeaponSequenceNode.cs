using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSequenceNode : WeaponLeafNode, INodeSequence
{
    public INodeLeaf curNodeLeaf { get; set; }
    public List<INodeLeaf> childNode { get; set; }
    public int curNodeIndex { get; set; }
    public NodeSequenceBehavior nodeSequenceBehavior { get; set; }

    public WeaponSequenceNode(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
        childNode = new List<INodeLeaf>();
        nodeSequenceBehavior = new NodeSequenceBehavior();
    }
    public override void Enter()
    {
        this.nodeSequenceBehavior.Enter(this);
    }

    public override void Exit()
    {
        this.nodeSequenceBehavior.Exit(this);
    }
    public override void UpdateNode()
    {
        this.nodeSequenceBehavior.UpdateNodeSequence(this);
    }
    public override void FixedUpdateNode()
    {
        this.nodeSequenceBehavior.FixedUpdateNodeSequencee(this);
    }

    public override bool IsComplete()
    {
        if (this.nodeSequenceBehavior.IsComplete(this) == true)
            return true;

        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if(this.nodeSequenceBehavior.IsReset(this) == true)
            return true;

        return base.IsReset();
    }

    public void AddChildNode(INodeLeaf nodeLeaf) => this.nodeSequenceBehavior.AddNode(this, nodeLeaf);
   
}
