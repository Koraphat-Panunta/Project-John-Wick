
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer,IWeaponAdvanceUser
{
    //C# Component
    //public PlayerController playerController;
    public PlayerAnimation playerAnimation;
    public PlayerMovement playerMovement;
    public PlayerStateManager playerStateManager;
    public HpRegenarate hpRegenarate;
    public MultiRotationConstraint rotationConstraint;
    public MovementTest movementTest;
    public CoverDetection coverDetection;

    public Transform RayCastPos;

    public enum ShoulderSide
    {
        Left,
        Right
    }
    public ShoulderSide curShoulderSide;
    public float MyHP;

    List<IPlayerComponent> playerComponents = new List<IPlayerComponent>();

    public Vector2 inputLookDir_Local;
    public Vector2 inputMoveDir_Local;
    public bool isSprint;
    public bool isAiming;
    public bool isPullTrigger;
    public bool isReload;
    public bool isSwapShoulder;
    public bool isSwitchWeapon;
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
        //playerController = new PlayerController(this);
        playerMovement = new PlayerMovement(this);
        //playerWeaponCommand = new PlayerWeaponCommand(this);
        coverDetection = new CoverDetection();
        LeanCover leanCover = new LeanCover(rotationConstraint,crosshairController,this);
        playerComponents.Add(leanCover);
        playerStateManager = new PlayerStateManager(this);
        playerStateManager.SetupState(this);
        //playerController.Awake();
        hpRegenarate = new HpRegenarate(this);
        curShoulderSide = ShoulderSide.Right;
        base.SetHP(100 );
        AddObserver(this);
        Initialized_IWeaponAdvanceUser();
        new WeaponFactorySTI9mm().CreateWeapon(this);
        (weaponBelt.secondaryWeapon as Weapon).AttachWeaponTo(weaponBelt.secondaryWeaponSocket);
        new WeaponFactoryAR15().CreateWeapon(this);

    }
    private void Update()
    {
        //Detect Cover
        playerStateManager.Update();
        hpRegenarate.Regenarate();
        MyHP = base.HP;
        if(playerComponents.Count>0)
        foreach(IPlayerComponent P in playerComponents)
            P.UpdateComponent();

        BlackBoardBufferUpdate();
    }
    private void FixedUpdate()
    {
        playerStateManager.FixedUpdate();
        playerMovement.MovementUpdate();
        if (playerComponents.Count > 0)
        foreach (IPlayerComponent P in playerComponents)
            P.FixedUpdateComponent();
    }
    public override void TakeDamage(float Damage)
    {
        hpRegenarate.regenarate_countDown = 3;
        base.TakeDamage(Damage*0.21f);
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
}
