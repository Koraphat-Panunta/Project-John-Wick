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
    public void ChangeState(State Nextstate)
    {
        Current_state.ExitState();
        Current_state = Nextstate;
        Current_state.EnterState();
    }
}
