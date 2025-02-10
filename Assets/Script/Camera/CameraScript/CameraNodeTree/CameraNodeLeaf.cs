using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraNodeLeaf : CameraNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }
    public bool isComplete { get;protected set; }

    public CameraNodeLeaf(CameraController cameraController, Func<bool> preCondition) : base(cameraController, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public virtual void Enter()
    {

    }
    
    public virtual void Exit()
    {
       
    }

    public virtual void FixedUpdateNode()
    {

    }
    public virtual bool IsComplete() => isComplete;
   

    public virtual bool IsReset() => nodeLeafBehavior.IsReset(isReset);
    

    public virtual void UpdateNode()
    {
        
    }
}
