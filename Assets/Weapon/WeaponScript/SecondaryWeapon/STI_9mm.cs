using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STI_9mm : SecondaryWeapon
{
    //SetUpStats
    private int _magazineCapacity = 15;
    private float _rateOfFire = 260;
    private float _reloadSpeed;
    private float _accuracy;
    private BulletType _bulletType;
    private float _recoil = 0.2f;
    private float _aimDownSightSpeed = 3f;
    public override int Magazine_capacity
    {
        get { return _magazineCapacity; }
        protected set { _magazineCapacity = value; }
    }
    public override float rate_of_fire
    {
        get { return _rateOfFire; }
        protected set { _rateOfFire = value; }
    }
    public override float reloadSpeed
    {
        get { return _reloadSpeed; }
        protected set { _reloadSpeed = value; }
    }
    public override float Accuracy
    {
        get { return _accuracy; }
        protected set { _accuracy = value; }
    }
    public override BulletType bulletType
    {
        get { return _bulletType; }
        protected set { _bulletType = value; }
    }
    public override float Recoil
    {
        get { return _recoil; }
        protected set { _recoil = value; }
    }
    public override float aimDownSight_speed
    {
        get { return _aimDownSightSpeed; }
        protected set { _aimDownSightSpeed = value; }
    }

}
