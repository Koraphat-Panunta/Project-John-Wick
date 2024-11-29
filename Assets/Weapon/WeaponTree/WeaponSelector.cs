using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : WeaponNode
{
    public override List<WeaponNode> childNode { get; set; }
    protected override WeaponTreeManager weaponTree { get ; set; }
    protected override WeaponBlackBoard blackBoard { get ; set ; }
    protected override Func<bool> preCondidtion { get ; set; }

    public WeaponSelector(WeaponTreeManager weaponTreeManager,Func<bool> preCondition) :base(weaponTreeManager)
    {
        this.weaponTree = weaponTreeManager;
        this.blackBoard = weaponTreeManager.WeaponBlackBoard;
        this.preCondidtion = preCondition;
    }
    public override void Update()
    {
        weaponTree.currentNode = Selector();
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
