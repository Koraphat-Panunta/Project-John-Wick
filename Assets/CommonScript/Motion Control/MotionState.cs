using UnityEngine;

public abstract class MotionState 
{
    public MotionState()
    {

    }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Enter()
    {

    }
    public virtual void Exit() 
    {
        
    }
}
