using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15 : Weapon, PrimaryWeapon, MagazineType, IMicroOpticAttachAble
{

    //SetUpStats
    private int _MagazineCapacity = 30;

    private RifileBullet _556MmBullet;
    public Transform slingAnchor { get ; set ; }

    public override int maxAmmoCapacity { get => _MagazineCapacity;}
   
    public override float min_CrosshairSize { get => base.min_CrosshairSize  - this._reduceMinCrosshairSize; }
    public override float max_CrosshairSize { get => base.max_CrosshairSize - this._reduceMaxCrosshairSize; }
    public override float aimDownSight_speed { get => base.aimDownSight_speed + _aimDownSightSpeedIncrease; }
    public override Bullet bullet { get ; set ; }

    public override Chamber chamber { get ; protected set ; }
    public override BulletCapacity bulletCap { get ; protected set ; }


    #region Initialized MagazineType
    [SerializeField] private MagazineWeaponAnimationStateOverrideScriptableObject MagazineWeaponAnimationStateOverrideScriptableObject;
    public MagazineWeaponAnimationStateOverrideScriptableObject magazineWeaponAnimationStateOverrideScriptableObject 
    { get => this.MagazineWeaponAnimationStateOverrideScriptableObject ; set => this.MagazineWeaponAnimationStateOverrideScriptableObject = value ; }
    public override WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject 
    { get => this.magazineWeaponAnimationStateOverrideScriptableObject; set => this.magazineWeaponAnimationStateOverrideScriptableObject = value as MagazineWeaponAnimationStateOverrideScriptableObject; }
    public Weapon _weapon { get => this; set { } }
    public ReloadMagazineLogic _reloadMagazineLogic { get; set; }
    public override NodeSelector _reloadSelecotrOverriden => this._reloadStageSelector;
    public NodeSelector _reloadStageSelector { get; set; }
    public ReloadMagazineFullStageNodeLeaf _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagazineFullStage { get; set; }

    [SerializeField] protected TimelineTriggerEventScriptableObject reload_timelineTriggerEventSCRP;
    [SerializeField] protected TimelineTriggerEventScriptableObject tacticalReload_timelineTriggerEventSCRP;
    public TimelineTriggerEventScriptableObject _reload_timelineTriggerEventSCRP => this.reload_timelineTriggerEventSCRP;
    public TimelineTriggerEventScriptableObject _tacticalReload_timelineTriggerEventSCRP => this.tacticalReload_timelineTriggerEventSCRP;
    public void InitailizedReloadStageSelector() => _reloadMagazineLogic.InitailizedReloadStageSelector(this);
    

    #endregion

    public override void Initialized()
    {
        this.bulletCap = new BulletCapacity(this.bullet, this.maxAmmoCapacity);
        this.chamber = new Chamber(this.bullet, this.bulletSpawner, this);

        fireMode = FireMode.FullAuto;

        _556MmBullet = new RifileBullet(this);
        bullet = _556MmBullet;
        _reloadMagazineLogic = new ReloadMagazineLogic();
        InitailizedReloadStageSelector();
        base.Initialized();
    }
  
    
    protected override void Update()
    {
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
               return chamber.isLoad
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

        base.InitailizedNode();
    }

    
    protected override void SetDefaultAttribute()
    {


        this.bulletCap.Load(this.bullet, this.maxAmmoCapacity, out int overAmout);
        this.chamber.Load(this.bullet);
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

    public void ReleseMagazine()
    {
        this.bulletCap = null;
        Notify<Action>(this,ReleseMagazine);

    }

    public void InputMagazine(BulletCapacity bulletCapacity)
    {
        this.bulletCap = bulletCapacity;
        Notify<Action<BulletCapacity>>(this,InputMagazine);
    }

    public void ReloadChamber()
    {
        if (this.chamber.isLoad)
        {
            Debug.LogWarning("Chamber been loaded "+this);
            return;
        }

        if(this.bulletCap == null)
        {
            Debug.LogWarning("Magazine is null " + this);
            return;
        }

        if(this.bulletCap.GetBulletOut(out Bullet loadBullet))
        {
            this.chamber.Load(loadBullet);
            Notify<Action>(this,this.ReloadChamber);
        }
        else
        {
            Debug.LogWarning("Magazine is empty " + this);
        }


    }
}
