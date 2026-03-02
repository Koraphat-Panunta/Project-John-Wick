using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManagerNodeLeaf : GameManagerNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public string sceneName {protected get; set; }
    protected GameManager gameManager { get; set; }
    protected bool isTriggerReset;
    public GameManagerNodeLeaf(string sceneName,GameManager gameManager, Func<bool> preCondition) : base(preCondition)
    {
        this.isReset = new List<Func<bool>>();
        this.sceneName = sceneName;
        nodeLeafBehavior = new NodeLeafBehavior();
        this.gameManager = gameManager;
    }
    
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        this.isTriggerReset = false;
    }

    public virtual void FixedUpdateNode() { }


    public virtual bool IsComplete() { return false; }

    public void TriggerReset() => this.isTriggerReset = true;

    public virtual bool IsReset() 
    {
        if(this.isTriggerReset)
            return true;

        return nodeLeafBehavior.IsReset(this.isReset); 
    }


    public abstract void UpdateNode();
   
}
