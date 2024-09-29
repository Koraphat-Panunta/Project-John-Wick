
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : SubjectPlayer,IObserverPlayer
{
    //C# Component
    public PlayerController playerController;
    public WeaponSocket weaponSocket;
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

    public enum ShoulderSide
    {
        Left,
        Right
    }
    private ShoulderSide curShoulderSide;
    public float MyHP;

    private void Start()
    {
        //_+_+_+_+_+_ SetUp Queqe Order _+_+_+_+_+_//

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
    }
    private void Update()
    {
        //Detect Cover
        if(coverDetection.CheckingObstacleToward(RayCastPos.transform.position,RayCastPos.transform.forward))
        {
            Debug.Log("DetectCover");
            playerStateManager.ChangeState(playerStateManager.idle);
            playerStateManager.idle = playerStateManager.idleInCover;
            playerStateManager.move = playerStateManager.moveInCover;
        }
        else
        {
            Debug.Log("Non DetectCover");
            playerStateManager.ChangeState(playerStateManager.idle);
            playerStateManager.idle = playerStateManager.normalIdle;
            playerStateManager.move = playerStateManager.normalMove;
        }
        Debug.Log("CurIdle =" + playerStateManager.idle);
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
        if (coverDetection.GetAimPos(curShoulderSide))
        {

        }
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
            //animator.SetLayerWeight(1,Mathf.Lerp(animator.GetLayerWeight\));
            NotifyObserver(this, PlayerAction.LowReady);
        }
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        NotifyObserver(this, PlayerAction.Reloading);
        base.Reloading(weapon, reloadType);
    }
    public override void TakeDamage(float Damage)
    {
        hpRegenarate.regenarate_countDown = 3;
        base.TakeDamage(Damage);
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
