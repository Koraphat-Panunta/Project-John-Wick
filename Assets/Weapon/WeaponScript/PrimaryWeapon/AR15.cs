using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15 : PrimaryWeapon
{
    [SerializeField] private Transform MuzzleSocket;
    [SerializeField] private Transform GripSocket;
    [SerializeField] private Transform Scope;
    [SerializeField] private Transform Stock;
    [SerializeField] private Transform Magazine;
    [SerializeField] private Transform Laser;
    //SetUpStats
    [SerializeField] PrimaryWeaponModel Model;
    private int _magazineCapacity = 30;
    private float _rateOfFire = 550;
    private float _reloadSpeed = 3;
    private float _accuracy = 30;
    [SerializeField] private GameObject _bulletType;
    private float _recoilController = 18.56f;
    private float _recoilCameraKickBack = 0.02f;
    private float _aimDownSightSpeed = 3f;
    private float _recoilKickBack = 50;
    private float min_percision = 20;
    private float max_percision = 65;





    //public override GameObject bullet
    //{
    //    get { return _bulletType; }
    //    set { _bulletType = value; }
    //}



    protected override void Start()
    {
        weaponSlotPos.Add(AttachmentSlot.MUZZLE,MuzzleSocket);
        weaponSlotPos.Add(AttachmentSlot.GRIP,GripSocket);
        weaponSlotPos.Add(AttachmentSlot.SCOPE,Scope);
        weaponSlotPos.Add(AttachmentSlot.STOCK,Stock);
        weaponSlotPos.Add(AttachmentSlot.MAGAZINE,Magazine);
        weaponSlotPos.Add(AttachmentSlot.LASER,Laser);
       
        fireMode = FireMode.FullAuto;
        
        base.Start();
    }
    private void Initialized()
    {

    }
}
