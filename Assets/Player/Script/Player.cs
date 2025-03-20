
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer,IWeaponAdvanceUser,
    IBulletDamageAble,IAimingProceduralAnimate,IGunFuAble,
    IAmmoRecivedAble,IHPReciveAble,I_NPCTargetAble
{
    public PlayerMovement playerMovement;
    public HpRegenarate hpRegenarate;
    public CoverDetection coverDetection;
    public PlayerStateNodeManager playerStateNodeManager;

    public Transform RayCastPos;
    public CinemachineCamera cinemachineCamera;

    public Character selfNPCTarget => this;

    [SerializeField] public bool isImortal { get; private set; }

    public float MyHP;
   
    private void BlackBoardBufferUpdate()
    {
        isReloadCommand = false;
        isSwapShoulder = false;
        isSwitchWeaponCommand = false;
        triggerDodgeRoll = false;
        isPickingUpWeaponCommand = false;
        isDropWeaponCommand = false;

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
    private void Awake()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        animator = GetComponent<Animator>();
        playerMovement = new PlayerMovement(this);
        coverDetection = new CoverDetection();
        hpRegenarate = new HpRegenarate(this);

        curShoulderSide = ShoulderSide.Right;

        base.maxHp = 100;
        base.SetHP(100);

        AddObserver(this);

        InitailizedGunFuComponent();

        Initialized_IWeaponAdvanceUser();

        InitializedAimingProceduralAnimate();

        //new WeaponFactorySTI9mm().CreateWeapon(this);
        //(weaponBelt.secondaryWeapon as Weapon).AttachWeaponToSocket(weaponBelt.secondaryWeaponSocket);
        //new WeaponFactoryAR15().CreateWeapon(this);

        playerBulletDamageAbleBehavior = new PlayerBulletDamageAbleBehavior(this);
        playerStateNodeManager = new PlayerStateNodeManager(this);
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
        weaponManuverManager.UpdateNode();

        playerMovement.MovementUpdate();
        hpRegenarate.Regenarate();
        MyHP = base.HP;

    }
    private void LateUpdate()
    {
        BlackBoardBufferUpdate();
    }

    private void FixedUpdate()
    {
        playerStateNodeManager.FixedUpdateNode();
        weaponManuverManager.FixedUpdateNode();
        playerMovement.MovementFixedUpdate();
    }

  
    public void OnNotify(Player player, PlayerAction playerAction)
    {
        
    }

    public void OnNotify(Player player)
    {
    }

    #region ImplementBulletDamageAble
    public PlayerBulletDamageAbleBehavior playerBulletDamageAbleBehavior;
    public void TakeDamage(IDamageVisitor damageVisitor) => playerBulletDamageAbleBehavior.TakeDamage(damageVisitor);
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce) => playerBulletDamageAbleBehavior.TakeDamage(damageVisitor,hitPos,hitDir,hitforce);

    #endregion

    #region InitailizedWeaponAdvanceUser

    //[SerializeField] private PrimaryWeapon primaryWeapon;
    //[SerializeField] private SecondaryWeapon secondaryWeapon;
    [SerializeField] private Transform primaryHolster;
    [SerializeField] private Transform secondaryHolster;
    [SerializeField] private Transform weaponMainSocket;
    [SerializeField] private Transform weaponSecondHandSocket;
    public CrosshairController crosshairController;
    public enum ShoulderSide
    {
        Left,
        Right
    }
    public ShoulderSide curShoulderSide;
    public bool isSwitchWeaponCommand { get; set; }
    public bool isPullTriggerCommand { get; set; }
    public bool isAimingCommand { get; set; }
    public bool isReloadCommand { get; set; }
    public bool isSwapShoulder;
    public bool isPickingUpWeaponCommand { get; set; }
    public bool isDropWeaponCommand { get; set; }
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }
    public WeaponBelt weaponBelt { get; set;}
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public WeaponManuverManager weaponManuverManager { get ; set ; }
    public Vector3 shootingPos { get 
        { return crosshairController.CrosshiarShootpoint.GetShootPointDirection(); } set { } }
    public Vector3 pointingPos { get => crosshairController.CrosshiarShootpoint.GetPointDirection(); set { } }
    public Animator weaponUserAnimator { get; set; }
    public Character userWeapon { get => this;}
    [SerializeField] AnimatorOverrideController AnimatorOverrideController;
    public AnimatorOverrideController _animatorOverride { get; set; }
    public FindingWeaponBehavior findingWeaponBehavior { get ; set ; }
    public void Initialized_IWeaponAdvanceUser()
    {
        shootingPos = new Vector3();
        currentWeaponSocket = weaponMainSocket;
        leftHandSocket = weaponSecondHandSocket;
        weaponUserAnimator = animator;
        weaponBelt = new WeaponBelt(primaryHolster, secondaryHolster, new AmmoProuch(90, 90, 60, 60));
        weaponAfterAction = new WeaponAfterActionPlayer(this);
        weaponCommand = new WeaponCommand(this);
        weaponManuverManager = new PlayerWeaponManuver(this,this);
        findingWeaponBehavior = new FindingWeaponBehavior(this);
        _animatorOverride = this.AnimatorOverrideController;
    }
    #endregion

    #region ProceduralAim_Lean
    [SerializeField] private MultiAimConstraint aimConstraint;
    [SerializeField] private MultiRotationConstraint rotationConstraint;
    [SerializeField] private Transform aimPosRef;
    //[SerializeField] private CrosshairController crosshairController;
    public MultiAimConstraint _aimConstraint { get => aimConstraint; set => aimConstraint = value; }
    public MultiRotationConstraint _rotationConstraint { get => rotationConstraint; set => rotationConstraint = value ; }
    public AimingProceduralAnimate _aimingProceduralAnimate { get; set ; }
    public Transform _aimPosRef { get => aimPosRef; set => aimPosRef = value; }
    public LeanCover _leanCover { get; set ; }
    public CrosshairController _crosshairController { get => crosshairController; }
   

    public void InitializedAimingProceduralAnimate()
    {
        _aimingProceduralAnimate = new AimingProceduralAnimate(this,this,_aimPosRef,_aimConstraint,this);
        _leanCover = new LeanCover(_rotationConstraint, _crosshairController, this);
    }


    #endregion

    #region InitailizedGunFu
    public bool _triggerGunFu { get ; set ; }
    public float triggerGunFuBufferTime { get ; set ; }
    public IWeaponAdvanceUser _weaponUser { get ; set; }
    public Vector3 _gunFuAimDir { get; set; }
    public Transform _gunFuUserTransform { get ; set; }
    public LayerMask _layerTarget { get ; set ; }
    [SerializeField] Transform targetAdjustTranform;
    public Transform _targetAdjustTranform { get; set; }

    [SerializeField] private GunFuDetectTarget GunFuDetectTarget;
    public GunFuDetectTarget gunFuDetectTarget { get => this.GunFuDetectTarget ; set => this.GunFuDetectTarget = value; }
    public IGunFuGotAttackedAble attackedAbleGunFu { get; set; }
    public IGunFuNode curGunFuNode { get 
        {
            if(playerStateNodeManager.curNodeLeaf is IGunFuNode gunFuNode)
                return gunFuNode;
            return null;
        } set { } 
    }

    [SerializeField] public GunFuHitNodeScriptableObject hit1;
    [SerializeField] public GunFuHitNodeScriptableObject hit2;
    [SerializeField] public GunFuHitNodeScriptableObject knockDown;
    [SerializeField] public GunFuHitNodeScriptableObject dodgeSpinKick;
    [SerializeField] public GunFuInteraction_ScriptableObject humanShield;
    [SerializeField] public GunFuInteraction_ScriptableObject humanThrow;

    public void InitailizedGunFuComponent()
    {
        _weaponUser = this;
        _gunFuUserTransform = RayCastPos;
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(0));
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(7));

        _targetAdjustTranform = targetAdjustTranform;
        triggerGunFuBufferTime = 1;
    }
    public void UpdateDetectingTarget()
    {

        if(playerStateNodeManager.curNodeLeaf is IGunFuNode)
            return;

        if(gunFuDetectTarget.CastDetect(out IGunFuGotAttackedAble target))
            attackedAbleGunFu = target;
        else
            attackedAbleGunFu = null;
    }
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

    public enum PlayerStance { stand, crouch, prone }
    public PlayerStance playerStance = PlayerStance.stand;

    public Transform centreTransform;

    #endregion

    #region ImplementIAmmoGetAble & IHpGetAble
    public void Recived(AmmoGetAbleObject ammoGetAbleObject)
    {
        NotifyObserver(this, PlayerAction.RecivedAmmo);
    }

    void IHPReciveAble.Recived(HpGetAbleObject hpGetAbleObject)
    {
        NotifyObserver(this, PlayerAction.RecivedHp);
    }

    public bool PreCondition(ItemObject itemObject)
    {
        switch (itemObject)
        {
            case AmmoGetAbleObject ammoRecivedAble: 
                {
                    if(currentWeapon == null)
                        return false;

                    BulletType primaryType = (weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon).bullet.myType;
                    BulletType secondaryType = (weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon).bullet.myType;

                    if (weaponAdvanceUser.weaponBelt.ammoProuch.amountOf_ammo[primaryType] < weaponBelt.ammoProuch.maximunAmmo[primaryType]
                        || weaponAdvanceUser.weaponBelt.ammoProuch.amountOf_ammo[secondaryType] < weaponBelt.ammoProuch.maximunAmmo[secondaryType])
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


}
