using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class StateManager : MonoBehaviour
{
    public State Current_state { get; protected set; }
    protected virtual void Start()
    {
        SetUpState();
    }
    protected virtual void Update()
    {
        Current_state.FrameUpdateState(this);
    }
    protected virtual void FixedUpdate()
    {
        Current_state.PhysicUpdateState(this);
    }
    public virtual void ChangeState(State Nextstate)
    {
       
            Current_state.ExitState();
            Current_state = Nextstate;
           Current_state.EnterState();
        
    }
    protected virtual void SetUpState() 
    {
        
    }

   
}
