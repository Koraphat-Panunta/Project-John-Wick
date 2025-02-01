
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer,IWeaponAdvanceUser,IBulletDamageAble,IAimingProceduralAnimate,IGunFuAble
{
    public PlayerMovement playerMovement;
    public HpRegenarate hpRegenarate;
    public CoverDetection coverDetection;

    public Transform RayCastPos;

    [SerializeField] private bool isImortal;

    public float MyHP;
   
    private void BlackBoardBufferUpdate()
    {
        isReloadCommand = false;
        isSwapShoulder = false;
        isSwitchWeaponCommand = false;
        _triggerGunFu = false;
    }

    protected override void Start()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        animator = GetComponent<Animator>();
        playerMovement = new PlayerMovement(this);
        coverDetection = new CoverDetection();
        hpRegenarate = new HpRegenarate(this);

        curShoulderSide = ShoulderSide.Right;
        base.SetHP(100);
        AddObserver(this);

        InitailizedGunFuComponent();

        InitializedPlayerNodeTree();

        Initialized_IWeaponAdvanceUser();

        InitializedAimingProceduralAnimate();

        new WeaponFactorySTI9mm().CreateWeapon(this);
        (weaponBelt.secondaryWeapon as Weapon).AttachWeaponTo(weaponBelt.secondaryWeaponSocket);
        new WeaponFactoryAR15().CreateWeapon(this);

    }


    private void Update()
    {
        inputMoveDir_World = TransformLocalToWorldVector(new Vector3(inputMoveDir_Local.x,0,inputMoveDir_Local.y),Camera.main.transform.forward);

        UpdatePlayerTree();
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
        FixedUpdatePlayerTree();
        weaponManuverManager.FixedUpdateNode();
        playerMovement.MovementFixedUpdate();
    }

    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        if (isImortal)
            return;
        Bullet bulletObj = damageVisitor as Bullet;
        float damage = bulletObj.hpDamage;

        HP -= damage * 0.21f;
        hpRegenarate.regenarate_countDown = 3;
        NotifyObserver(this, PlayerAction.GetShoot);

        if (GetHP() <= 0)
            NotifyObserver(this, PlayerAction.Dead);
    }

    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        TakeDamage(damageVisitor);
    }

    public void OnNotify(Player player, PlayerAction playerAction)
    {
        if(playerAction == PlayerAction.SwapShoulder)
        {
            if (curShoulderSide == ShoulderSide.Left)
            { curShoulderSide = ShoulderSide.Right; }

            else if (curShoulderSide == ShoulderSide.Right)
            { curShoulderSide = ShoulderSide.Left; }
        }
    }

    public void OnNotify(Player player)
    {
    }

    #region InitailizedWeaponAdvanceUser

    [SerializeField] private Weapon CurrentWeapon;
    [SerializeField] private PrimaryWeapon primaryWeapon;
    [SerializeField] private SecondaryWeapon secondaryWeapon;
    [SerializeField] private Transform primaryHolster;
    [SerializeField] private Transform secondaryHolster;
    [SerializeField] private Transform weaponMainSocket;
    [SerializeField] private Transform weaponSecondHandSocket;
    [SerializeField] private CrosshairController crosshairController;
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
    public void Initialized_IWeaponAdvanceUser()
    {
        shootingPos = new Vector3();
        CurrentWeapon = currentWeapon;
        currentWeaponSocket = weaponMainSocket;
        leftHandSocket = weaponSecondHandSocket;
        weaponUserAnimator = animator;
        weaponBelt = new WeaponBelt(primaryHolster, secondaryHolster, new AmmoProuch(90, 90, 360, 360));
        weaponAfterAction = new WeaponAfterActionPlayer(this);
        weaponCommand = new WeaponCommand(this);
        weaponManuverManager = new PlayerWeaponManuver(this,this);
    }
    #endregion

    #region Initailized Player Tree node
    public PlayerActionNodeLeaf curPlayerActionNode { get; private set; }

    public PlayerSelectorNode stanceSelectorNode { get; private set; }

    public PlayerSelectorNode standSelectorNode { get; private set; }

    public PlayerSprintNode playerSprintNode { get; private set; }
    public PlayerSelectorNode standIncoverSelector { get; private set; }
    public PlayerStandIdleNode playerStandIdleNode { get; private set; }
    public PlayerStandMoveNode playerStandMoveNode { get; private set; }
    public PlayerInCoverStandMoveNode playerInCoverStandMoveNode { get;private set; }
    public PlayerInCoverStandIdleNode playerInCoverStandIdleNode { get;private set; }

    public Hit1GunFuNode Hit1gunFuNode { get; private set; }
    public Hit2GunFuNode Hit2GunFuNode { get; private set; }
    public KnockDown_GunFuNode knockDown_GunFuNode{ get; private set; }
  

    private void InitializedPlayerNodeTree()
    {
        stanceSelectorNode = new PlayerSelectorNode(this,
            () => { return true; });

        standSelectorNode = new PlayerSelectorNode(this,
            () => { return playerStance == PlayerStance.stand; });

        playerSprintNode = new PlayerSprintNode(this);
        standIncoverSelector = new PlayerSelectorNode(this,
            () => {return isInCover; });
        playerStandMoveNode = new PlayerStandMoveNode(this);
        playerStandIdleNode = new PlayerStandIdleNode(this);

        playerInCoverStandMoveNode = new PlayerInCoverStandMoveNode(this);
        playerInCoverStandIdleNode = new PlayerInCoverStandIdleNode(this);

        Hit1gunFuNode = new Hit1GunFuNode(this,hit1);
        Hit2GunFuNode = new Hit2GunFuNode(this, hit2);
        knockDown_GunFuNode = new KnockDown_GunFuNode(this,knockDown);


        stanceSelectorNode.AddChildNode(standSelectorNode);

        standSelectorNode.AddChildNode(Hit1gunFuNode);    
        standSelectorNode.AddChildNode(playerSprintNode);
        standSelectorNode.AddChildNode(standIncoverSelector);
        standSelectorNode.AddChildNode(playerStandMoveNode);
        standSelectorNode.AddChildNode(playerStandIdleNode);

        standIncoverSelector.AddChildNode(playerInCoverStandMoveNode);
        standIncoverSelector.AddChildNode(playerInCoverStandIdleNode);

        stanceSelectorNode.Transition(out PlayerActionNodeLeaf playerActionNode);
        curPlayerActionNode = playerActionNode;

    }
    private void UpdatePlayerTree()
    {
        if (curPlayerActionNode.IsReset()){
            curPlayerActionNode.Exit();
            curPlayerActionNode = null;
            stanceSelectorNode.Transition(out PlayerActionNodeLeaf playerActionNode);
            curPlayerActionNode = playerActionNode;
            curPlayerActionNode.Enter();
        }

        if(curPlayerActionNode != null)
            curPlayerActionNode.Update();
    }
    private void FixedUpdatePlayerTree()
    {
        if (curPlayerActionNode != null)
            curPlayerActionNode.FixedUpdate();
    }

    public void ChangeNode(PlayerActionNodeLeaf playerActionNodeLeaf)
    {
        curPlayerActionNode.Exit();
        curPlayerActionNode = playerActionNodeLeaf;
        curPlayerActionNode.Enter();
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
    public IWeaponAdvanceUser _weaponUser { get ; set; }
    public Vector3 _gunFuAimDir { get => Camera.main.transform.forward; }

    [Range(0, 10)]
    [SerializeField] private float shpere_Raduis_Detecion;
    public float _shpere_Raduis_Detecion { get => shpere_Raduis_Detecion; set { } }
    [Range(0, 10)]
    [SerializeField] private float sphere_Distance_Detection;
    public float _sphere_Distance_Detection { get => sphere_Distance_Detection; set { } }
    [Range(0, 360)]
    [SerializeField] private float limitAimAngleDegrees;
    public float _limitAimAngleDegrees { get =>limitAimAngleDegrees; set { } }

    public Transform _gunFuUserTransform { get ; set; }
    public LayerMask _layerTarget { get ; set ; }
    [SerializeField] Transform targetAdjustTranform;
    public Transform _targetAdjustTranform { get; set; }
  

    [SerializeField] GunFuHitNodeScriptableObject hit1;
    [SerializeField] GunFuHitNodeScriptableObject hit2;
    [SerializeField] GunFuHitNodeScriptableObject knockDown;
    public void InitailizedGunFuComponent()
    {
        _weaponUser = this;
        _gunFuUserTransform = RayCastPos;
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(0));
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(7));

        _targetAdjustTranform = targetAdjustTranform;
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
    public float moveAccelerate;
    [Range(0, 100)]
    public float moveMaxSpeed;
    [Range(0, 100)]
    public float moveRotateSpeed;

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

    public bool isInCover { get { return coverDetection.CheckingObstacleToward(RayCastPos.position, Camera.main.transform.forward); } }

    public Vector2 inputLookDir_Local;
    public Vector3 inputLookDir_World;
    public Vector2 inputMoveDir_Local;
    public Vector3 inputMoveDir_World;

    public bool isSprint;

    public enum PlayerStance { stand, crouch, prone }
    public PlayerStance playerStance = PlayerStance.stand;

    public Transform centreTransform;

    #endregion

}
