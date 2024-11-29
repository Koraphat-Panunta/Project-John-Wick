using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STI_9mm : SecondaryWeapon,MagazineType
{
    //SetUpStats
    private int _magazineCapacity = 15;
    private float _rateOfFire = 500;
    private float _reloadSpeed = 1.2f ;
    private float _accuracy = 136;
    private float _recoilController = 1;
    private float _recoilCameraController = 5;
    private float _aimDownSightSpeed = 3.6f;
    private float _recoilKickBack ;
    private float min_percision = 18;
    private float max_percision = 65;
    public override int Magazine_capacity
    {
        get { return _magazineCapacity; }
        set { _magazineCapacity = value; }
    }
    public override float rate_of_fire
    {
        get { return _rateOfFire; }
        set { _rateOfFire = value; }
    }
    public override float reloadSpeed
    {
        get { return _reloadSpeed; }
        set { _reloadSpeed = value; }
    }
    public override float Accuracy
    {
        get { return _accuracy; }
        set { _accuracy = value; }
    }
    public override float RecoilController
    {
        get { return _recoilController; }
        set { _recoilController = value; }
    }
    public override float aimDownSight_speed
    {
        get { return _aimDownSightSpeed; }
        set { _aimDownSightSpeed = value; }
    }
    public override float RecoilKickBack 
    {
        get { return _recoilKickBack; }
        set { _recoilKickBack = value; }
    }
    public override float RecoilCameraController 
    {
        get { return _recoilCameraController; }
        set { _recoilCameraController = value; }
    }
    public override float min_Precision
    {
        get { return min_percision; }
        set { min_percision = value; }
    }

    public override float max_Precision
    {
        get { return max_percision; }
        set { max_percision = value; }
    }
    public override Bullet bullet { get; set; }
    public override float movementSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isMagIn { get; set ; }
    protected override float quickDrawTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    protected override WeaponTreeManager weaponTree { get ; set ; }
    private WeaponTreeMagazineAuto weaponTreeMagazineAuto;
    protected override void FixedUpdate()
    {
        weaponTreeMagazineAuto.FixedUpdateTree();
        base.FixedUpdate();
    }
    protected override void Update()
    {
        weaponTreeMagazineAuto.UpdateTree();
        base.Update();
    }
    protected override void Start()
    {
        weaponTree = new WeaponTreeMagazineAuto(this);
        weaponTreeMagazineAuto = weaponTree as WeaponTreeMagazineAuto;
        weaponTreeMagazineAuto.InitailizedTree();
        fireMode = FireMode.Single;
        bullet = new _9mmBullet();
        RecoilKickBack = bullet.recoilKickBack;

        bulletStore.Add(BulletStackType.Magazine, Magazine_capacity);
        base.Start();
        
    }

}
