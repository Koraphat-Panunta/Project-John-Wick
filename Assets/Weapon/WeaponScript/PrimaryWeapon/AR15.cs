using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AR15 :Weapon, PrimaryWeapon,MagazineType,IBlowBack
{

    [SerializeField] private Transform MuzzleSocket;
    [SerializeField] private Transform GripSocket;
    [SerializeField] private Transform Scope;
    [SerializeField] private Transform Stock;
    [SerializeField] private Transform Magazine;
    [SerializeField] private Transform Laser;

    public int ChamberCount;
    public int MagCount;

    //SetUpStats
    private int _MagazineCapacity = 30;
    private float _RateOfFire = 720;
    private float _ReloadSpeed = 2;
    private float _Accuracy = 112;
    private float _RecoilController = 40;
    private float _RecoilCameraController = 30f;
    private float _AimDownSightSpeed = 2.4f;
    private float _RecoilKickBack = 60;
    private float Min_percision = 11;
    private float Max_percision = 74;
    private _556mmBullet _556MmBullet = new _556mmBullet();

    public Transform forntGrip { get ; set ; }
    public Transform slingAnchor { get ; set ; }

    public override int Magazine_capacity { get => _MagazineCapacity; set => _MagazineCapacity = value; }
    public override float rate_of_fire { get => _RateOfFire; set => _RateOfFire = value; }
    public override float reloadSpeed { get => _ReloadSpeed; set => _ReloadSpeed = value; }
    public override float Accuracy { get => _Accuracy; set => _Accuracy = value; }
    public override float RecoilController { get => _RecoilController; set => _RecoilController = value; }
    public override float RecoilCameraController { get => _RecoilCameraController; set => _RecoilCameraController = value; }
    public override float RecoilKickBack { get => _RecoilKickBack; set => _RecoilKickBack = value; }
    public override float min_Precision { get => Min_percision; set => Min_percision = value; }
    public override float max_Precision { get => Max_percision; set => Max_percision = value; }
    public override float aimDownSight_speed { get => _AimDownSightSpeed; set => _AimDownSightSpeed = value; }
    public override Bullet bullet { get ; set ; }
    public override float movementSpeed { get ; set ; }

    public bool isMagIn { get; set; }

    private void Awake()
    {
        bullet = _556MmBullet;
        RecoilKickBack = bullet.recoilKickBack;
    }
    protected override void Start()
    {

        weaponSlotPos.Add(AttachmentSlot.MUZZLE,MuzzleSocket);
        weaponSlotPos.Add(AttachmentSlot.GRIP,GripSocket);
        weaponSlotPos.Add(AttachmentSlot.SCOPE,Scope);
        weaponSlotPos.Add(AttachmentSlot.STOCK,Stock);
        weaponSlotPos.Add(AttachmentSlot.MAGAZINE,Magazine);
        weaponSlotPos.Add(AttachmentSlot.LASER,Laser);

        attachment.Add(AttachmentSlot.MUZZLE, null);
        attachment.Add(AttachmentSlot.GRIP, null);
        attachment.Add(AttachmentSlot.SCOPE, null);
        attachment.Add(AttachmentSlot.STOCK, null);
        attachment.Add(AttachmentSlot.MAGAZINE, null);
        attachment.Add(AttachmentSlot.LASER, null);
       
        fireMode = FireMode.FullAuto;

        bulletStore.Add(BulletStackType.Magazine, Magazine_capacity);

        isMagIn = true;
        
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
    public override WeaponSelector startStanceNode { get ; set ; }
    public override WeaponSelector startEventNode { get ; set ; }
    public WeaponSelector reloadStageSelector { get; private set; }
    public ReloadMagazineFullStage reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    private FiringNode fire;
    public AutoLoadChamberNode autoLoadChamber { get ; set; }
    private AimDownSightNode aimDownSight;
    private LowReadyNode lowReady;
    private RestNode restNode;


    protected override void InitailizedTree()
    {
        startStanceNode = new WeaponSelector(this,() => true);

        startEventNode = new WeaponSelector(this,() => true);

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
