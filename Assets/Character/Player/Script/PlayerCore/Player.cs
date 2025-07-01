
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class Player : SubjectPlayer,IWeaponAdvanceUser,
    IBulletDamageAble,
    IAmmoRecivedAble,IHPReciveAble,I_NPCTargetAble
    
{
    public PlayerMovement playerMovement;
    public PlayerHpRegenarate hpRegenarate;
    public CoverDetection coverDetection;
    public PlayerStateNodeManager playerStateNodeManager;

    public Transform RayCastPos;
    public CinemachineCamera cinemachineCamera;
    public Character selfNPCTarget => this;

    [SerializeField] public bool isImortal;

    public float MyHP;
   
    private void BlackBoardBufferUpdate()
    {
        _isHolsterWeaponCommand = false;
        _isDrawPrimaryWeaponCommand = false;
        _isDrawSecondaryWeaponCommand = false;
        _isReloadCommand = false;
        isSwapShoulder = false;
        triggerDodgeRoll = false;
        _isPickingUpWeaponCommand = false;
        _isDropWeaponCommand = false;
        _triggerExecuteGunFu = false;
        _isParkourCommand = false;

        if (_triggerGunFu == true)
        {
            triggerGunFuBufferTime -= Time.deltaTime;

            if (triggerGunFuBufferTime <= 0)
            {
                _triggerGunFu = false;
                triggerGunFuBufferTime = 2;
            }
        }

    }
    protected override void Awake()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        animator = GetComponent<Animator>();
        playerMovement = new PlayerMovement(this);
        coverDetection = new CoverDetection();
        hpRegenarate = new PlayerHpRegenarate(this);

        curShoulderSide = ShoulderSide.Right;

        base.maxHp = 100;
        base.SetHP(100);
        playerStateNodeManager = new PlayerStateNodeManager(this);

        InitailizedGunFuComponent();

        Initialized_IWeaponAdvanceUser();

        playerBulletDamageAbleBehavior = new PlayerBulletDamageAbleBehavior(this);

        aimPosRef.transform.SetParent(null, true);
    }
    protected override void Start()
    {
       

    }
    private void Update()
    {

        inputMoveDir_World = TransformLocalToWorldVector(new Vector3(inputMoveDir_Local.x,0,inputMoveDir_Local.y),Camera.main.transform.forward);
        _gunFuAimDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        UpdateDetectingTarget();

        playerStateNodeManager.UpdateNode();
        _weaponManuverManager.UpdateNode();

        playerMovement.MovementUpdate();
        hpRegenarate.Regenarate();
        MyHP = base.HP;

        _triggerHitedGunFu = false;

    }
    private void LateUpdate()
    {
        BlackBoardBufferUpdate();
    }

    private void FixedUpdate()
    {
        playerStateNodeManager.FixedUpdateNode();
        _weaponManuverManager.FixedUpdateNode();
        playerMovement.MovementFixedUpdate();
    }
    
  
   
    #region ImplementBulletDamageAble
    public PlayerBulletDamageAbleBehavior playerBulletDamageAbleBehavior;
    public void TakeDamage(IDamageVisitor damageVisitor) => playerBulletDamageAbleBehavior.TakeDamage(damageVisitor);
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce) => playerBulletDamageAbleBehavior.TakeDamage(damageVisitor,hitPos,hitDir,hitforce);

    #endregion

    #region InitailizedWeaponAdvanceUser

    [SerializeField] private MainHandSocket MainHandSocket;
    [SerializeField] private SecondHandSocket SecondHandSocket;
    [SerializeField] private PrimaryWeaponSocket PrimaryWeaponSocket;
    [SerializeField] private SecondaryWeaponSocket SecondaryWeaponSocket;

    public CrosshairController crosshairController;
    public enum ShoulderSide
    {
        Left,
        Right
    }
    public ShoulderSide curShoulderSide;
    public MainHandSocket _mainHandSocket { get => this.MainHandSocket; set => this.MainHandSocket = value; }
    public SecondHandSocket _secondHandSocket { get => this.SecondHandSocket; set => this.SecondHandSocket = value; }

    public bool _isPullTriggerCommand { get; set; }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool isSwapShoulder;
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }

    public Weapon _currentWeapon { get; set; }
    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public WeaponManuverManager _weaponManuverManager { get ; set ; }
    public Vector3 _shootingPos { get 
        { 
            if((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>()) 
            {
                Ray ray = new Ray(_currentWeapon.bulletSpawnerPos.position, _currentWeapon.bulletSpawnerPos.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, 0))
                    return hitInfo.point;
                else
                    return ray.GetPoint(100);
            }    
            return crosshairController.CrosshiarShootpoint.GetShootPointDirection();
        } set { } }
    public Vector3 _pointingPos { get => crosshairController.CrosshiarShootpoint.GetPointDirection(); set { } }
    public Animator _weaponUserAnimator { get; set; }
    public Character _userWeapon { get => this;}
    [SerializeField] private AnimatorOverrideController AnimatorOverrideController;
    public AnimatorOverrideController _animatorWeaponAdvanceUserOverride { get => this.AnimatorOverrideController; set => this.AnimatorOverrideController = value; }
    public FindingWeaponBehavior _findingWeaponBehavior { get ; set ; }
    public void Initialized_IWeaponAdvanceUser()
    {
        _shootingPos = new Vector3();

        _weaponUserAnimator = animator;
        _findingWeaponBehavior = new FindingWeaponBehavior(this);
        _weaponBelt = new WeaponBelt(PrimaryWeaponSocket, SecondaryWeaponSocket, new AmmoProuch(45, 45, 30, 30
            ,45,45,60,60));
        _weaponAfterAction = new WeaponAfterActionPlayer(this);

        _weaponManuverManager = new PlayerWeaponManuver(this,this);
    }
    #endregion

    #region ProceduralAim_Lean

    [SerializeField] private Transform aimPosRef;
    public Transform _aimPosRef { get => aimPosRef; set => aimPosRef = value; }
    #endregion

    #region TransformLocalWorld
    private Vector3 TransformLocalToWorldVector(Vector3 dirChild, Vector3 dirParent)
    {
        float zeta;

        Vector3 Direction;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
    private Vector3 TransformWorldToLocalVector(Vector3 dirChild, Vector3 dirParent)
    {
        Vector3 Direction = Vector3.zero;
        float zeta;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        zeta = -zeta;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }

   

    #endregion

    #region MovementStats

    [Range(0, 100)]
    public float StandMoveAccelerate;
    [Range(0, 100)]
    public float StandMoveMaxSpeed;
    [Range(0, 100)]
    public float StandMoveRotateSpeed;


    [Range(0, 100)]
    public float CrouchMoveAccelerate;
    [Range(0, 100)]
    public float CrouchMoveMaxSpeed;
    [Range(0, 100)]
    public float CrouchMoveRotateSpeed;

    [Range(0, 100)]
    public float sprintAccelerate;
    [Range(0, 100)]
    public float sprintMaxSpeed;
    [Range(0, 100)]
    public float sprintRotateSpeed;

    [Range(0, 100)]
    public float breakDecelerate;
    [Range(0, 100)]
    public float breakMaxSpeed;

    [Range(0, 100)]
    public float aimingRotateSpeed;

    [Range(0, 100)]
    public float dodgeImpluseForce;
    [Range(0, 100)]
    public float dodgeInAirStopForce;
    [Range(0, 100)]
    public float dodgeOnGroundStopForce;

    public bool isInCover { get { return coverDetection.CheckingObstacleToward(RayCastPos.position, Camera.main.transform.forward); } }

    public Vector2 inputLookDir_Local;
    public Vector3 inputLookDir_World;
    public Vector2 inputMoveDir_Local;
    public Vector3 inputMoveDir_World;

    public bool isSprint;
    public bool triggerDodgeRoll;

    public PlayerGetUpStateScriptableObject PlayerGetUpStateScriptableObject;
    public enum PlayerStance { stand, crouch, prone }
    public PlayerStance playerStance = PlayerStance.stand;

    public Transform centreTransform;

    [SerializeField] public AnimationCurve moveWarping;

    #endregion

    #region ImplementIAmmoGetAble & IHpGetAble
    public void Recived(AmmoGetAbleObject ammoGetAbleObject)
    {
        NotifyObserver(this, NotifyEvent.RecivedAmmo);
    }

    void IHPReciveAble.Recived(HpGetAbleObject hpGetAbleObject)
    {
        NotifyObserver(this, NotifyEvent.RecivedHp);
    }

    public bool PreCondition(ItemObject itemObject)
    {
        switch (itemObject)
        {
            case AmmoGetAbleObject ammoRecivedAble: 
                {
                    if (_weaponBelt.ammoProuch.amountOf_ammo[BulletType._9mm] < _weaponBelt.ammoProuch.maximunAmmo[BulletType._9mm]
                        || _weaponBelt.ammoProuch.amountOf_ammo[BulletType._45mm] < _weaponBelt.ammoProuch.maximunAmmo[BulletType._45mm]
                        || _weaponBelt.ammoProuch.amountOf_ammo[BulletType._556mm] < _weaponBelt.ammoProuch.maximunAmmo[BulletType._556mm]
                        || _weaponBelt.ammoProuch.amountOf_ammo[BulletType._762mm] < _weaponBelt.ammoProuch.maximunAmmo[BulletType._762mm])
                        return true;
                }
                break;
            case HpGetAbleObject hpReciveAble: 
                {
                    if(GetHP()< maxHp)
                        return true;
                }
                break;
        }
        return false;
    }

   
    public IWeaponAdvanceUser weaponAdvanceUser { get => this; }
    Transform IRecivedAble.transform { get => centreTransform;}
    Character IHPReciveAble.character { get => this; }

    #endregion

    private void OnValidate()
    {
        crosshairController = FindAnyObjectByType<CrosshairController>();
    }

    private void OnDrawGizmos()
    {
        if(attackedAbleGunFu != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(attackedAbleGunFu._gunFuAttackedAble.position,.15f);
        }

        if(executedAbleGunFu != null)
        {
            Gizmos.color= Color.red;
            Gizmos.DrawSphere(executedAbleGunFu._gunFuAttackedAble.position,.15f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position,.2f);

    }
}

