using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateManager :StateManager
{
    
    public Fire fireState { get; protected set; }
    public Reload reloadState { get; protected set; }
    public None none { get; protected set; }
    protected override void Start()
    {
   
        fireState = GetComponent<Fire>();
        reloadState = GetComponent<Reload>();
        none = GetComponent<None>();
        base.Current_state = none;
        base.Start();
    }
    public override void ChangeState(State Nextstate)
    {
        base.ChangeState(Nextstate);
    }

    protected override void FixedUpdate()
    {
        Current_state.PhysicUpdateState(this);
    }
    protected override void Update()
    {
        Current_state.FrameUpdateState(this);
    }
    protected override void SetUpState()
    {
        
    }

}
