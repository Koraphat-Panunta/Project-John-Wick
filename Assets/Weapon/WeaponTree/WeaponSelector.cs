using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : WeaponNode
{
    public override List<WeaponNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get ; set; }

    public WeaponSelector(Weapon weapon,Func<bool> preCondition) :base(weapon)
    {
        this.preCondidtion = preCondition;
    }
    public WeaponSelector(Weapon weapon) : base(weapon)
    {
       
    }
    public override void Update()
    {
        Weapon.currentNode = Selector();
    }
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
    protected WeaponNode Selector()
    {
        WeaponNode reTurnNode = null;
        foreach (WeaponNode node in childNode)
        {
            if (node.PreCondition() == true)
                reTurnNode = node;
            break;
        }
        return reTurnNode;
    }
   
}
