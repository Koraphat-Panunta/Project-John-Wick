using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSequenceNode : WeaponActionNode
{
    public override List<WeaponNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }

    protected WeaponActionNode curActionNode;
    int curNodeIndex;
    public WeaponSequenceNode(Weapon weapon,Func<bool> preCondition) : base(weapon)
    {
        this.preCondidtion = preCondition;
    }
    public override void Enter()
    {
        curActionNode = null;
        curNodeIndex = 0;
    }

    public override void Exit()
    {
        curActionNode = null;
    }
    public override void Update() 
    {
        //Begin
        if (curActionNode == null)
        {
            curActionNode = childNode[curNodeIndex] as WeaponActionNode;
            if(curActionNode != null)
                curActionNode.Enter();
        }

         
        if (curActionNode.IsComplete())
        {
            curNodeIndex += 1;
            
            curActionNode.Exit();
            if(curNodeIndex < childNode.Count)
            curActionNode = childNode[curNodeIndex] as WeaponActionNode;
            if (curActionNode != null)
            curActionNode.Enter();
        }
        curActionNode.Update();
    }
    public override void FixedUpdate()
    {
      
        if(curActionNode != null)
        curActionNode.FixedUpdate();
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
        Debug.Log("Call precondition sequence = "+preCondidtion.Invoke());
        return preCondidtion.Invoke();
    }
    

  

    public override bool IsComplete()
    {
        if (curNodeIndex == childNode.Count)
            return true;
        else return false;
    }
}
