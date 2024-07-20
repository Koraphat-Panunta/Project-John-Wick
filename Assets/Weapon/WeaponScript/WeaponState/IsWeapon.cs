using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsWeapon
{
    public  int Magazine_capacity { get; protected set; }
    public  float rate_of_fire { get; protected set; }
    public float reloadSpeed { get; protected set; }
    public float Accuracy { get; protected set; }
    public float Recoil { get; protected set; }
    public float aimDownSight_speed { get; protected set; }
    public BulletType bulletType { get; protected set; }
}
