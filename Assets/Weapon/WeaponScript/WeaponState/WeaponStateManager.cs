using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateManager :StateManager
{
    public WeaponStanceManager stanceManager { get; protected set; }
    public Fire fireState { get; protected set; }
    public Reload reloadState { get; protected set; }
    public None none;
    protected override void Start()
    {
        stanceManager = GetComponent<WeaponStanceManager>();
        base.Current_state = none;
        base.Start();
    }
    public override void ChangeState(State Nextstate)
    {
        base.ChangeState(Nextstate);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void SetUpState()
    {
        
    }

}
