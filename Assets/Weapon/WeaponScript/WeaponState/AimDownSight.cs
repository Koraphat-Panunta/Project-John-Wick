using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : WeaponStance
{
    [SerializeField] WeaponSingleton weaponSingleton;
    private Camera camera;
    GameObject WeaponUserCharacter;
    public AimDownSight(WeaponSingleton weaponSingleton)
    {
        this.weaponSingleton = weaponSingleton;
        base.animator = this.weaponSingleton.animator;
        WeaponUserCharacter = weaponSingleton.UserWeapon;
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void WeaponStanceUpdate(WeaponStanceManager weaponStanceManager)
    {
        weaponSingleton.Aim.Invoke(weaponSingleton.GetWeapon());
        base.animator.SetLayerWeight(1, weaponSingleton.GetStanceManager().AimingWeight);
        RotateUser(weaponSingleton.UserWeapon);
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
    private void RotateUser(GameObject userWeapon)
    {
        if (userWeapon.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.rotateObjectToward.RotateTowards(weaponSingleton.Camera.transform.forward, player.gameObject, 6);
        }
        else if (userWeapon.TryGetComponent<Enemy>(out Enemy enemy))
        {

        }
    }
   

    
}
