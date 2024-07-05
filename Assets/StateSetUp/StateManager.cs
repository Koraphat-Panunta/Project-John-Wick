using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State Current_state { get; protected set; }
    public StateManager(State StartState)
    {
        Current_state = StartState;
    }
    public virtual void UpdateState()
    {
        Current_state.FrameUpdateState();
    }
    public virtual void FixedStateUpdate()
    {
        Current_state.PhysicUpdateState();
    }
    public virtual void ChangeState(State Nextstate)
    {
        if (Current_state == null)
        {
            Current_state = Nextstate;
            Current_state.EnterState();
        }
        else
        {
            Current_state.ExitState();
            Current_state = Nextstate;
            Current_state.EnterState();
        }
    }
}
