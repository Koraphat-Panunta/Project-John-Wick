using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAnimationNode 
{
    protected PlayerAnimationManager playerAnimationManager;
    protected Animator animator;

    public PlayerAnimationNode(PlayerAnimationManager player,Animator animator)
    {
        this.playerAnimationManager = player;
        this.animator = animator;
    }

    public abstract List<PlayerAnimationNode> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out PlayerAnimationNodeLeaf playerAnimationNodeLeaf)
    {
        playerAnimationNodeLeaf = null;
        foreach (PlayerAnimationNode playerAnimNode in childNode)
        {
            if (playerAnimNode.PreCondition() == false)
            { continue; }

            if (playerAnimNode.GetType().IsSubclassOf(typeof(PlayerAnimationNodeLeaf)))
            {
                playerAnimationNodeLeaf = playerAnimNode as PlayerAnimationNodeLeaf;

            }
            else
            {
                playerAnimNode.Transition(out playerAnimationNodeLeaf);
            }
            break;
        }
    }
    public void AddChildNode(PlayerAnimationNode playerAnimationNode)
    {
        childNode.Add(playerAnimationNode);
    }

}
