using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Glock17_9mm : Weapon, SecondaryWeapon, MagazineType, IBoltBack
{
    //SetUpStats
    private int _magazineCapacity = 17;
  
    public override int bulletCapacity
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
    public void InitailizedReloadStageSelector() => _reloadMagazineLogic.InitailizedReloadStageSelector(this);
    public void ReloadMagazine(MagazineType magazineWeapon, AmmoProuch ammoProuch, IReloadMagazineNode reloadMagazineNode)
        => _reloadMagazineLogic.ReloadMagazine(magazineWeapon, ammoProuch, reloadMagazineNode);
    #endregion

    public override WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject 
    { get => this.magazineWeaponAnimationStateOverrideScriptableObject; set => magazineWeaponAnimationStateOverrideScriptableObject = value as MagazineWeaponAnimationStateOverrideScriptableObject; }

    public override void Initialized()
    {
        fireMode = FireMode.Single;
        bullet = new _9mmBullet(this);
        bulletStore.Add(BulletStackType.Magazine, bulletCapacity);
        bulletStore.Add(BulletStackType.Chamber, 1);
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
        bulletStore[BulletStackType.Chamber] = 1;
        bulletStore[BulletStackType.Magazine] = bulletCapacity;
        _isMagIn = true;
        base.SetDefaultAttribute();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        fire = new FiringNode(this
            , this
            , () => bulletStore[BulletStackType.Chamber] > 0
            && triggerState == TriggerState.IsDown);

        autoLoadChamber = new AutoLoadChamberNode(this, () => true);

        restNode = new WeaponRestNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(fire);
        startNodeSelector.AddtoChildNode(restNode);

        fire.AddTransitionNode(autoLoadChamber);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
}
