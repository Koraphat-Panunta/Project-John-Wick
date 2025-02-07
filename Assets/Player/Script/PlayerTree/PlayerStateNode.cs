using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class PlayerStateNode :INode
{
    public PlayerStateNode(Player player,Func<bool> preCondition)
    {
        this.player = player;
        this.preCondition = preCondition;
    }

    protected Player player { get; set; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get ; set ; }

    public bool Precondition()=>preCondition.Invoke();
   
}
