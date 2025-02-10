using System;
using UnityEngine;

public abstract class CameraNode : INode
{
    public Func<bool> preCondition { get; set ; }
    public INode parentNode { get ; set ; }
    protected CameraController cameraController { get; set ; }
    public CameraNode(CameraController cameraController,Func<bool> preCondition)
    {
        this.cameraController = cameraController;
        this.preCondition = preCondition;
    }
    public virtual bool Precondition() => preCondition.Invoke();
   
}
