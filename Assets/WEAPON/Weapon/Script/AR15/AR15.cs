using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15 : Weapon, PrimaryWeapon, MagazineType, IBoltBack, IMicroOpticAttachAble
{

    public int ChamberCount;
    public int MagCount;

    //SetUpStats
    private int _MagazineCapacity = 30;

    private _556mmBullet _556MmBullet;
    public Transform slingAnchor { get ; set ; }

    public override int bulletCapacity { get => _MagazineCapacity;}
   
    public override float min_CrosshairSize { get => base.min_CrosshairSize  - this._reduceMinCrosshairSize; }
    public override float max_CrosshairSize { get => base.max_CrosshairSize - this._reduceMaxCrosshairSize; }
    public override float aimDownSight_speed { get => base.aimDownSight_speed + _aimDownSightSpeedIncrease; }
    public override Bullet bullet { get ; set ; }





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

    public override INodeSelector startNodeSelector { get; set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get ; set; }
    public override WeaponRestNodeLeaf restNode { get; set ; }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        fire = new FiringNode(this, this,
           () =>
           {
               return bulletStore[BulletStackType.Chamber] > 0
               && (triggerState == TriggerState.Down || triggerState == TriggerState.IsDown);
           }
           );

        autoLoadChamber = new AutoLoadChamberNode(this,
            () =>
            {
                return true;
            });

        restNode = new WeaponRestNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(fire);
        startNodeSelector.AddtoChildNode(restNode);

        fire.AddTransitionNode(autoLoadChamber);

        this._nodeManagerBehavior.SearchingNewNode(this);
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
  
    public float _reduceMinCrosshairSize
    { get 
        {
            float total = 0;

            if (_microOptic != null)
                total += _microOptic.min_Precision_PN;

            return total;
        } set { } }
    public float _reduceMaxCrosshairSize
    { get 
        {
            float total = 0;

            if (_microOptic != null)
                total += _microOptic.max_Precision_PN;

            return total;
        } set { } }
    public float _aimDownSightSpeedIncrease
    { get 
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

    [SerializeField] private Transform FrontGripSocket;
    public Transform forntGripAttachment { get => this.FrontGripSocket; set => this.FrontGripSocket = value; }

    private void OnValidate()
    {
        if(_microOptic != null)
        {
            _microOptic.Attach(this);
        }
    }

    #endregion
}
