
using Unity.Cinemachine;
using UnityEngine;

public partial class Player : SubjectPlayer,
    IBulletDamageAble,
    IAmmoRecivedAble,
    IHPReciveAble,
    I_EnemyAITargeted
    
{
    public CoverDetection coverDetection;
    public PlayerStateNodeManager playerStateNodeManager;
    public override MovementCompoent _movementCompoent { get; set; }
    public PlayerMovement playerMovement => _movementCompoent as PlayerMovement;    
    public Transform RayCastPos;
    public CinemachineCamera cinemachineCamera;
    [SerializeField] private CharacterMovementController characterController;
    public Character selfEnemyAIBeenTargeted => this;
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
        base.maxHp = 150;
        base.SetHP(maxHp);

        _movementCompoent = new PlayerMovement(
            this
            , transform
            , this
            , this.characterController
            , this.stand_CharacterControllerSCRP
            , this.crouch_CharacterControllerSCRP
            , this.parkour_CharacterControllerSCRP
            );
        playerStateNodeManager = new PlayerStateNodeManager(this);
        InitailizedGunFuComponent();
        Initialized_IWeaponAdvanceUser();
        playerBulletDamageAbleBehavior = new PlayerBulletDamageAbleBehavior(this);

        base.Initialized();
    }

    private void BlackBoardBufferUpdate()
    {
        _isInteractCommand = false;
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
        _isTriggerThrowCommand = false;
 
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
    public void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce) 
    {
        if ((this as I_IFrameAble)._isIFrame)
            return;

        playerBulletDamageAbleBehavior.TakeDamageBullet(damageVisitor, hitPos, hitDir, hitforce); 
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


    public bool isInCover { get { return coverDetection.CheckingObstacleToward(RayCastPos.position, Camera.main.transform.forward); } }

    public Vector2 inputLookDir_Local;
    public Vector3 inputLookDir_World;
    public Vector2 inputMoveDir_Local;
    public Vector3 inputMoveDir_World;

    public bool isSprint;
    public bool triggerDodgeRoll;
    
    public Stance playerStance = Stance.stand;

    public Transform centreTransform;

    [SerializeField] public AnimationCurve moveWarping;
    [SerializeField] public CharacterMovementControllerScriptableObject stand_CharacterControllerSCRP;
    [SerializeField] public CharacterMovementControllerScriptableObject crouch_CharacterControllerSCRP;
    [SerializeField] public CharacterMovementControllerScriptableObject parkour_CharacterControllerSCRP;

    #endregion

    #region ImplementIAmmoGetAble & IHpGetAble
    public void Recived(AmmoGetAbleObject ammoGetAbleObject)
    {
        NotifyObserver(this, NotifyEvent.RecivedAmmo);
    }

    void IHPReciveAble.Recived(HpGetAbleObject hpGetAbleObject)
    {
        hpGetAbleObject.amoutOfHpAdd = 20f;
        if ((GetHP() / GetMaxHp()) < 0.35f)
        {
            AddHP(Mathf.Abs((maxHp * 0.35f) - GetHP()));
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
                    if (_weaponBelt.ammoProuch.CheckAmmo(BulletType.handgunAmmo) < _weaponBelt.ammoProuch.CheckAmmo(BulletType.handgunAmmo)
                        || _weaponBelt.ammoProuch.CheckAmmo(BulletType.rifleAmmo) < _weaponBelt.ammoProuch.CheckAmmo(BulletType.rifleAmmo)
                        || _weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo) < _weaponBelt.ammoProuch.CheckAmmo(BulletType.buckShotAmmo)
                        || _weaponBelt.ammoProuch.CheckAmmo(BulletType.battleRifleAmmo) < _weaponBelt.ammoProuch.CheckAmmo(BulletType.battleRifleAmmo)
                        )
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

 