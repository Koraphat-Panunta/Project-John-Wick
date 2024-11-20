
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer,IWeaponAdvanceUser
{
    //C# Component
    public PlayerController playerController;
    public PlayerAnimation playerAnimation;

    //Class coposition
    //public PlayerWeaponCommand playerWeaponCommand;
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

    List<IPlayerComponent> playerComponents;

    private void Start()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        animator = GetComponent<PlayerAnimation>().animator;
        playerController = new PlayerController(this);
        playerMovement = new PlayerMovement(this);
        //playerWeaponCommand = new PlayerWeaponCommand(this);
        coverDetection = new CoverDetection();
        playerStateManager = new PlayerStateManager(this);
        playerStateManager.SetupState(this);
        playerController.Awake();
        hpRegenarate = new HpRegenarate(this);
        curShoulderSide = ShoulderSide.Right;
        base.SetHP(100 );
        AddObserver(this);
        new WeaponFactorySTI9mm().CreateWeapon(this);
        //if (curentWeapon.TryGetComponent<SecondaryWeapon>(out SecondaryWeapon s))
        //{
        //    secondaryWeapon = s;
        //}
        //new WeaponFactoryAR15().CreateWeapon(this);
        //if (curentWeapon.TryGetComponent<PrimaryWeapon>(out PrimaryWeapon p))
        //{
        //    primaryWeapon = p;
        //}
        //secondaryWeapon.AttachWeaponTo(secondaryHolster);
        //primaryWeapon.AttatchWeaponTo(this);
    }
    private void Update()
    {
        //Detect Cover
        playerStateManager.Update();
        hpRegenarate.Regenarate();
        MyHP = base.HP;
        foreach(IPlayerComponent P in playerComponents)
            P.UpdateComponent();
    }
    private void FixedUpdate()
    {
        playerStateManager.FixedUpdate();
        playerMovement.MovementUpdate();
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
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
        
    public Transform leftHandSocket { get; set; }
    public WeaponBelt weaponBelt { get; set;}
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public Vector3 pointingPos { get; set ;}
    public Animator weaponUserAnimator { get; set; }

    public void Initialized_IWeaponAdvanceUser()
    {
        pointingPos = new Vector3(); 
        currentWeapon = CurrentWeapon;
        currentWeaponSocket = weaponMainSocket;
        leftHandSocket = weaponSecondHandSocket;
        weaponUserAnimator = animator;
        weaponBelt = new WeaponBelt(primaryHolster, secondaryHolster, new AmmoProuch(90, 90, 360, 360));
        weaponAfterAction = new WeaponAfterActionPlayer(this);
        weaponCommand = new WeaponCommand(this);
    }
}
