using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour 
{
    public WeaponStateManager weapon_stateManager { get; protected set; }
    public WeaponStanceManager weapon_StanceManager { get; protected set; }
    public int Magazine_count;
    public int Chamber_Count;
    public abstract int Magazine_capacity { get; protected set; }
    public abstract float rate_of_fire { get; protected set; }
    public abstract float reloadSpeed { get; protected set; }
    public abstract float Accuracy { get; protected set; }
    public abstract float RecoilController { get; protected set; }
    public abstract float aimDownSight_speed { get; protected set; }
    public abstract GameObject bullet { get; protected set; }
    public abstract float RecoilKickBack { get; protected set; }
    public abstract float min_Precision { get; protected set; }
    public abstract float max_Precision { get; protected set; }
    public enum FireMode
    {
        Single,
        //Burst,
        FullAuto
    }
    public FireMode fireMode { get; protected set; }
    public enum TriggerPull
    {
        Up,
        IsDown,
        Down,
        IsUp,
    }
    public TriggerPull triggerPull = TriggerPull.Up;

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
    public virtual void Aim()
    {
        weapon_StanceManager.ChangeStance(weapon_StanceManager.aimDownSight);
    }
    public virtual void Fire() 
    {
        if (fireMode == FireMode.Single)
        {
            if(triggerPull == TriggerPull.IsDown)
            {
                weapon_stateManager.ChangeState(weapon_stateManager.fireState);
            }
        }
        if(fireMode == FireMode.FullAuto)
        {
            if(triggerPull == TriggerPull.IsDown||triggerPull == TriggerPull.Down)
            {
                weapon_stateManager.ChangeState(weapon_stateManager.fireState);
            }
        }
    }
    public virtual void Reload() 
    {
        weapon_stateManager.ChangeState(weapon_stateManager.reloadState);
    }
    public virtual void LowWeapon()
    {
        weapon_StanceManager.ChangeStance(weapon_StanceManager.lowReady);
    }

}
