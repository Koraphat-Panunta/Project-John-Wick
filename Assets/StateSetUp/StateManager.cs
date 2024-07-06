using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StateManager : MonoBehaviour
{
    public State Current_state { get; protected set; }
    public StateManager(State StartState)
    {
        Current_state = StartState;
    }
    protected virtual void Update()
    {
        Current_state.FrameUpdateState();
    }
    protected virtual void FixedUpdate()
    {
        Current_state.PhysicUpdateState();
    }
    public virtual void ChangeState(State Nextstate)
    {
       
            Current_state.ExitState();
            Current_state = Nextstate;
            Current_state.EnterState();
      
    }
}
