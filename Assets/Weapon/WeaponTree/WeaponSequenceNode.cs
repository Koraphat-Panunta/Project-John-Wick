using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSequenceNode : WeaponActionNode
{
    public override List<WeaponNode> childNode { get; set; }
    protected override WeaponTreeManager weaponTree { get; set; }
    protected override WeaponBlackBoard blackBoard { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected Queue<WeaponActionNode> actionNodes { get; set; }
    public override List<WeaponNode> SubNode { get; set ; }

    protected WeaponActionNode curActionNode;
    public WeaponSequenceNode(WeaponTreeManager weaponTreeManager,Func<bool> preCondition) : base(weaponTreeManager)
    {
        this.weaponTree = weaponTreeManager;
        blackBoard = weaponTreeManager.WeaponBlackBoard;
        this.preCondidtion = preCondition;
    }
    public override void Update()
    {
        //Begin
        if (curActionNode == null)
            curActionNode = UpdateSequence();

        curActionNode.Update();

        if (curActionNode.IsComplete())
            curActionNode = UpdateSequence();
    }
    public override void FixedUpdate()
    {
        if (curActionNode == null)
            curActionNode = UpdateSequence();

        curActionNode.FixedUpdate();

        if (curActionNode.IsComplete())
            curActionNode = UpdateSequence();
    }

    public override bool IsReset()
    {
       if(IsComplete())
            return true;
       else 
            return false;   
    }

    public override bool PreCondition()
    {
        return preCondidtion.Invoke();
    }
    public void AddQueueActionNode(WeaponActionNode actionNode)
    {
        this.actionNodes.Enqueue(actionNode);
    }
    public WeaponActionNode UpdateSequence()
    {
        WeaponActionNode actionNode = actionNodes.Dequeue();
        if (actionNodes.Count <= 0)
            return null;
        return actionNode;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override bool IsComplete()
    {
        if (curActionNode == null && actionNodes.Dequeue() == null)
            return true;
        else return false;
    }
}
