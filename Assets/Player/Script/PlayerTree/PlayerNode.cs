using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class PlayerNode 
{
    public PlayerNode(Player player)
    {
        this.player = player;
        childNode = new List<PlayerNode>();
    }

    protected Player player { get; set; }
    public abstract List<PlayerNode> childNode { get; set; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out PlayerActionNodeLeaf playerActionNode)
    {
        playerActionNode = null;
        foreach (PlayerNode playerNode in childNode)
        {
            if(playerNode.PreCondition()==false)
                { continue; }

            if (playerNode.GetType().IsSubclassOf(typeof(PlayerActionNodeLeaf)))
            {
                playerActionNode = playerNode as PlayerActionNodeLeaf;

            }
            else
            {
               
                playerNode.Transition(out playerActionNode);
            }
            break;
        }
    }
    public void AddChildNode(PlayerNode playerNode)
    {
        childNode.Add(playerNode);
    }
}
