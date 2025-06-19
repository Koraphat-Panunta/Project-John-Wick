
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyTestingSystemCommandDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    private Queue<IEnemyTestingCommand> enemyTestingCommands = new Queue<IEnemyTestingCommand>();

    private IEnemyTestingCommand moveToPos1;
    private IEnemyTestingCommand rotateToPos2;
    private IEnemyTestingCommand sprintToPos3;
    private IEnemyTestingCommand freez_3s;
    private IEnemyTestingCommand moveToWeaponPickedUpPrimary;
    private IEnemyTestingCommand pickUpWeaponPrimary;
    private IEnemyTestingCommand holsterWeaponPrimary;
    private IEnemyTestingCommand drawWeaponPrimary;
    private IEnemyTestingCommand dropWeaponPrimary;
    private IEnemyTestingCommand pickUpWeaponPrimary2;
    private IEnemyTestingCommand moveToWeaponPickedUpSecondary;
    private IEnemyTestingCommand pickUpWeaponSecondary;
    private IEnemyTestingCommand switchWeaponSecondaryToPrimary;
    private IEnemyTestingCommand switchWeaponPrimaryToSecondary;
    private IEnemyTestingCommand ADS_PullTrigger;
    private IEnemyTestingCommand tacticalReload;
    private IEnemyTestingCommand ADS_PillTriggerAllOutMag;
    private IEnemyTestingCommand reload;
    private IEnemyTestingCommand findAndBookCover1;
    private IEnemyTestingCommand moveToTakeCover1;
    private IEnemyTestingCommand coverManuver1;
    private IEnemyTestingCommand moveToSpinKick;
    private IEnemyTestingCommand spinKick;

    [SerializeField] private Transform moveTransPos1;
    [SerializeField] private Transform rotateTransPos2;
    [SerializeField] private Transform sprintTransPos3;
    [SerializeField] private Weapon pickedUpPrimaryWeapon;
    [SerializeField] private float freezTimer = 3;
    [SerializeField] private Weapon pickedUpSecondaryWeapon;
    [SerializeField] private CoverPoint coverPoint;
    [SerializeField] private float timerCoverManuver = 9;
    [SerializeField] private Transform targetPos;

    [SerializeField,TextArea(10,10)] private string debugLog;
    protected override void Awake()
    {
        InitializedCommand();
        base.Awake();
    }

    private void InitializedCommand()
    {
        if (enemyCommand == null)
            enemyCommand = GetComponent<EnemyCommandAPI>();

        moveToPos1 = new EnemyMoveToPos(enemy.transform, this.moveTransPos1.position, true, enemyCommand);
        rotateToPos2 = new EnemyRotateToPos(enemy.transform, rotateTransPos2.position, enemy.aimingRotateSpeed, enemyCommand);
        sprintToPos3 = new EnemyTestingCommand(() => { }, ()=>enemyCommand.SprintToPosition(this.sprintTransPos3.position,enemy.sprintRotateSpeed));
        freez_3s = new EnemyTestingCommand(
    () =>
    {
        this.freezTimer -= Time.deltaTime;
        enemyCommand.FreezPosition();
    },
    () => this.freezTimer <= 0);
        moveToWeaponPickedUpPrimary = new EnemyTestingCommand(() => { },
            ()=> 
            { if (enemyCommand.MoveToPositionRotateToward(pickedUpPrimaryWeapon.transform.position, enemy.moveMaxSpeed, enemy.moveRotateSpeed))
                {
                    enemyCommand.FreezPosition();
                    return true;
                }
            return false;
            });
        pickUpWeaponPrimary = new EnemyTestingCommand(() => enemyCommand.PickUpWeapon(),()=> { return enemy._currentWeapon ? true : false; });
        holsterWeaponPrimary = new EnemyTestingCommand(()=>enemyCommand.HolsterWeapon(),()=> enemy._currentWeapon == null);
        drawWeaponPrimary = new EnemyTestingCommand(() => enemyCommand.DrawWeaponPrimary(), () => enemy._currentWeapon == enemy._weaponBelt.myPrimaryWeapon as Weapon);
        dropWeaponPrimary = new EnemyTestingCommand(() => enemyCommand.DropWeapon(), () => enemy._currentWeapon == null);
        pickUpWeaponPrimary2 = new EnemyTestingCommand(() => enemyCommand.PickUpWeapon(),()=> enemy._currentWeapon != null);
        moveToWeaponPickedUpSecondary = new EnemyMoveToPos(enemy.transform, pickedUpSecondaryWeapon.transform.position, true, enemyCommand);
        pickUpWeaponSecondary = new EnemyTestingCommand(() => enemyCommand.PickUpWeapon(), () => enemy._currentWeapon is SecondaryWeapon);
        switchWeaponSecondaryToPrimary = new EnemyTestingCommand(() => enemyCommand.SwitchWeapon(),()=> enemy._currentWeapon is PrimaryWeapon);
        switchWeaponPrimaryToSecondary = new EnemyTestingCommand(() => enemyCommand.SwitchWeapon(), () => enemy._currentWeapon is SecondaryWeapon);
        ADS_PullTrigger = new EnemyTestingCommand(
            () =>
            {
                enemyCommand.AimDownSight();
                enemyCommand.PullTrigger();
            }, () => enemy._currentWeapon.bulletStore[BulletStackType.Magazine] <= enemy._currentWeapon.bulletCapacity * 0.7f);
        reload = new EnemyTestingCommand(() => enemyCommand.Reload(), () => enemy._currentWeapon.bulletStore[BulletStackType.Magazine] == enemy._currentWeapon.bulletCapacity);
        findAndBookCover1 = new EnemyTestingCommand(() => 
        {
            enemyCommand.FindCoverAndBook(10, out CoverPoint coverPoint);
            
        }, () => enemy.coverPoint != null);
        moveToTakeCover1 = new EnemyTestingCommand(() => { }, () => enemyCommand.SprintToCover(coverPoint));
        coverManuver1 = new EnemyTestingCommand(
            () => 
            {
                if(timerCoverManuver > 7)
                {
                    enemyCommand.AimDownSight();
                }
                else if(timerCoverManuver > 4)
                {
                    enemyCommand.AimDownSight();
                    enemyCommand.PullTrigger();
                }
                else if(timerCoverManuver > 2)
                {
                    enemyCommand.AimDownSight();
                }
                else
                {

                }
            },
            () => timerCoverManuver <= 0);
        moveToSpinKick = new EnemyMoveToPos(enemy.transform, targetPos.position, true, enemyCommand);
        spinKick = new EnemyTestingCommand(() => enemyCommand.SpinKick(), () => enemy.curNodeLeaf is EnemySpinKickGunFuNodeLeaf);

        enemyTestingCommands.Enqueue(moveToPos1);
        enemyTestingCommands.Enqueue(rotateToPos2);
        enemyTestingCommands.Enqueue(sprintToPos3);
        enemyTestingCommands.Enqueue(freez_3s);
        enemyTestingCommands.Enqueue(moveToWeaponPickedUpPrimary);
        enemyTestingCommands.Enqueue(pickUpWeaponPrimary);
        enemyTestingCommands.Enqueue(holsterWeaponPrimary);
        enemyTestingCommands.Enqueue(drawWeaponPrimary);
        enemyTestingCommands.Enqueue(dropWeaponPrimary);
        enemyTestingCommands.Enqueue(pickUpWeaponPrimary2);
        enemyTestingCommands.Enqueue(moveToWeaponPickedUpSecondary);
        enemyTestingCommands.Enqueue(pickUpWeaponSecondary);
        enemyTestingCommands.Enqueue(switchWeaponSecondaryToPrimary);
        enemyTestingCommands.Enqueue(switchWeaponPrimaryToSecondary);
        enemyTestingCommands.Enqueue(ADS_PullTrigger);
        enemyTestingCommands.Enqueue(reload);
        enemyTestingCommands.Enqueue(findAndBookCover1);
        enemyTestingCommands.Enqueue(moveToTakeCover1);
        enemyTestingCommands.Enqueue(coverManuver1);
        enemyTestingCommands.Enqueue(moveToSpinKick);
        enemyTestingCommands.Enqueue(spinKick);

    }

    protected override void Update()
    {
        if(enemyTestingCommands == null || enemyTestingCommands.Count <= 0)
            return;

        if (enemyTestingCommands.Peek().IsComplete())
        {
            enemyTestingCommands.Dequeue();
            debugLog += enemyTestingCommands + " been complete \n";
            if (enemyTestingCommands.Count <= 0)
                debugLog += "complete all command test \n";
        }

        try
        {
            enemyTestingCommands.Peek().Update();
        }
        catch (Exception e) 
        {
            
        }
        base.Update();
    }
    protected override void FixedUpdate()
    {

        try
        {
            enemyTestingCommands.Peek().FixedUpdate();
        }
        catch (Exception e)
        {

        }
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        
    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
         
    }
    private class EnemyMoveToPos : IEnemyTestingCommand
    {
        private Vector3 pos;
        private bool isRotateTowardDes;
        private Transform myTrans;
        private EnemyCommandAPI enemyCommandAPI;
        private float reachDes = 0.5f;
        public EnemyMoveToPos(Transform myTrans,Vector3 pos, bool isRotateTowardDes,EnemyCommandAPI enemyCommandAPI)
        {
            this.pos = pos;
            this.isRotateTowardDes = isRotateTowardDes;
            this.myTrans = myTrans;
            this.enemyCommandAPI = enemyCommandAPI;
        }

        public void FixedUpdate()
        {
            
        }

        public bool IsComplete()
        {
            if(Vector3.Distance(myTrans.position,this.pos) <= this.reachDes)
                return true;
            return false;
        }

        public void Update()
        {
            if (isRotateTowardDes)
                enemyCommandAPI.MoveToPositionRotateToward(this.pos, enemyCommandAPI._enemy.moveMaxSpeed, enemyCommandAPI._enemy.moveRotateSpeed, reachDes);
            else
                enemyCommandAPI.MoveToPosition(this.pos, enemyCommandAPI._enemy.moveMaxSpeed, reachDes);
        }
    }
    private class EnemyRotateToPos:IEnemyTestingCommand
    {
        private float rotateSpeed;
        private Vector3 towardedPos;
        private Transform myTrans;
        private EnemyCommandAPI enemyCommandAPI;
        public EnemyRotateToPos(Transform myTrans,Vector3 towardedPos,float rotateSpeed,EnemyCommandAPI enemyCommandAPI) 
        {
            this.myTrans = myTrans;
            this.towardedPos = towardedPos;
            this.rotateSpeed = rotateSpeed;
            this.enemyCommandAPI = enemyCommandAPI;
        }

        public void FixedUpdate()
        {
            
        }

        public bool IsComplete()
        {
            Vector3 dir = this.towardedPos - myTrans.position;
            dir.Normalize();

            if (Vector3.Dot(myTrans.forward, dir) > 0.95f)
                return true;
            return false;
        }

        public void Update()
        {
            enemyCommandAPI.RotateToPosition(this.towardedPos,this.rotateSpeed);
        }
    }
    private class EnemyTestingCommand:IEnemyTestingCommand
    {
        private Action update;
        private Action fixUpdate;
        private Func<bool> isComplete;
        public EnemyTestingCommand(Action update,Action fixUpdate, Func<bool> isComplete) 
        {
            this.update = update;
            this.fixUpdate = fixUpdate;
            this.isComplete = isComplete;
        }
        public EnemyTestingCommand(Action update,Func<bool> isComplete) : this(update,null,isComplete)
        {

        }
        public void FixedUpdate()
        {
           if(this.fixUpdate != null)
                this.fixUpdate.Invoke();
        }

        public bool IsComplete()
        {
            return isComplete.Invoke();
        }

        public void Update()
        {
            if(this.update != null)
                this.update.Invoke();
        }
    }
    private interface IEnemyTestingCommand
    {
        public void Update();
        public void FixedUpdate();
        public bool IsComplete();
        

        
    }

}

