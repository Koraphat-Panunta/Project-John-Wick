using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class WeaponNode 
{
    public WeaponNode(WeaponTreeManager weaponTree)
    {
        this.weaponTree = weaponTree;
        childNode = new List<WeaponNode>();
    }

    protected abstract WeaponTreeManager weaponTree { get; set; }
    protected abstract WeaponBlackBoard blackBoard { get; set; }
    public abstract List<WeaponNode> childNode { get ; set ; }
    protected abstract Func<bool> preCondidtion { get; set; }
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract bool IsReset();
    public abstract bool PreCondition();
    public void Transition(out WeaponActionNode weaponActionNode)
    {
        weaponActionNode = null;
        foreach(WeaponNode weaponNode in childNode)
        {
            if (weaponNode.PreCondition())
            {
                if (weaponNode.GetType().IsSubclassOf(typeof(WeaponActionNode)))
                    weaponActionNode = weaponNode as WeaponActionNode;
                else
                    weaponNode.Transition(out weaponActionNode);
                break;
            }
        }
    }
    public void AddChildNode(WeaponNode weaponNode)
    {
        childNode.Add(weaponNode);
    }
    

    
}
