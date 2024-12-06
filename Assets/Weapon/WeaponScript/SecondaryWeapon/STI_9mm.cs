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
    public override int Magazine_capacity
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
    public float quickDrawTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override Bullet bullet { get; set; }
    public override float movementSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool isMagIn { get; set ; }
    

    protected override void FixedUpdate()
    {
        
        base.FixedUpdate();
    }
    protected override void Update()
    {
      
        base.Update();
    }
    protected override void Start()
    {

        fireMode = FireMode.Single;
        bullet = new _9mmBullet();
        RecoilKickBack = bullet.recoilKickBack;

        bulletStore.Add(BulletStackType.Magazine, Magazine_capacity);
        base.Start();
        
    }
    public override WeaponSelector startStanceNode { get; set; }
    public override WeaponSelector startEventNode { get; set; }
    public WeaponSelector reloadStageSelector { get; private set; }
    public ReloadMagazineFullStage reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get; set; }
    private AimDownSightNode aimDownSight;
    private LowReadyNode lowReady;
    private RestNode restNode;
    protected override void InitailizedTree()
    {
        startStanceNode = new WeaponSelector(this, () => true);

        startEventNode = new WeaponSelector(this, () => true);

        reloadStageSelector = new WeaponSelector(this,
           () => {
               bool reload = isReloadCommand;
               isReloadCommand = false;
               return reload && bulletStore[BulletStackType.Magazine] < Magazine_capacity;
           }
           );

        reloadMagazineFullStage = new ReloadMagazineFullStage(this);

        tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStage(this);

        firingAutoLoad = new WeaponSequenceNode(this,
            () => { return bulletStore[BulletStackType.Chamber] > 0 && triggerState == TriggerState.Down; }
            );

        fire = new FiringNode(this);

        autoLoadChamber = new AutoLoadChamberNode(this);

        aimDownSight = new AimDownSightNode(this);

        lowReady = new LowReadyNode(this);

        restNode = new RestNode(this);



        startEventNode.AddChildNode(reloadStageSelector);
        startEventNode.AddChildNode(firingAutoLoad);
        startEventNode.AddChildNode(restNode);

        startStanceNode.AddChildNode(aimDownSight);
        startStanceNode.AddChildNode(lowReady);
        startStanceNode.AddChildNode(restNode);

        reloadStageSelector.AddChildNode(reloadMagazineFullStage);
        reloadStageSelector.AddChildNode(tacticalReloadMagazineFullStage);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        startEventNode.Transition(out WeaponActionNode eventNode);
        currentEventNode = eventNode;
        startStanceNode.Transition(out WeaponActionNode stanceNode);
        currentStanceNode = stanceNode;
    }
}
