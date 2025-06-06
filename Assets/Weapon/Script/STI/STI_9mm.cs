using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class STI_9mm : Weapon, SecondaryWeapon, MagazineType, IBlowBack
{
    //SetUpStats
    private int _magazineCapacity = 15;
    private float _rateOfFire = 500;
    private float _reloadSpeed = 1.2f;
    [Range(0,600)]
    [SerializeField] private float _accuracy /*= 136*/;
    [Range(0,600)]
    [SerializeField] private float _recoilController/* = 1*/;
    [Range(0,600)]
    [SerializeField] private float _recoilCameraController/* = 5*/;
    [Range(0, 600)]
    [SerializeField] private float _aimDownSightSpeed = 3.6f;
    [Range(0, 600)]
    [SerializeField] private float _recoilKickBack;
    [Range(0, 600)]
    [SerializeField] private float min_percision /*= 18*/;
    [Range(0, 600)]
    [SerializeField] private float max_percision /*= 65*/;
    private float DrawSpeed = 1;

    public override Transform gripPos { get => transform;set { } }
    public override Transform SecondHandgripPos { get => transform; set { } }
    public Weapon _weapon { get => this; set { } }
    public ReloadMagazineLogic _reloadMagazineLogic { get ; set ; }

    public override float drawSpeed 
    { 
        get => this.DrawSpeed ; 
        set => this.DrawSpeed = value ; 
    }

    public override int bulletCapacity
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

    public float quickDrawTime { get ; set ; }
    public override Bullet bullet { get; set; }
    public override float movementSpeed { get; set; }

    public bool _isMagIn { get { return true; } set { } }
    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Awake()
    {
        fireMode = FireMode.Single;
        bullet = new _9mmBullet(this);
        _reloadMagazineLogic = new ReloadMagazineLogic();
        RecoilKickBack = bullet.recoilKickBack;

        bulletStore.Add(BulletStackType.Magazine, bulletCapacity);
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    public void InitailizedReloadStageSelector()
    {
        _reloadStageSelector = new NodeSelector(
           () => {
               if (userWeapon.isReloadCommand
               && userWeapon.weaponBelt.ammoProuch.amountOf_ammo[bullet.myType] > 0
               && bulletStore[BulletStackType.Magazine] < bulletCapacity)
                   return true;
               else
                   return false;
           }, nameof(_reloadStageSelector)
           );
    }
    public void Performed(MagazineType magazineWeapon, AmmoProuch ammoProuch,IReloadMagazineNode reloadMagazineNode,Action enter) 
        => _reloadMagazineLogic.ReloadMagzine(magazineWeapon, ammoProuch,reloadMagazineNode,enter);
    

    public override WeaponSelector startEventNode { get; set; }
    public ReloadMagazineFullStage _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage _tacticalReloadMagazineFullStage { get; set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get; set; }
    public override RestNode restNode { get ; set ; }
    public NodeSelector _reloadStageSelector { get ; set ; }

    protected override void InitailizedTree()
    {

        startEventNode = new WeaponSelector(this, () => true);

        

        _reloadMagazineFullStage = new ReloadMagazineFullStage(this, 
            () => 
            {
                int chamberCount = bulletStore[BulletStackType.Chamber];
                int magCount = bulletStore[BulletStackType.Magazine];
                bool isMagIn = this._isMagIn;
                if
                    (
                     isMagIn == true
                    && chamberCount == 0
                    && magCount == 0
                    )
                    return true;
                else
                    return false;
            }
            );

        _tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStage(this, 
            () => 
            {
                bool IsMagIn = this._isMagIn;
                int MagCount = bulletStore[BulletStackType.Magazine];
                if (
                    IsMagIn == true
                    && MagCount >= 0
                    )
                    return true;
                else
                    return false;
            }
            );

        firingAutoLoad = new WeaponSequenceNode(this,
            () => {
                return bulletStore[BulletStackType.Chamber] > 0
                && triggerState == TriggerState.IsDown;
            }
            );

        fire = new FiringNode(this, () =>bulletStore[BulletStackType.Chamber] > 0);

        autoLoadChamber = new AutoLoadChamberNode(this,()=>true);

        restNode = new RestNode(this,()=>true);

        startEventNode.AddtoChildNode(reloadStageSelector);
        startEventNode.AddtoChildNode(firingAutoLoad);
        startEventNode.AddtoChildNode(restNode);

        reloadStageSelector.AddtoChildNode(_reloadMagazineFullStage);
        reloadStageSelector.AddtoChildNode(_tacticalReloadMagazineFullStage);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        startEventNode.FindingNode(out INodeLeaf eventNode);
        currentEventNode = eventNode as WeaponLeafNode ;
     
    }

    public void ReloadMagzine(MagazineType magazineWeapon, AmmoProuch ammoProuch, IReloadMagazineNode reloadMagazineNode, Action action)
    {
        throw new NotImplementedException();
    }
}
