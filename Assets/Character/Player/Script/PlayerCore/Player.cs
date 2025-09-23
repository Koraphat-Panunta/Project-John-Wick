
using Unity.Cinemachine;
using UnityEngine;

public partial class Player : SubjectPlayer,
    IBulletDamageAble,
    IAmmoRecivedAble,IHPReciveAble,I_EnemyAITargeted
    
{
    public CoverDetection coverDetection;
    public PlayerStateNodeManager playerStateNodeManager;
    public override MovementCompoent _movementCompoent { get; set; }
    public Transform RayCastPos;
    public CinemachineCamera cinemachineCamera;
    [SerializeField] private CharacterController characterController;
    public Character selfEnemyAIBeenTargeted => this;
    private TimeControlBehavior timeControlBehavior;
    [SerializeField] public bool isImortal;

    public float MyHP;

    public override bool isDead { get 
        {
            if(isImortal)
                return false;

            return base.isDead;
        } }

    public CommandBufferManager commandBufferManager;
    public override void Initialized()
    {

        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        this.AddObserver(this);
        coverDetection = new CoverDetection();
        commandBufferManager = new CommandBufferManager();
        curShoulderSide = ShoulderSide.Right;
        timeControlBehavior = new TimeControlBehavior(this);
        base.maxHp = 150;
        base.SetHP(maxHp);

        _movementCompoent = new PlayerMovement(this, transform, this, this.characterController);
        playerStateNodeManager = new PlayerStateNodeManager(this);
        InitailizedGunFuComponent();
        Initialized_IWeaponAdvanceUser();
        playerBulletDamageAbleBehavior = new PlayerBulletDamageAbleBehavior(this);

        base.Initialized();
    }

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
        _triggerGunFu = false;
 
    }
   
    private void Update()
    {

        inputMoveDir_World = TransformLocalToWorldVector(new Vector3(inputMoveDir_Local.x,0,inputMoveDir_Local.y),Camera.main.transform.forward);
        _gunFuAimDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;

        UpdateDetectingTarget();
        UpdateFindingInteractableObject();

        playerStateNodeManager.UpdateNode();
        _weaponManuverManager.UpdateNode();

        _movementCompoent.UpdateNode();
        MyHP = base.HP;

        commandBufferManager.CommandBufferProcess();
        this.RegenHPUpdate();

        _triggerHitedGunFu = false;
        debugIsIFrame = (this as I_IFrameAble)._isIFrame;
    }
    [SerializeField] bool debugIsIFrame;
    private void LateUpdate()
    {
        BlackBoardBufferUpdate();
    }

    private void FixedUpdate()
    {
        playerStateNodeManager.FixedUpdateNode();
        _weaponManuverManager.FixedUpdateNode();
        _movementCompoent.FixedUpdateNode();
    }
    
  
   
    #region ImplementBulletDamageAble
    public PlayerBulletDamageAbleBehavior playerBulletDamageAbleBehavior;
    public float penatrateResistance { get => 10; set { } }
    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        if((this as I_IFrameAble)._isIFrame)
            return;

        playerBulletDamageAbleBehavior.TakeDamage(damageVisitor);
    }
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce) 
    {
        if ((this as I_IFrameAble)._isIFrame)
            return;

        playerBulletDamageAbleBehavior.TakeDamage(damageVisitor, hitPos, hitDir, hitforce); 
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
    
    public Stance playerStance = Stance.stand;

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
        hpGetAbleObject.amoutOfHpAdd = 35f;
        if ((GetHP() / GetMaxHp()) < 0.5f)
        {
            AddHP(Mathf.Abs((maxHp * 0.5f) - GetHP()));
            AddHP(hpGetAbleObject.amoutOfHpAdd );
        }
        else
        {
            AddHP(hpGetAbleObject.amoutOfHpAdd);

        }
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
 
}

 