using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponStateManager weapon_stateManager;
    public int Magazine_count { get; protected set; }
    public abstract int Magazine_capacity { get; protected set; }
    public abstract int rate_of_fire { get; protected set; }
    public abstract int reloadSpeed { get; protected set; }
    public abstract int Accuracy { get; protected set; }
    public abstract BulletType bulletType { get; protected set; }

    // Start is called before the first frame update
   protected void SetUpStats(int MagCap,int rate_of_fire,int reloadSpeed,int Accuracy,BulletType bulletType)
    {
        Magazine_capacity = MagCap;
        this.rate_of_fire = rate_of_fire;
        this.reloadSpeed = reloadSpeed;
        this.Accuracy = Accuracy;
        this.bulletType = bulletType;
    }
    void Start()
    {
        weapon_stateManager = GetComponent<WeaponStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
