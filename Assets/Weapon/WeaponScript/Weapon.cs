using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour 
{
    public WeaponStateManager weapon_stateManager { get; protected set; }
    public int Magazine_count { get; protected set; }

    public abstract int Magazine_capacity { get; protected set; }
    public abstract float rate_of_fire { get; protected set; }
    public abstract float reloadSpeed { get; protected set; }
    public abstract float Accuracy { get; protected set; }
    public abstract float Recoil { get; protected set; }
    public abstract float aimDownSight_speed { get; protected set; }
    public abstract BulletType bulletType { get; protected set; }

    // Start is called before the first frame update
    //protected void SetUpStats(int MagCap,float rate_of_fire,float reloadSpeed,float Accuracy,float Recoil,float ads_Speed,BulletType bulletType)
    // {
    //     Magazine_capacity = MagCap;
    //     this.rate_of_fire = rate_of_fire;
    //     this.reloadSpeed = reloadSpeed;
    //     this.Accuracy = Accuracy;
    //     this.Recoil = Recoil;
    //     this.aimDownSight_speed = aimDownSight_speed;
    //     this.bulletType = bulletType;
    // }
    void Start()
    {
        weapon_stateManager = GetComponent<WeaponStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
