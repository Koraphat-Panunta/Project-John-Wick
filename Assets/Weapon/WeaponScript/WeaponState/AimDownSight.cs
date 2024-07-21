using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : WeaponStance
{
    [SerializeField] WeaponSingleton weaponSingleton;
    Camera camera;
    GameObject WeaponUserCharacter;
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState()
    {
        Debug.Log("Aim");
        base.animator.SetLayerWeight(1, 1);
        RotateTowards(camera.transform.forward);
        base.FrameUpdateState();
    }

    public override void PhysicUpdateState()
    {
        base.PhysicUpdateState();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        camera = Camera.main;
        WeaponUserCharacter = weaponSingleton.UserWeapon;
        base.Start();
    }
    protected float rotationSpeed = 5.0f;
    protected void RotateTowards(Vector3 direction)
    {
        // Ensure the direction is normalized
        direction.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        direction.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (direction != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            WeaponUserCharacter.transform.rotation = Quaternion.Slerp(WeaponUserCharacter.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
