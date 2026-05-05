using System;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTimerNodeLeaf : INodeLeaf
{
    public List<Func<bool>> isReset { get; set ; }
    public NodeLeafBehavior nodeLeafBehavior { get; set ; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }

    public float timer { get; protected set; }

    public float loopDuration { get; protected set; }
    public float[] phaseThresholds { get; protected set; }
    public bool isLooping { get; protected set; }

    public PhaseTimerNodeLeaf(Func<bool> preCondition,float loopDurarion, float[] phaseThresholds,bool isLoop)
    {
        this.isReset = new List<Func<bool>>();
        this.nodeLeafBehavior = new NodeLeafBehavior();
        this.preCondition = preCondition;

        this.loopDuration = loopDurarion;
        this.phaseThresholds = phaseThresholds;
        this.isLooping = isLoop;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }
    public void UpdateNode()
    {
        if (this.timer >= this.loopDuration
            && this.isLooping) 
        {
            this.timer = 0;
        }

        if (this.timer < this.loopDuration)
            this.timer += Time.deltaTime;
        

    }

    public void FixedUpdateNode()
    {
        
    }
    public bool IsComplete()
    {
        return this.timer >= this.loopDuration && this.isLooping == false;
    }
    public int GetCurrentPhase()
    {
        if(this.phaseThresholds == null
            || this.phaseThresholds.Length <= 0)
        {
            return 0;
        }

        int curPhaseNumber = 0;

        for (int i = 0; i < this.phaseThresholds.Length; i++) 
        {
            if(this.timer > this.phaseThresholds[i])
            {
                curPhaseNumber += 1;
            }
            else
                break;
        }
        return curPhaseNumber;
    }
    public void SetPassPoint(float[] passPoint) => this.phaseThresholds = passPoint;  
    public bool Precondition() => this.preCondition.Invoke();
    public bool IsReset() => this.nodeLeafBehavior.IsReset(this.isReset);
    
}
