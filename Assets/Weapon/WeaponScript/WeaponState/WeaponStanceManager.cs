using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponStanceManager : StateManager
{

    public LowReady lowReady { get;protected set; }
    public AimDownSight aimDownSight { get;protected set; }
    public float AimingWeight = 0;

    protected override void Start()
    {
        lowReady = gameObject.GetComponent<LowReady>();
        aimDownSight = gameObject.GetComponent<AimDownSight>();
        base.Current_state = lowReady;
        base.Start();
    }
   
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void SetUpState()
    {
    }

    protected override void Update()
    {
        AimingWeight = Mathf.Clamp(AimingWeight, 0, 1);
        base.Update();
    }
}
