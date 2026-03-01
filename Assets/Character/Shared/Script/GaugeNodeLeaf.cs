using System;
using System.Collections.Generic;
using UnityEngine;

public class GaugeNodeLeaf : INodeLeaf
{
    public List<Func<bool>> isReset { get ; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set ; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    public float gauge { get; protected set; }
    public float maxGauge { get; protected set; }

    public GaugeNodeLeaf(Func<bool> preCondition,
        float gauge, float maxGauge)
    {
        this.isReset = new List<Func<bool>>();
        this.nodeLeafBehavior = new NodeLeafBehavior();
        this.preCondition = preCondition;
        this.gauge = gauge;
        this.maxGauge = maxGauge;
    }
    public void Enter()
    {

    }

    public void Exit()
    {
    
    }
    public void UpdateNode()
    {

    }
    public void FixedUpdateNode()
    {
  
    }

    public bool IsComplete()
    {
        return false;
    }

    public float SetGauge(float value) => this.gauge = Mathf.Clamp(value,0,this.maxGauge);

    public bool IsReset() => this.nodeLeafBehavior.IsReset(this.isReset);
  
    public bool Precondition() => this.preCondition();
   

   
}
