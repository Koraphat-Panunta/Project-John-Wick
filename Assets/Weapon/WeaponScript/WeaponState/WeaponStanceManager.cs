using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStanceManager : StateManager
{

    public LowReady lowReady { get;protected set; }
    public AimDownSight aimDownSight { get;protected set; }
    protected override void Start()
    {
        lowReady = gameObject.GetComponent<LowReady>();
        aimDownSight = gameObject.GetComponent<AimDownSight>();
        base.Current_state = lowReady;
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
