using System;
using System.Collections.Generic;
using UnityEngine;

public class DropWeaponManuverNodeLeaf : WeaponManuverLeafNode,INodeLeafTransitionAble
{
    private bool isComplete;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public DropWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        nodeManager = weaponAdvanceUser.weaponManuverManager;
        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }

    public override void Enter()
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);

        isComplete = false;
        weaponAdvanceUser._currentWeapon.DropWeapon();
        isComplete = true;

        nodeLeafTransitionBehavior.TransitionAbleAll(this);
    }

    public override void Exit()
    {
        
    }
    public override bool IsReset()
    {
       return isComplete;
    }
    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override void UpdateNode()
    {
        Transitioning();
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);

    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
