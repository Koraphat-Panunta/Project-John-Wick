using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectorNode : PlayerNode
{
    public PlayerSelectorNode(Player player,Func<bool> preCondition) : base(player)
    {
        this.preCondidtion = preCondition;
    }

    public override List<PlayerNode> childNode { get ; set; }
    protected override Func<bool> preCondidtion { get ; set ; }


    public override void FixedUpdate()
    {
       
    }

    public override bool IsReset()
    {
        return true;
    }

    public override bool PreCondition()
    {
       return preCondidtion.Invoke();
    }

    public override void Update()
    {
        
    }
}
