using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AR15 : PrimaryWeapon,MagazineType
{

    [SerializeField] private Transform MuzzleSocket;
    [SerializeField] private Transform GripSocket;
    [SerializeField] private Transform Scope;
    [SerializeField] private Transform Stock;
    [SerializeField] private Transform Magazine;
    [SerializeField] private Transform Laser;

    [SerializeField] private Muzzle muzzle;
    [SerializeField] private Sight sight;
    //SetUpStats
    private int _MagazineCapacity = 30;
    private float _RateOfFire = 720;
    private float _ReloadSpeed = 2;
    private float _Accuracy = 112;
    [SerializeField] private GameObject _bulletType;
    private float _RecoilController = 40;
    private float _RecoilCameraController = 30f;
    private float _AimDownSightSpeed = 2.4f;
    private float _RecoilKickBack = 60;
    private float Min_percision = 11;
    private float Max_percision = 74;
    private _556mmBullet _556MmBullet = new _556mmBullet();
    public override Transform forntGrip { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override Transform slingAnchor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
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
    public override float movementSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isMagIn { get; set; }
    public override WeaponSelector startNode { get; set; }

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
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public WeaponSelector stanceSelector { get; private set; }
    public ReloadStageSelector reloadStageSelector { get; private set; }
    public WeaponSequenceNode firingAutoLoad { get; private set; }
    public ReloadMagazineFullStage reloadMagazineFullStage { get; private set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; private set; }

    private AimDownSightNode aimDownSight;
    private LowReadyNode lowReady;
    private FiringNode fire;
    private AutoLoadChamberNode autoLoadChamber;
    protected override void InitailizedTree()
    {
        reloadMagazineFullStage = new ReloadMagazineFullStage(this);
        tacticalReloadMagazineFullStage = new TacticalReloadMagazineFullStage(this);
        startNode = new WeaponSelector(this, () => true);

        reloadStageSelector = new ReloadStageSelector(this);
        stanceSelector = new WeaponSelector(this,
            () => { return true; }
            );
        firingAutoLoad = new WeaponSequenceNode(this,
            () => { return bulletStore[BulletStackType.Chamber] > 0 && triggerState == TriggerState.Down; }
            );

        aimDownSight = new AimDownSightNode(this);
        lowReady = new LowReadyNode(this);
        fire = new FiringNode(this);
        autoLoadChamber = new AutoLoadChamberNode(this);

        startNode.AddChildNode(stanceSelector);

        stanceSelector.AddChildNode(reloadStageSelector);
        stanceSelector.AddChildNode(aimDownSight);
        stanceSelector.AddChildNode(lowReady);

        reloadStageSelector.AddChildNode(reloadMagazineFullStage);
        reloadStageSelector.AddChildNode(tacticalReloadMagazineFullStage);

        aimDownSight.AddChildNode(firingAutoLoad);

        firingAutoLoad.AddChildNode(fire);
        firingAutoLoad.AddChildNode(autoLoadChamber);

        currentNode = startNode;

    }
}
