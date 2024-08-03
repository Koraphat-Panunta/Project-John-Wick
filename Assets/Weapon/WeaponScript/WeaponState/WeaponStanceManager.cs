 using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponStanceManager : StateManager
{
    [SerializeField] WeaponSingleton weaponSingleton;
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
        
        base.Update();
    }
    public void AimingWeightUpdate(Weapon weapon)
    {
        if (weapon.weapon_StanceManager.Current_state == weapon.weapon_StanceManager.aimDownSight)
        {
            AimingWeight += weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        }
        if(weapon.weapon_StanceManager.Current_state == weapon.weapon_StanceManager.lowReady)
        {
            AimingWeight -= weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        }
      
        AimingWeight = Mathf.Clamp(AimingWeight, 0, 1);

    }
    private void OnEnable()
    {
        weaponSingleton.Aim += AimingWeightUpdate;
        weaponSingleton.LowReady += AimingWeightUpdate;
    }
}
