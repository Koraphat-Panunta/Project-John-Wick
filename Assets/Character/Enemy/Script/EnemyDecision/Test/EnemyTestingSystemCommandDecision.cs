
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
    private IEnemyTestingCommand ADS_PullTrigger_Reload_PullTriggerAllOut_Reload;
    private IEnemyTestingCommand findAndBookCover1;
    private IEnemyTestingCommand moveToTakeCover1;
    private IEnemyTestingCommand coverManuver1;
    private IEnemyTestingCommand findAndBookCover2;
    private IEnemyTestingCommand sprintToTakeCover;
    private IEnemyTestingCommand coverManuver2;
    private IEnemyTestingCommand approuchingSpinKick;

    [SerializeField] private Transform moveTransPos1;
    [SerializeField] private Transform rotateTransPos2;
    [SerializeField] private Transform sprintTransPos3;
    [SerializeField] private float freezTimer = 3;

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
        Debug.Log(moveTransPos1.position);
        rotateToPos2 = new EnemyRotateToPos(enemy.transform, rotateTransPos2.position, enemy.aimingRotateSpeed, enemyCommand);
        freez_3s = new EnemyTestingCommand(
            () =>
            {
                this.freezTimer -= Time.deltaTime;
                enemyCommand.FreezPosition();
            },
            () => this.freezTimer <= 0);
        sprintToPos3 = new EnemyTestingCommand(() => { }, ()=>enemyCommand.SprintToPosition(this.sprintTransPos3.position,enemy.sprintRotateSpeed));

        enemyTestingCommands.Enqueue(moveToPos1);
        enemyTestingCommands.Enqueue(rotateToPos2);
        enemyTestingCommands.Enqueue(sprintToPos3);
        enemyTestingCommands.Enqueue(freez_3s);

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

