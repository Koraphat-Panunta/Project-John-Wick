using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class STI_9mm : Weapon, SecondaryWeapon, MagazineType, IBoltBack
{
    //SetUpStats
    private int _magazineCapacity = 15;
    private float _rateOfFire = 400;
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

    [SerializeField] private Transform mainHandGripTransform;
    [SerializeField] private Transform SecondHandGripTransform;
    public override Transform _mainHandGripTransform { get => mainHandGripTransform; set { } }
    public override Transform _SecondHandGripTransform { get => SecondHandGripTransform; set { } }

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

        _reloadMagazineLogic = new ReloadMagazineLogic();
        InitailizedReloadStageSelector();

        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    public override WeaponSelector startEventNode { get; set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get; set; }
    public override RestNode restNode { get ; set ; }
   

    protected override void InitailizedTree()
    {

        startEventNode = new WeaponSelector(this, () => true);

        firingAutoLoad = new WeaponSequenceNode(this,
            () => {
                return bulletStore[BulletStackType.Chamber] > 0
                && triggerState == TriggerState.IsDown;
            }
            );

        fire = new FiringNode(this, () =>bulletStore[BulletStackType.Chamber] > 0);

        autoLoadChamber = new AutoLoadChamberNode(this,()=>true);

        restNode = new RestNode(this,()=>true);

        startEventNode.AddtoChildNode(firingAutoLoad);
        startEventNode.AddtoChildNode(restNode);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        startEventNode.FindingNode(out INodeLeaf eventNode);
        currentEventNode = eventNode as WeaponLeafNode ;
     
    }

   
}
