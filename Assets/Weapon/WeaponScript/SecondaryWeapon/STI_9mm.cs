using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STI_9mm : SecondaryWeapon
{
    //SetUpStats
    public override int Magazine_capacity
    {
        get { return Magazine_capacity; }
        protected set { Magazine_capacity = 15; }
    }
    public override float rate_of_fire
    {
        get { return rate_of_fire; }
        protected set { rate_of_fire = value; }
    }
    public override float reloadSpeed
    {
        get { return reloadSpeed; }
        protected set { reloadSpeed = value; }
    }
    public override float Accuracy
    {
        get { return Accuracy; }
        protected set { Accuracy = value; }
    }
    public override BulletType bulletType
    {
        get { return bulletType; }
        protected set { bulletType = value; }
    }
    public override float Recoil
    {
        get { return Recoil; }
        protected set { Recoil = value; }
    }
    public override float aimDownSight_speed
    {
        get { return aimDownSight_speed; }
        protected set { aimDownSight_speed = 0.3f; }
    }

}
