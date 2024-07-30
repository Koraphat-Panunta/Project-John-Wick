using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowReady : WeaponStance
{
    [SerializeField] private WeaponSingleton weaponSingleton;
    CameraController cameraController;
    public override void EnterState()
    {
        base.EnterState();
        WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.LowReady, weaponSingleton.GetWeapon());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState(StateManager stateManager)
    {
        weaponSingleton.GetStanceManager().AimingWeight -= weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.LowReady,weaponSingleton.GetWeapon());
        base.animator.SetLayerWeight(1,weaponSingleton.GetStanceManager().AimingWeight);
        base.FrameUpdateState(stateManager);
    }

    public override void PhysicUpdateState(StateManager stateManager)
    {
        base.PhysicUpdateState(stateManager);
    }

    protected override void Start()
    {
        
        base.Start();
    }
    private void Update()
    {
        if (cameraController == null)
        {
            cameraController = weaponSingleton.Camera.GetComponent<CameraController>();
        }
    }
}
