using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateManager :StateManager
{
     WeaponStanceManager stanceManager;
    public Fire fireState { get; protected set; }
    public Reload reloadState { get; protected set; }
    protected override void Start()
    {
        stanceManager = GetComponent<WeaponStanceManager>();
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
