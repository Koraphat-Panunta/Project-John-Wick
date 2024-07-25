
using UnityEngine;


public abstract class StatePattern : MonoBehaviour
{
    public bool IsEnter { get; protected set; }
    public bool IsExit { get; protected set; }

    public float timerState { get; private set; }

    private void Start()
    {
        IsEnter = false;
        IsExit = false;
    }

    public virtual void EnterState()
    {
        IsEnter = true;
        IsExit = false;
    }
    private bool nextframeIsExit = false;
    public virtual void FrameUpdateState()
    {
        timerState += Time.deltaTime;
    }
    public virtual void PhysicUpdateState() 
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
    }
    public virtual void ExitState()
    {
        IsEnter = false;
        IsExit = true;
        timerState = 0;
    }

}
