using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STI_9mm :Weapon,SecondaryWeapon,MagazineType,IBlowBack
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
    private float DrawSpeed = 1;

    public override Transform gripPos { get => transform;set { } }
    public override Transform SecondHandgripPos { get => transform; set { } }

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

    public bool isMagIn { get { return true; } set { } }
    

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
        RecoilKickBack = bullet.recoilKickBack;

        bulletStore.Add(BulletStackType.Magazine, bulletCapacity);
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        
    }
    public override WeaponSelector startEventNode { get; set; }
    public WeaponSelector reloadStageSelector { get; private set; }
    public ReloadMagazineFullStage reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get; set; }
    public override RestNode restNode { get ; set ; }

    protected override void InitailizedTree()
    {

        startEventNode = new WeaponSelector(this, () => true);

        reloadStageSelector = new WeaponSelector(this,
           () => {
               bool reload = isReloadCommand;
               return reload && bulletStore[BulletStackType.Magazine] < bulletCapacity;
           }
           );

        reloadMagazineFullStage = new ReloadMagazineFullStage(this, 
            () => 
            {
                int chamberCount = bulletStore[BulletStackType.Chamber];
                int magCount = bulletStore[BulletStackType.Magazine];
                bool isMagIn = this.isMagIn;
                int ammoProuchCount = userWeapon.weaponBelt.ammoProuch.amountOf_ammo[bullet.myType];

                if
                    (
                     isMagIn == true
                    && chamberCount == 0
                    && magCount == 0
                    && ammoProuchCount >= 0
                    )
                    return true;
                else
                    return false;
            }
            );

        tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStage(this, 
            () => 
            {
                bool IsMagIn = this.isMagIn;
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

        reloadStageSelector.AddtoChildNode(reloadMagazineFullStage);
        reloadStageSelector.AddtoChildNode(tacticalReloadMagazineFullStage);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        startEventNode.FindingNode(out INodeLeaf eventNode);
        currentEventNode = eventNode as WeaponLeafNode ;
     
    }
}
