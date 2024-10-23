
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer
{
    //C# Component
    public PlayerController playerController;
    public PlayerAnimation playerAnimation;

    //Class coposition
    public PlayerWeaponCommand playerWeaponCommand;
    public PlayerMovement playerMovement;
    public PlayerStateManager playerStateManager;
    public HpRegenarate hpRegenarate;

    public MultiRotationConstraint rotationConstraint;

    public MovementTest movementTest;
    public CoverDetection coverDetection;

    public Transform RayCastPos;

    public PrimaryWeapon primaryWeapon;
    public SecondaryWeapon secondaryWeapon;

    public Transform primaryHolster;
    public Transform secondaryHolster;

    public enum ShoulderSide
    {
        Left,
        Right
    }
    public ShoulderSide curShoulderSide;
    public float MyHP;

    private void Start()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//
        animator = GetComponent<PlayerAnimation>().animator;
        playerController = new PlayerController(this);
        playerMovement = new PlayerMovement(this);
        playerWeaponCommand = new PlayerWeaponCommand(this);
        coverDetection = new CoverDetection();
        playerStateManager = new PlayerStateManager(this);
        playerStateManager.SetupState(this);
        playerController.Awake();
        hpRegenarate = new HpRegenarate(this);
        curShoulderSide = ShoulderSide.Right;
        base.SetHP(100);
        AddObserver(this);
        new WeaponFactorySTI9mm().CreateWeapon(this);
        if (curentWeapon.TryGetComponent<SecondaryWeapon>(out SecondaryWeapon s))
        {
            secondaryWeapon = s;
        }
        new WeaponFactoryAR15().CreateWeapon(this);
        if (curentWeapon.TryGetComponent<PrimaryWeapon>(out PrimaryWeapon p))
        {
            primaryWeapon = p;
        }
        secondaryWeapon.AttachWeaponTo(secondaryHolster);
        primaryWeapon.AttatchWeaponTo(this);
    }
    private void Update()
    {
        //Detect Cover
        playerStateManager.Update();
        hpRegenarate.Regenarate();
        MyHP = base.HP;
    }
    private void FixedUpdate()
    {
        playerStateManager.FixedUpdate();
        playerMovement.MovementUpdate();
    }
    public override void Aiming(Weapon weapon)
    {
        RotateObjectToward rotateObjectToward = new RotateObjectToward();
        rotateObjectToward.RotateTowards(Camera.main.transform.forward,gameObject,6);
        NotifyObserver(this, PlayerAction.Aim);
        base.Aiming(weapon);
    }

    public override void Firing(Weapon weapon)
    {
        NotifyObserver(this, PlayerAction.Firing);
        base.Firing(weapon);
    }

    public override void LowReadying(Weapon weapon)
    {
        if (weapon != null)
        {
            NotifyObserver(this, PlayerAction.LowReady);
            base.LowReadying(weapon);
        }
        else
        {
            NotifyObserver(this, PlayerAction.LowReady);
        }
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        if (reloadType == global::Reload.ReloadType.ReloadFinished)
        {
            playerWeaponCommand.ammoProuch.prochReload.Performed(weapon);
        }
        NotifyObserver(this, PlayerAction.Reloading);
        base.Reloading(weapon, reloadType);
    }
    public override void TakeDamage(float Damage)
    {
        hpRegenarate.regenarate_countDown = 3;
        base.TakeDamage(Damage*0.6f);
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
}
