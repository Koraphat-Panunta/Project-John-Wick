using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSequenceNode : WeaponActionNode
{
    public override List<WeaponNode> childNode { get; set; }
    protected override WeaponTreeManager weaponTree { get; set; }
    protected override WeaponBlackBoard blackBoard { get; set; }
    protected override Func<bool> preCondidtion { get; set; }

    protected WeaponActionNode curActionNode;
    private int currentIndexNode;
    public WeaponSequenceNode(WeaponTreeManager weaponTreeManager,Func<bool> preCondition) : base(weaponTreeManager)
    {
        this.weaponTree = weaponTreeManager;
        blackBoard = weaponTreeManager.WeaponBlackBoard;
        this.preCondidtion = preCondition;
    }
    public override void Enter()
    {
        currentIndexNode = 0;
    }
    public override void Exit()
    {

    }

    public override bool IsComplete()
    {
        if (curActionNode == null && UpdateSequence() == null)
            return true;
        else return false;
    }
    public override void Update()
    {
        Debug.Log("SequenceNode Update");
        if (curActionNode != null) Debug.Log("curAc = "+curActionNode);
        //Begin
        if (curActionNode == null)
        {
            curActionNode = UpdateSequence();
            curActionNode.Enter();
        }

         curActionNode.Update();

        if (curActionNode.IsComplete())
        {
            curActionNode.Exit();
            curActionNode = UpdateSequence();
            if(curActionNode != null)
            curActionNode.Enter();
        }
    }
    public override void FixedUpdate()
    {
        //if (curActionNode == null)
        //    curActionNode = UpdateSequence();

        curActionNode.FixedUpdate();

        //if (curActionNode.IsComplete())
        //    curActionNode = UpdateSequence();
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
    //public void AddActionNode(WeaponActionNode actionNode)
    //{
    //    this.actionNodes.Enqueue(actionNode);
    //}
    public WeaponActionNode UpdateSequence()
    {
        Debug.Log("IndexNode sq" + currentIndexNode);
        if(currentIndexNode == childNode.Count)
            return null;
        WeaponActionNode actionNode;
        if (childNode[currentIndexNode] is WeaponActionNode)
            actionNode = childNode[currentIndexNode] as WeaponActionNode;
        else
            childNode[currentIndexNode].Transition(out actionNode);
        currentIndexNode++;
        return actionNode;
    }

    
}
