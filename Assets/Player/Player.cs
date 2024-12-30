
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer,IWeaponAdvanceUser,IDamageAble
{

    public PlayerAnimation playerAnimation;
    public PlayerMovement playerMovement;
    public PlayerStateManager playerStateManager;
    public HpRegenarate hpRegenarate;
    public MultiRotationConstraint rotationConstraint;
    public MovementTest movementTest;
    public CoverDetection coverDetection;


    public Transform RayCastPos;

    [SerializeField] private bool isImortal;

    public enum ShoulderSide
    {
        Left,
        Right
    }
    public ShoulderSide curShoulderSide;


    public float MyHP;


    public Vector2 inputLookDir_Local;
    public Vector2 inputMoveDir_Local;
    public bool isSprint;
    public bool isAiming;
    public bool isPullTrigger;
    public bool isReload;
    public bool isSwapShoulder;
    public bool isSwitchWeapon;
    public enum PlayerStance {stand,crouch,prone }
    public PlayerStance playerStance = PlayerStance.stand;
    public bool isInCover { get{return coverDetection.CheckingObstacleToward(RayCastPos.position, RayCastPos.forward); } }
    //public bool isGround;
    private void BlackBoardBufferUpdate()
    {
        isReload = false;
        isSwapShoulder = false;
        isSwitchWeapon = false;
    }

    private void Start()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        animator = GetComponent<PlayerAnimation>().animator;
        playerMovement = new PlayerMovement(this);
        coverDetection = new CoverDetection();
        LeanCover leanCover = new LeanCover(rotationConstraint,crosshairController,this);
        hpRegenarate = new HpRegenarate(this);

        playerStateManager = new PlayerStateManager(this);
        playerStateManager.SetupState(this);

        curShoulderSide = ShoulderSide.Right;
        base.SetHP(100);
        AddObserver(this);

        InitializedPlayerNodeTree();

        Initialized_IWeaponAdvanceUser();

        new WeaponFactorySTI9mm().CreateWeapon(this);
        (weaponBelt.secondaryWeapon as Weapon).AttachWeaponTo(weaponBelt.secondaryWeaponSocket);
        new WeaponFactoryAR15().CreateWeapon(this);

    }


    private void Update()
    {
        //playerStateManager.Update();
        UpdatePlayerTree();
        hpRegenarate.Regenarate();
        MyHP = base.HP;


    }
    private void LateUpdate()
    {
        BlackBoardBufferUpdate();
    }

    private void FixedUpdate()
    {
        //playerStateManager.FixedUpdate();
        FixedUpdatePlayerTree();
        playerMovement.MovementUpdate();
    }


    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        if (isImortal)
            return;
        Bullet bulletObj = damageVisitor as Bullet;
        float damage = bulletObj.hpDamage;

        HP -= damage * 0.21f;
        hpRegenarate.regenarate_countDown = 3;
        NotifyObserver(this,PlayerAction.GetShoot);
        if (GetHP() <= 0)
        {
            NotifyObserver(this, PlayerAction.Dead);
        }
    }

    public void OnNotify(Player player, PlayerAction playerAction)
    {
        if(playerAction == PlayerAction.SwapShoulder)
        {
            if(curShoulderSide == ShoulderSide.Left)
            {
                curShoulderSide = ShoulderSide.Right;
            }
            else if(curShoulderSide == ShoulderSide.Right)
            {
                curShoulderSide = ShoulderSide.Left;
            }
        }
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
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }
    public WeaponBelt weaponBelt { get; set;}
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public Vector3 pointingPos { get 
        { return crosshairController.CrosshiarShootpoint.GetPointDirection(); } set { } }
    public Animator weaponUserAnimator { get; set; }
    public Character userWeapon { get => this;}
    bool IWeaponAdvanceUser.isAiming { get => this.isAiming; set => this.isAiming = value; }
    bool IWeaponAdvanceUser.isPullTrigger { get => this.isPullTrigger; set => this.isPullTrigger = value; }
    bool IWeaponAdvanceUser.isReload { get => this.isReload; set => isReload = value; }
    bool IWeaponAdvanceUser.isSwapShoulder { get => this.isSwapShoulder; set => this.isSwapShoulder = value; }
    bool IWeaponAdvanceUser.isSwitchWeapon { get => this.isSwitchWeapon; set => this.isSwitchWeapon = value; }
    public void Initialized_IWeaponAdvanceUser()
    {
        pointingPos = new Vector3();
        CurrentWeapon = currentWeapon;
        currentWeaponSocket = weaponMainSocket;
        leftHandSocket = weaponSecondHandSocket;
        weaponUserAnimator = animator;
        weaponBelt = new WeaponBelt(primaryHolster, secondaryHolster, new AmmoProuch(90, 90, 360, 360));
        weaponAfterAction = new WeaponAfterActionPlayer(this);
        weaponCommand = new WeaponCommand(this);
    }
    #endregion

    #region Initailized Player Tree node
    public PlayerActionNode curPlayerActionNode { get; private set; }

    public PlayerSelectorNode stanceSelectorNode { get; private set; }

    public PlayerSelectorNode standSelectorNode { get; private set; }

    public PlayerSprintNode playerSprintNode { get; private set; }
    public PlayerSelectorNode standIncoverSelector { get; private set; }
    public PlayerStandIdleNode playerStandIdleNode { get; private set; }
    public PlayerStandMoveNode playerStandMoveNode { get; private set; }
    public PlayerInCoverStandMoveNode playerInCoverStandMoveNode { get;private set; }
    public PlayerInCoverStandIdleNode playerInCoverStandIdleNode { get;private set; }

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


        stanceSelectorNode.AddChildNode(standSelectorNode);

        standSelectorNode.AddChildNode(playerSprintNode);
        standSelectorNode.AddChildNode(standIncoverSelector);
        standSelectorNode.AddChildNode(playerStandMoveNode);
        standSelectorNode.AddChildNode(playerStandIdleNode);

        standIncoverSelector.AddChildNode(playerInCoverStandMoveNode);
        standIncoverSelector.AddChildNode(playerInCoverStandIdleNode);

        stanceSelectorNode.Transition(out PlayerActionNode playerActionNode);
        curPlayerActionNode = playerActionNode;
        Debug.Log("Out PlayerNode = " + playerActionNode);
    }
    private void UpdatePlayerTree()
    {
        if (curPlayerActionNode.IsReset()){
            curPlayerActionNode.Exit();
            curPlayerActionNode = null;
            stanceSelectorNode.Transition(out PlayerActionNode playerActionNode);
            Debug.Log("Out PlayerNode = " + playerActionNode);
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
    #endregion
}
