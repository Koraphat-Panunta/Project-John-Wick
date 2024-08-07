using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : WeaponStance
{
    [SerializeField] WeaponSingleton weaponSingleton;
    private Camera camera;
    GameObject WeaponUserCharacter;
    public AimDownSight()
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        //WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.Aim,weaponSingleton.GetWeapon());
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void WeaponStanceUpdate(WeaponStanceManager weaponStanceManager)
    {
        WeaponActionEvent.Publish(WeaponActionEvent.WeaponEvent.Aim, weaponSingleton.GetWeapon());
        weaponSingleton.Aim.Invoke(weaponSingleton.GetWeapon());
        base.animator.SetLayerWeight(1, weaponSingleton.GetStanceManager().AimingWeight);
        RotateTowards(camera.transform.forward);
    }
    public override void WeaponStanceFixedUpdate(WeaponStanceManager weaponStanceManager)
    {
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
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
