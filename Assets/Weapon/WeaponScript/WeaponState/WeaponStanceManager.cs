 using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponStanceManager : MonoBehaviour
{
    [SerializeField] WeaponSingleton weaponSingleton;
    public LowReady lowReady { get;protected set; }
    public AimDownSight aimDownSight { get;protected set; }
    public WeaponStance _currentStance { get; private set; }
    public float AimingWeight = 0;

    public void Start()
    {
       aimDownSight = new AimDownSight();
        lowReady = new LowReady();
       _currentStance = lowReady;
    }
   
    public void FixedUpdate()
    {
        _currentStance.WeaponStanceFixedUpdate(this);
    }

    public void Update()
    {
        _currentStance.WeaponStanceUpdate(this);
    }
    public void AimingWeightUpdate(Weapon weapon)
    {
        if (weapon.weapon_StanceManager._currentStance == weapon.weapon_StanceManager.aimDownSight)
        {
            AimingWeight += weaponSingleton.GetWeapon().aimDownSight_speed * Time.deltaTime;
        }
        if(weapon.weapon_StanceManager._currentStance == weapon.weapon_StanceManager.lowReady)
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
    public void ChangeStance(WeaponStance Nextstance)
    {
        if (_currentStance != Nextstance)
        {
            _currentStance.ExitState();
        }
        _currentStance = Nextstance;
        _currentStance.EnterState();
    }
}
