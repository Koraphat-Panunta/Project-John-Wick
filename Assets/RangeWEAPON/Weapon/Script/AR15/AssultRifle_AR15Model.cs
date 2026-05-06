using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultRifle_AR15Model : RangeWeapon, PrimaryWeapon, MagazineType
{

    //SetUpStats

    public Transform slingAnchor { get ; set ; }

    public override int maxAmmoCapacity { get => weaponStatsScriptableObject.bulletCapacity;}
   

    public override Bullet bullet { get ; set ; }

    public override Chamber chamber { get ; protected set ; }
    protected override BulletCapacity bulletCap { get ;  set ; }


    #region Initialized MagazineType
    public RangeWeapon _weapon { get => this; set { } }
    public ReloadMagazineLogic _reloadMagazineLogic { get; set; }
    public override NodeSelector _reloadSelecotrOverriden => this._reloadStageSelector;
    public NodeSelector _reloadStageSelector { get; set; }
    public ReloadMagazineFullStageNodeLeaf _reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagazineFullStage { get; set; }
    public ReloadMagazineFullStageNodeLeaf _magInputLoadBarrelReloadMagazineStage { get; set; }
    public ReloadMagazineFullStageNodeLeaf _magInputReloadMagazineStage { get; set; }
    public ReloadMagazineFullStageNodeLeaf _barrelLoadReloadMagazineStage { get; set; }

    [SerializeField] protected TimelineTriggerEventScriptableObject reload_timelineTriggerEventSCRP;
    [SerializeField] protected TimelineTriggerEventScriptableObject tacticalReload_timelineTriggerEventSCRP;
    public TimelineTriggerEventScriptableObject _reload_timelineTriggerEventSCRP => this.reload_timelineTriggerEventSCRP;
    public TimelineTriggerEventScriptableObject _tacticalReload_timelineTriggerEventSCRP => this.tacticalReload_timelineTriggerEventSCRP;
    public void InitailizedReloadStageSelector() => _reloadMagazineLogic.InitailizedReloadStageSelector(this);
    

    #endregion

    public override void Initialized()
    {
        this.bullet = new RifileBullet(this);

        this.bulletCap = new BulletCapacity(this.bullet, this.maxAmmoCapacity);
        this.chamber = new Chamber(this.bullet, this.bulletSpawner, this);

        fireMode = FireMode.FullAuto;

        _reloadMagazineLogic = new ReloadMagazineLogic();
        InitailizedReloadStageSelector();
        base.Initialized();
    }

    [SerializeField] bool isLoad;
    protected override void Update()
    {
        isLoad = this.chamber.isReadyShoot;
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }



    #region Initilaized Node

    public override INodeSelector startNodeSelector { get; set; }

    public AutoLoadChamberNode autoLoadChamber { get ; set; }
    public override WeaponRestNodeLeaf restNode { get; set ; }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        fire = new FiringNode(this, this,
           () =>
           {
               return chamber.isReadyShoot
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
  


    [SerializeField] private Transform FrontGripSocket;
    public Transform forntGripAttachment { get => this.FrontGripSocket; set => this.FrontGripSocket = value; }


    private void OnValidate()
    {
        
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
        if (this.chamber.isReadyShoot)
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
