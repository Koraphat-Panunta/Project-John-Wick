using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Glock17_9mm : Weapon, SecondaryWeapon, MagazineType
{
    //SetUpStats
    private int _magazineCapacity = 17;
  
    public override int maxAmmoCapacity
    {
        get { return _magazineCapacity; }
    }
    public override Bullet bullet { get; set; }


    #region Initialized MagazineType
    [SerializeField] private MagazineWeaponAnimationStateOverrideScriptableObject MagazineWeaponAnimationStateOverrideScriptableObject;
    public MagazineWeaponAnimationStateOverrideScriptableObject magazineWeaponAnimationStateOverrideScriptableObject 
    { get => this.MagazineWeaponAnimationStateOverrideScriptableObject ; set => MagazineWeaponAnimationStateOverrideScriptableObject = value ; }
    public Weapon _weapon { get => this; set { } }
    public bool _isMagIn { get { return true; } set { } }
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
    public void ReleseMagazine()
    {
        this.bulletCap = null;
        Notify<Action>(this, ReleseMagazine);

    }

    public void InputMagazine(BulletCapacity bulletCapacity)
    {
        this.bulletCap = bulletCapacity;
        Notify<Action<BulletCapacity>>(this, InputMagazine);
    }
    public void ReloadChamber()
    {
        if (this.chamber.isLoad)
        {
            Debug.LogWarning("Chamber been loaded " + this);
            return;
        }

        if (this.bulletCap == null)
        {
            Debug.LogWarning("Magazine is null " + this);
            return;
        }

        if (this.bulletCap.GetBulletOut(out Bullet loadBullet))
        {
            this.chamber.Load(loadBullet);
            Notify<Action>(this, this.ReloadChamber);
        }
        else
        {
            Debug.LogWarning("Magazine is empty " + this);
        }


    }
    #endregion
    public override Chamber chamber { get;protected set; }
    public override BulletCapacity bulletCap { get; protected set; }

    public override WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject 
    { get => this.magazineWeaponAnimationStateOverrideScriptableObject; set => magazineWeaponAnimationStateOverrideScriptableObject = value as MagazineWeaponAnimationStateOverrideScriptableObject; }

    public override void Initialized()
    {
        this.bulletCap = new BulletCapacity(this.bullet, this.maxAmmoCapacity);
        this.chamber = new Chamber(this.bullet, this.bulletSpawner, this);

        fireMode = FireMode.Single;
        bullet = new HandgunBullet(this);
        _isMagIn = true;
        _reloadMagazineLogic = new ReloadMagazineLogic();
        InitailizedReloadStageSelector();
        base.Initialized();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void Update()
    {
        base.Update();
    }

   

    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get; set; }
    public override WeaponRestNodeLeaf restNode { get ; set ; }
    public override INodeSelector startNodeSelector { get; set; }
   

    protected override void SetDefaultAttribute()
    {
        this.bulletCap = new BulletCapacity(this.bullet,this.maxAmmoCapacity);
        this.chamber = new Chamber(this.bullet, this.bulletSpawner, this);

        this.bulletCap.GetBulletOut(out Bullet lordBullet);
        this.chamber.Load(lordBullet);
        _isMagIn = true;
        base.SetDefaultAttribute();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        fire = new FiringNode(this
            , this
            , () => this.chamber.isLoad
            && triggerState == TriggerState.IsDown);

        autoLoadChamber = new AutoLoadChamberNode(this, () => true);

        restNode = new WeaponRestNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(fire);
        startNodeSelector.AddtoChildNode(restNode);

        fire.AddTransitionNode(autoLoadChamber);

        _nodeManagerBehavior.SearchingNewNode(this);

        base.InitailizedNode();
    }
}
