using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class AnimationNode 
{
    public AnimationNode(EnemyAnimation enemyAnimation)
    {
        this.enemyAnimation = enemyAnimation;
        childNode = new List<AnimationNode>();
    }

    protected EnemyAnimation enemyAnimation { get; set; }
    public abstract List<AnimationNode> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out AnimationNodeLeaf animationNodeLeaf)
    {
        animationNodeLeaf = null;
        foreach (AnimationNode animationNode in childNode)
        {
            if (animationNode.PreCondition())
            {
                if (animationNode.GetType().IsSubclassOf(typeof(AnimationNodeLeaf)))
                {
                    animationNodeLeaf = animationNode as AnimationNodeLeaf;
                    //Debug.Log("Transition from " + this + " ->" + weaponActionNode);
                }
                else
                {
                    //Debug.Log("Transition from " + this + " ->" + weaponNode);
                    animationNode.Transition(out animationNodeLeaf);
                }
                break;
            }
        }
    }
    public void AddChildNode(AnimationNode animationNode)
    {
        childNode.Add(animationNode);
    }
}
