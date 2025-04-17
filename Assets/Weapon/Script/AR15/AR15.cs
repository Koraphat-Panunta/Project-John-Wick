using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15 :Weapon, PrimaryWeapon,MagazineType,IBlowBack,IMicroOpticAttachAble
{

    //[SerializeField] private Transform MuzzleSocket;
    [SerializeField] private Transform FrontGripSocket;
    //[SerializeField] private Transform Scope;
    //[SerializeField] private Transform Stock;
    //[SerializeField] private Transform Magazine;
    //[SerializeField] private Transform Laser;

    public int ChamberCount;
    public int MagCount;

    //SetUpStats
    private int _MagazineCapacity = 30;
    private float _RateOfFire = 720;
    private float _ReloadSpeed = 2;

    [Range(0,600)]
    [SerializeField] private float _Accuracy /*= 112*/;
    [Range(0, 600)]
    [SerializeField] private float _RecoilController /*= 40*/;
    [Range(0, 600)]
    [SerializeField] private float _RecoilCameraController /*= 30f*/;
    [Range(0, 10)]
    [SerializeField] private float _AimDownSightSpeed /*= 2.4f*/;
    [Range(0, 600)]
    [SerializeField] private float _RecoilKickBack/* = 60*/;
    [Range(0, 1080)]
    [SerializeField] private float Min_percision/* = 11*/;
    [Range(0, 1080)]
    [SerializeField] private float Max_percision/* = 74*/;

    private float DrawSpeed = 1f;
    private _556mmBullet _556MmBullet;

    public Transform forntGrip { get ; set ; }
    public Transform slingAnchor { get ; set ; }

    public override Transform gripPos { get => transform; set { } }
    public override Transform SecondHandgripPos { get => forntGrip ; set => forntGrip = value ; }

  

    public override int bulletCapacity { get => _MagazineCapacity; set => _MagazineCapacity = value; }
    public override float rate_of_fire { get => _RateOfFire; set => _RateOfFire = value; }
    public override float reloadSpeed { get => _ReloadSpeed; set => _ReloadSpeed = value; }
    public float accuracyAdditional { get; set; }
    public override float Accuracy { get => _Accuracy + accuracyAdditional; set => _Accuracy = value; }
    public override float RecoilController { get => _RecoilController; set => _RecoilController = value; }
    public override float RecoilCameraController { get => _RecoilCameraController; set => _RecoilCameraController = value; }
    public override float RecoilKickBack { get => _RecoilKickBack; set => _RecoilKickBack = value; }
    public override float min_Precision { get => Min_percision + _min_PrecisionAdditional; set => Min_percision = value; }
    public override float max_Precision { get => Max_percision + _max_PrecisionAdditional; set => Max_percision = value; }
    public override float aimDownSight_speed { get => _AimDownSightSpeed + _aimDownSightSpeedAdditional; set => _AimDownSightSpeed = value; }
    public override Bullet bullet { get ; set ; }
    public override float movementSpeed { get ; set ; }
    public override float drawSpeed { get => this.DrawSpeed ; set => this.DrawSpeed = value; }

    public bool isMagIn { get; set; }

    protected override void Awake()
    {

        fireMode = FireMode.FullAuto;

        bulletStore.Add(BulletStackType.Magazine, bulletCapacity);

        isMagIn = true;

        _556MmBullet = new _556mmBullet(this);
        bullet = _556MmBullet;
        _RecoilKickBack = bullet.recoilKickBack;
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    
    protected override void Update()
    {
        ChamberCount = bulletStore[BulletStackType.Chamber];
        MagCount = bulletStore[BulletStackType.Magazine];
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override WeaponSelector startEventNode { get ; set ; }
    public WeaponSelector reloadStageSelector { get; private set; }
    public ReloadMagazineFullStage reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get ; set; }
    public override RestNode restNode { get; set ; }


    protected override void InitailizedTree()
    {

        startEventNode = new WeaponSelector(this,() => true);

        reloadStageSelector = new WeaponSelector(this,
           () => {
               if (isReloadCommand
              && userWeapon.weaponBelt.ammoProuch.amountOf_ammo[bullet.myType] > 0
              && bulletStore[BulletStackType.Magazine] < bulletCapacity)
                   return true;
               else
                   return false;
           }
           );

        reloadMagazineFullStage = new ReloadMagazineFullStage(this, () => 
        {
            int chamberCount = bulletStore[BulletStackType.Chamber];
            int magCount = bulletStore[BulletStackType.Magazine];
            bool isMagIn = this.isMagIn;

            if
                (
                 isMagIn == true
                && chamberCount == 0
                && magCount == 0
                )
                return true;
            else
                return false;
        });

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
                && (triggerState == TriggerState.Down || triggerState == TriggerState.IsDown); }
            );

        fire = new FiringNode(this,
            () => 
            {
                return bulletStore[BulletStackType.Chamber] > 0;
            }
            );

        autoLoadChamber = new AutoLoadChamberNode(this, 
            () => 
            {
                return true;
            });

        restNode = new RestNode(this,()=>true);


        startEventNode.AddtoChildNode(reloadStageSelector);
        startEventNode.AddtoChildNode(firingAutoLoad);
        startEventNode.AddtoChildNode(restNode);

      

        reloadStageSelector.AddtoChildNode(reloadMagazineFullStage);
        reloadStageSelector.AddtoChildNode(tacticalReloadMagazineFullStage);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        startEventNode.FindingNode(out INodeLeaf eventNode);
        currentEventNode = eventNode as WeaponLeafNode;
    }

    #region WeaponAttachment
    public float _accuracyAdditional { get 
        {
            float total = 0;

            if (_microOptic != null)
                total += _microOptic.accuracy_PN;

            return total;

        } set { } }
    public float _min_PrecisionAdditional { get 
        {
            float total = 0;

            if (_microOptic != null)
                total += _microOptic.min_Precision_PN;

            return total;
        } set { } }
    public float _max_PrecisionAdditional { get 
        {
            float total = 0;

            if (_microOptic != null)
                total += _microOptic.max_Precision_PN;

            return total;
        } set { } }
    public float _aimDownSightSpeedAdditional { get 
        {
            float total = 0;

            if (_microOptic != null)
                total += _microOptic.aimDownSightSpeed_N;

            return total;
        } set { } }

    [SerializeField] private Transform microOpticSocket;
    public Transform _microOpticSocket { get => this.microOpticSocket; set => this.microOpticSocket = value; }
    [SerializeField] private MicroOpticWeaponAttachment microOptic;
    public MicroOpticWeaponAttachment _microOptic { get => this.microOptic; set => microOptic = value; }


    private void OnValidate()
    {
        if(_microOptic != null)
        {
            _microOptic.Attach(this);
        }
    }
    #endregion
}
