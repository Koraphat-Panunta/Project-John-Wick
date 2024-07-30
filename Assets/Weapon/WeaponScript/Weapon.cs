using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour 
{
    public WeaponStateManager weapon_stateManager { get; protected set; }
    public WeaponStanceManager weapon_StanceManager { get; protected set; }
    public int Magazine_count;

    public abstract int Magazine_capacity { get; protected set; }
    public abstract float rate_of_fire { get; protected set; }
    public abstract float reloadSpeed { get; protected set; }
    public abstract float Accuracy { get; protected set; }
    public abstract float Recoil { get; protected set; }
    public abstract float aimDownSight_speed { get; protected set; }
    public abstract GameObject bullet { get; protected set; }

  
    protected virtual void Start()
    {
        weapon_stateManager = GetComponent<WeaponStateManager>();
        weapon_StanceManager = GetComponent<WeaponStanceManager>();
        Magazine_count = Magazine_capacity;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
