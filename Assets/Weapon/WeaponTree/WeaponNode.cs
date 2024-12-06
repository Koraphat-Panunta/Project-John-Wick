using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class WeaponNode 
{
    public WeaponNode(Weapon weapon)
    {
        this.Weapon = weapon;
        childNode = new List<WeaponNode>();
    }

    protected Weapon Weapon { get; set; }
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
                {
                    weaponActionNode = weaponNode as WeaponActionNode;
                    Debug.Log("Transition from " + this + " ->" + weaponActionNode);
                }
                else
                {
                    Debug.Log("Transition from " + this + " ->" + weaponNode);
                    weaponNode.Transition(out weaponActionNode);
                }
                break;
            }
        }
    }
    public void AddChildNode(WeaponNode weaponNode)
    {
        childNode.Add(weaponNode);
    }
    

    
}
