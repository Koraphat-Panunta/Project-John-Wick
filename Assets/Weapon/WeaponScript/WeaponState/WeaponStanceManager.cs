 using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponStanceManager : MonoBehaviour
{
    [SerializeField] public WeaponSingleton weaponSingleton;
    public LowReady lowReady { get;protected set; }
    public AimDownSight aimDownSight { get;protected set; }
    public WeaponStance _currentStance { get; private set; }
    public float AimingWeight = 0;

    public void Start()
    {
       aimDownSight = new AimDownSight(weaponSingleton);
        lowReady = new LowReady(weaponSingleton);
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
   
    private void OnEnable()
    {
    }
    public void ChangeStance(WeaponStance Nextstance)
    {
        if (_currentStance != Nextstance)
        {
            _currentStance.ExitState();
            _currentStance = Nextstance;
            _currentStance.EnterState();
        }
       
    }
}
