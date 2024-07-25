using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponStance : State
{
    protected WeaponSocket weaponSocket;
    public Animator animator { get; private set;}
    protected virtual void Start()
    {
        weaponSocket = GetComponentInParent<WeaponSocket>();
        animator = weaponSocket.GetAnimator();
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState(StateManager stateManager)
    {
        base.FrameUpdateState(stateManager);
    }

    public override void PhysicUpdateState(StateManager stateManager)
    {
        base.PhysicUpdateState(stateManager);
    }
}
