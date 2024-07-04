
using UnityEngine;


public abstract class State
{
    public bool IsEnter = false;
    public bool IsExit = false;
    
    public float timerState { get; private set; }



    public State()
    {
        

    }
    public virtual void EnterState()
    {
        IsEnter = true;
        IsExit = false;
    }
    private bool nextframeIsExit = false;
    public virtual void FrameUpdateState()
    {
        if (IsEnter == true)
        {
            IsEnter = false;
        }
        if (IsExit == true)
        {
            if (nextframeIsExit == true)
            {
                IsExit = false;
                nextframeIsExit = false;
            }
            if (IsExit == true)
            {
                nextframeIsExit = true;
            }
        }
        timerState += Time.deltaTime;
    }
    public virtual void PhysicUpdateState() { }
    public virtual void ExitState()
    {
        IsEnter = false;
        IsExit = true;
        timerState = 0;
    }
}
