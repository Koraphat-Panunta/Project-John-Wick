using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class EnemyAnimationNode 
{
    protected Animator animator;
    public EnemyAnimationNode(EnemyAnimationManager enemyAnimation)
    {
        this.enemyAnimation = enemyAnimation;
        this.animator = enemyAnimation.animator;
        childNode = new List<EnemyAnimationNode>();
    }

    protected EnemyAnimationManager enemyAnimation { get; set; }
    public abstract List<EnemyAnimationNode> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out EnemyAnimationNodeLeaf animationNodeLeaf)
    {
        animationNodeLeaf = null;
        foreach (EnemyAnimationNode animationNode in childNode)
        {
            if (animationNode.PreCondition())
            {
                if (animationNode.GetType().IsSubclassOf(typeof(EnemyAnimationNodeLeaf)))
                {
                    animationNodeLeaf = animationNode as EnemyAnimationNodeLeaf;
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
    public void AddChildNode(EnemyAnimationNode animationNode)
    {
        childNode.Add(animationNode);
    }
}
