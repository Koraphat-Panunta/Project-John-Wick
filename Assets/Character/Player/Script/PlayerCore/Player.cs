
using Unity.Cinemachine;
using UnityEngine;

public partial class Player : SubjectPlayer,
    IBulletDamageAble,
    IItemReceiver,
    I_EnemyAITargeted,
    IPowerUpReceiver

{

    public PlayerStateNodeManager playerStateNodeManager;
    public INodeManager stateNodeManager => this.playerStateNodeManager as INodeManager;
    public override MovementCompoent _movementCompoent { get; set; }
    public PlayerMovement playerMovement => _movementCompoent as PlayerMovement;    
    public Transform RayCastPos;
    public ThirdPersonCinemachineCamera cinemachineCamera;
    public Character selfEnemyAIBeenTargeted => this;
    [SerializeField] public bool isImortal;

    public override Gauge _hpGauge { get; protected set; }
    public Gauge staminaGauge { get; protected set; }
    public Gauge executeGauge { get; protected set; }
    public float MyMaxHP;

    public override bool isDead { get 
        {
            if(isImortal)
                return false;

            return base.isDead;
        } }
    public override Stance stance 
    {
        get 
        {
            try
            {
                if ((this.stateNodeManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>(out PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf))
                    || this.stateNodeManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
                    || this.stateNodeManager.TryGetCurNodeLeaf<PlayerBrounceOffNodeLeaf>())
                    return Stance.prone;

                if (this.stateNodeManager.TryGetCurNodeLeaf<PlayerCrouch_Idle_NodeLeaf>()
                    || this.stateNodeManager.TryGetCurNodeLeaf<PlayerCrouch_Move_NodeLeaf>())
                    return Stance.crouch;


                return Stance.stand;
            }
            catch 
            {
                return Stance.stand;
            }
        }
    }
    public CommandBufferManager commandBufferManager;
    public override void Initialized()
    {

        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        this.AddObserver(this);
        commandBufferManager = new CommandBufferManager();
        curShoulderSide = Side.Right;

        this._hpGauge = new Gauge
            (
            this.playerStatsScriptableObject.maxHP
            ,this.playerStatsScriptableObject.maxHP
            );

        this.staminaGauge = new Gauge
            (
            this.playerStatsScriptableObject.maxStamina
            ,this.playerStatsScriptableObject.maxStamina
            );
        this.executeGauge = new Gauge
            (
            0
            ,this.playerStatsScriptableObject.limitExecuteGauge
            );
        this._movementCompoent = new PlayerMovement(
            this
            , transform
            , this
            , this.characterController
            , this.stand_CharacterControllerSCRP
            , this.crouch_CharacterControllerSCRP
            , this.parkour_CharacterControllerSCRP
            );
        playerStateNodeManager = new PlayerStateNodeManager(this);
        Initialized_IWeaponAdvanceUser();
        this.InitializePowerUpReceiver();
        InitailizedGunFuComponent();
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
        _triggerExecute = false;
        _isParkourCommand = false;
        _triggerAttack = false;
        _isTriggerThrowCommand = false;
        isTriggerCrouchStand = false;
        isTriggerQuickShot = false;
 
    }
   
    private void Update()
    {

        this.inputMoveDir_World = TransformLocalToWorldVector(new Vector3(inputMoveDir_Local.x,0,inputMoveDir_Local.y),Camera.main.transform.forward);


        UpdateDetectingTarget();
        UpdateFindingInteractableObject();

        playerStateNodeManager.UpdateNode();
        _weaponManuverManager.UpdateNode();

        _movementCompoent.UpdateNode();
  

        this.commandBufferManager.CommandBufferProcess();

        _triggerHitedGunFu = false;
        debugIsIFrame = (this as I_IFrameAble)._isIFrame;
    }
    [SerializeField] bool debugIsIFrame;
    private void LateUpdate()
    {
        BlackBoardBufferUpdate();
        this.MyMaxHP = base.GetMaxHp();
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

        switch (damageVisitor)
        {
            case MeleeWeapon meleeWeapon:
                {
                    this._triggerHitedGunFu = true;

                    Vector3 rotateDir = (meleeWeapon.transform.position - this.playerMovement.curPosition).normalized;
                    rotateDir = new Vector3(rotateDir.x,0, rotateDir.z).normalized;
                    this._movementCompoent.SetRotateToDirWorldSlerp(rotateDir, 1);
                }
                break;
        }

        if (damageVisitor is IHPDamageVisitor hPDamageVisitor)
            this.AddHP(-hPDamageVisitor._hPDamage);

        this.NotifyObserver(this, NotifyEvent.GetDamaged);

    }
    public void TakeDamageBullet(Bullet damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce) 
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


    public Vector2 inputLookDir_Local;
    public Vector3 inputLookDir_World;
    public Vector2 inputMoveDir_Local;
    public Vector3 inputMoveDir_World;

    public bool isSprint;
    public bool triggerDodgeRoll;
    public bool isTriggerCrouchStand;


    public Transform centreTransform;

    [SerializeField] public AnimationCurve moveWarping;
    [SerializeField] public CharacterMovementControllerScriptableObject stand_CharacterControllerSCRP;
    [SerializeField] public CharacterMovementControllerScriptableObject crouch_CharacterControllerSCRP;
    [SerializeField] public CharacterMovementControllerScriptableObject parkour_CharacterControllerSCRP;

    #endregion

    // ----- ImplementIAmmoGetAble & IHpGetAble removed -----
    // Replaced by PlayerImplementIItemReceiver.cs (unified IItemReceiver capability).
    // Power-up reception unchanged (still routes through PlayerImplementIPowerUpReceiver).


    private void OnValidate()
    {
        crosshairController = FindAnyObjectByType<CrosshairController>();
    }

    
 
}

 