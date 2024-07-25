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
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState(StateManager stateManager)
    {
        Debug.Log("LowReady");
        weaponSingleton.GetStanceManager().AimingWeight -= weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        cameraController.CinemachineFreeLook.m_Lens.FieldOfView = 65 - weaponSingleton.GetStanceManager().AimingWeight * 25;
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
