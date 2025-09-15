using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15 : Weapon, PrimaryWeapon, MagazineType, IBoltBack, IMicroOpticAttachAble
{
    [SerializeField] private Transform FrontGripSocket;

    public int ChamberCount;
    public int MagCount;

    //SetUpStats
    private int _MagazineCapacity = 30;
    private float _RateOfFire = 670;
    private float _ReloadSpeed = 2f;

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
    public Transform forntGripAttachment { get ; set ; }
    public Transform slingAnchor { get ; set ; }
    [SerializeField] private Transform mainHandGripTransform;
    [SerializeField] private Transform secondHnadGripTransform;
    public override Transform _mainHandGripTransform { get => mainHandGripTransform; set { } }
    public override Transform _SecondHandGripTransform { get => secondHnadGripTransform; set => secondHnadGripTransform = value ; }
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



    #region Initialized MagazineType
    [SerializeField] private MagazineWeaponAnimationStateOverrideScriptableObject MagazineWeaponAnimationStateOverrideScriptableObject;
    public MagazineWeaponAnimationStateOverrideScriptableObject magazineWeaponAnimationStateOverrideScriptableObject 
    { get => this.MagazineWeaponAnimationStateOverrideScriptableObject ; set => this.MagazineWeaponAnimationStateOverrideScriptableObject = value ; }
    public override WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject 
    { get => this.magazineWeaponAnimationStateOverrideScriptableObject; set => this.magazineWeaponAnimationStateOverrideScriptableObject = value as MagazineWeaponAnimationStateOverrideScriptableObject; }
    public bool _isMagIn { get; set; }
    public Weapon _weapon { get => this; set { } }
    public ReloadMagazineLogic _reloadMagazineLogic { get; set; }
    public override NodeSelector _reloadSelecotrOverriden => this._reloadStageSelector;
    public NodeSelector _reloadStageSelector { get; set; }
    public ReloadMagazineFullStageNodeLeaf _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagazineFullStage { get; set; }
    public void InitailizedReloadStageSelector() => _reloadMagazineLogic.InitailizedReloadStageSelector(this);
    public void ReloadMagazine(MagazineType magazineWeapon, AmmoProuch ammoProuch, IReloadMagazineNode reloadMagazineNode)
        => _reloadMagazineLogic.ReloadMagazine(magazineWeapon, ammoProuch, reloadMagazineNode);

    #endregion
    public override void Initialized()
    {
        fireMode = FireMode.FullAuto;

        _isMagIn = true;

        bulletStore.Add(BulletStackType.Magazine, bulletCapacity);;
        bulletStore.Add(BulletStackType.Chamber, 1);
        _556MmBullet = new _556mmBullet(this);
        bullet = _556MmBullet;
        _RecoilKickBack = bullet.recoilKickBack;
        _reloadMagazineLogic = new ReloadMagazineLogic();
        InitailizedReloadStageSelector();
        base.Initialized();
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

  
    

    #region Initilaized Node

    public override WeaponSelector startEventNode { get ; set ; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get ; set; }
    public override RestNode restNode { get; set ; }

    protected override void InitailizedTree()
    {
        startEventNode = new WeaponSelector(this,() => true);

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

        startEventNode.AddtoChildNode(firingAutoLoad);
        startEventNode.AddtoChildNode(restNode);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        startEventNode.FindingNode(out INodeLeaf eventNode);
        currentEventNode = eventNode as WeaponLeafNode;
    }
    protected override void SetDefaultAttribute()
    {
        bulletStore[BulletStackType.Chamber] = 1;
        bulletStore[BulletStackType.Magazine] = bulletCapacity;
        _isMagIn = true;
        base.SetDefaultAttribute();
    }
    #endregion

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
