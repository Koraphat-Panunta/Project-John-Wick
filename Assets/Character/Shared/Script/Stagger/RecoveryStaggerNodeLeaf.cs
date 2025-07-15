using System;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryStaggerNodeLeaf : INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    private IStaggerAble staggerAble { get; set; }
    private float timer;
    private float recoverTime;
    private bool isComplete;
    public RecoveryStaggerNodeLeaf(Func<bool> preCondition, IStaggerAble staggerAble,float recoverTime)
    {
        this.preCondition = preCondition;
        this.staggerAble = staggerAble;
        this.nodeLeafBehavior = new NodeLeafBehavior();
        this.isReset = new List<Func<bool>>();  
        this.recoverTime = recoverTime;
    }
    public void Enter()
    {
        this.isComplete = false;
        this.timer = 0;
    }

    public void Exit()
    {
        
    }
    public void UpdateNode()
    {
        this.timer += Time.deltaTime;

        if(this.timer >= this.recoverTime)
        {
            this.isComplete = true;
            this.staggerAble.staggerGauge = staggerAble.maxStaggerGauge;
        }
    }
    public void FixedUpdateNode()
    {
       
    }
    public bool IsComplete()
    {
        return isComplete;
    }
    public bool IsReset()
    {
        if(IsComplete())
            return true;

        return nodeLeafBehavior.IsReset(isReset);
    }
    public bool Precondition()
    {
       return preCondition.Invoke();
    }
}
