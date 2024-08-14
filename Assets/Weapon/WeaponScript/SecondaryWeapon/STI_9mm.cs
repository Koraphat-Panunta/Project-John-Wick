using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STI_9mm : SecondaryWeapon
{
    //SetUpStats
    private int _magazineCapacity = 15;
    private float _rateOfFire = 260;
    private float _reloadSpeed;
    private float _accuracy = 50;
   [SerializeField] private GameObject _bulletType;
    private float _recoilController = 18.56f;
    private float _aimDownSightSpeed = 3f;
    private float _recoilKickBack = 20;
    private float min_percision = 20;
    private float max_percision = 65;
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
    public override GameObject bullet
    {
        get { return _bulletType; }
        protected set { _bulletType = value; }
    }
    public override float RecoilController
    {
        get { return _recoilController; }
        protected set { _recoilController = value; }
    }
    public override float aimDownSight_speed
    {
        get { return _aimDownSightSpeed; }
        protected set { _aimDownSightSpeed = value; }
    }
    public override float RecoilKickBack 
    {
        get { return _recoilKickBack; }
        protected set { _recoilKickBack = value; }
    }

    public override float min_Precision
    {
        get { return min_percision; }
        protected set { min_percision = value; }
    }

    public override float max_Precision
    {
        get { return max_percision; }
        protected set { max_percision = value; }
    }

    protected override void Start()
    {
        fireMode = FireMode.Single;
        Chamber_Count = 1;
        Magazine_count = Magazine_capacity;
        base.Start();
    }

}
