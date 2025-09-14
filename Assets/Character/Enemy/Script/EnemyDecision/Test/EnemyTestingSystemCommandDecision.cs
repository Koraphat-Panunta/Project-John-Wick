
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestingSystemCommandDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    private Queue<ITaskingExecute> enemyTestingCommands = new Queue<ITaskingExecute>();

    private ITaskingExecute dodge;
    private ITaskingExecute crouch;
    private ITaskingExecute moveToPos1;
    private ITaskingExecute rotateToPos2;
    private ITaskingExecute sprintToPos3;
    private ITaskingExecute freez_3s;
    private ITaskingExecute moveToWeaponPickedUpPrimary;
    private ITaskingExecute pickUpWeaponPrimary;
    private ITaskingExecute holsterWeaponPrimary;
    private ITaskingExecute drawWeaponPrimary;
    private ITaskingExecute dropWeaponPrimary;
    private ITaskingExecute pickUpWeaponPrimary2;
    private ITaskingExecute moveToWeaponPickedUpSecondary;
    private ITaskingExecute pickUpWeaponSecondary;
    private ITaskingExecute switchWeaponSecondaryToPrimary;
    private ITaskingExecute switchWeaponPrimaryToSecondary;
    private ITaskingExecute ADS_PullTrigger;
    private ITaskingExecute tacticalReload;
    private ITaskingExecute ADS_PillTriggerAllOutMag;
    private ITaskingExecute reload;
    private ITaskingExecute moveToTakeCover1;
    private ITaskingExecute softcoverManuver;
    private ITaskingExecute sprintToSpinKick;
    private ITaskingExecute spinKick;

    [SerializeField] private Transform moveTransPos1;
    [SerializeField] private Transform moveTransPos2;
    [SerializeField] private Transform rotateTransPos2;
    [SerializeField] private Transform sprintTransPos3;
    [SerializeField] private Weapon pickedUpPrimaryWeapon;
    [SerializeField] private float freezTimer = 3;
    [SerializeField] private Weapon pickedUpSecondaryWeapon;
    [SerializeField] private CoverPoint coverPoint;
    [SerializeField] private float timerCoverManuver ;
    [SerializeField] private Transform targetPos;

    [SerializeField,TextArea(10,10)] private string debugLog;

    [SerializeField] private int queueCount;

    [Range(0, 20)]
    [SerializeField] private float raduisFindCover;

    public override void Initialized()
    {
        InitializedCommand();
        base.Initialized();
    }
  

    private void InitializedCommand()
    {
        if (enemyCommand == null)
            enemyCommand = GetComponent<EnemyCommandAPI>();
        dodge = new EnemyTestingCommand(() => enemyCommand.Dodge(enemy.transform.forward)
        , () => enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>());
        crouch = new EnemyTestingCommand(() => enemyCommand.Crouch(),
            () => enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>() || enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>());
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
        holsterWeaponPrimary = new EnemyTestingCommand(()=> enemyCommand.HolsterWeapon(),()=> enemy._currentWeapon == null);
        drawWeaponPrimary = new EnemyTestingCommand(() => enemyCommand.DrawWeaponPrimary(), () => enemy._currentWeapon == enemy._weaponBelt.myPrimaryWeapon as Weapon);
        dropWeaponPrimary = new EnemyTestingCommand(() => enemyCommand.DropWeapon(), () => enemy._currentWeapon == null);
        pickUpWeaponPrimary2 = new EnemyTestingCommand(() => enemyCommand.PickUpWeapon(), 
            () => 
            { if(enemy._currentWeapon != null)
                {
                    debugLog += enemy._currentWeapon;
                    return true;
                }
            return false;
                    });
        moveToWeaponPickedUpSecondary = new EnemyTestingCommand(() => { },
            () =>
            {
                if (enemyCommand.MoveToPositionRotateToward(pickedUpSecondaryWeapon.transform.position, enemy.moveMaxSpeed, enemy.moveRotateSpeed))
                {
                    enemyCommand.FreezPosition();
                    return true;
                }
                return false;
            });
        pickUpWeaponSecondary = new EnemyTestingCommand(() => enemyCommand.PickUpWeapon(), () => enemy._currentWeapon is SecondaryWeapon);
        switchWeaponSecondaryToPrimary = new EnemyTestingCommand(() => enemyCommand.DrawWeaponPrimary(),()=> enemy._currentWeapon is PrimaryWeapon);
        switchWeaponPrimaryToSecondary = new EnemyTestingCommand(() => enemyCommand.DrawWeaponSecondary(), () => enemy._currentWeapon is SecondaryWeapon);
        ADS_PullTrigger = new EnemyTestingCommand(
            () =>
            {
                enemyCommand.AimDownSight(enemy.targetKnewPos);
                if(enemy._currentWeapon.triggerState == TriggerState.Up)
                    enemyCommand.PullTrigger();
            }, () => enemy._currentWeapon.bulletStore[BulletStackType.Magazine] <= (int)(enemy._currentWeapon.bulletCapacity * 0.7f));
        reload = new EnemyTestingCommand(() => enemyCommand.Reload(), () => enemy._currentWeapon.bulletStore[BulletStackType.Magazine] == enemy._currentWeapon.bulletCapacity);
       
        moveToTakeCover1 = new EnemyTestingCommand(() => { }, () => enemyCommand.SprintToPosition(coverPoint.coverPos.position,1,0.5f));
        softcoverManuver = new EnemyTestingCommand(
            () => 
            {
                enemyCommand.AutoDetectSoftCover();

                timerCoverManuver -= Time.deltaTime;

                if (timerCoverManuver <= 0)
                    timerCoverManuver = 3;

                if(timerCoverManuver > 1f)
                {
                    enemyCommand.AimDownSight(targetPos.position);
                    enemyCommand.NormalFiringPattern.Performing();
                }
                else
                {
                    enemyCommand.LowReady();
                }
               
            },
            () => 
            {
                if (enemyCommand.MoveToPosition(this.moveTransPos2.position, 1))
                    return true;
                return false;
                    });
        sprintToSpinKick = new EnemyTestingCommand(() => { },
            ()=> enemyCommand.SprintToPosition(enemy.targetKnewPos,enemy.sprintRotateSpeed,2f));
        spinKick = new EnemyTestingCommand(() => enemyCommand.SpinKick(), () => enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>());

        enemyTestingCommands.Enqueue(dodge);//23
        enemyTestingCommands.Enqueue(crouch);//22
        enemyTestingCommands.Enqueue(moveToPos1);//21
        enemyTestingCommands.Enqueue(rotateToPos2);//20
        enemyTestingCommands.Enqueue(sprintToPos3);//19
        enemyTestingCommands.Enqueue(freez_3s);//18
        enemyTestingCommands.Enqueue(moveToWeaponPickedUpPrimary);//17
        enemyTestingCommands.Enqueue(pickUpWeaponPrimary);//16
        enemyTestingCommands.Enqueue(holsterWeaponPrimary);//15
        enemyTestingCommands.Enqueue(drawWeaponPrimary);//14
        enemyTestingCommands.Enqueue(dropWeaponPrimary);//13
        enemyTestingCommands.Enqueue(pickUpWeaponPrimary2);//12
        enemyTestingCommands.Enqueue(moveToWeaponPickedUpSecondary);//11
        enemyTestingCommands.Enqueue(pickUpWeaponSecondary);//10
        enemyTestingCommands.Enqueue(switchWeaponSecondaryToPrimary);//9
        enemyTestingCommands.Enqueue(switchWeaponPrimaryToSecondary);//8
        enemyTestingCommands.Enqueue(ADS_PullTrigger);//7
        enemyTestingCommands.Enqueue(reload);//6
        enemyTestingCommands.Enqueue(moveToTakeCover1);//4
        enemyTestingCommands.Enqueue(softcoverManuver);//3
        enemyTestingCommands.Enqueue(sprintToSpinKick);//2
        enemyTestingCommands.Enqueue(spinKick);//1

    }

    protected override void Update()
    {
        queueCount = enemyTestingCommands.Count;

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
    private class EnemyMoveToPos : ITaskingExecute
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
    private class EnemyRotateToPos:ITaskingExecute
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
    private class EnemyTestingCommand:ITaskingExecute
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
    private void OnDrawGizmos()
    {
        DrawCircle(enemy.transform.position, raduisFindCover);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(enemy.targetKnewPos, 0.25f);
    }
    private void DrawCircle(Vector3 center, float radius)
    {
        int segments = 32;
        Gizmos.color = Color.yellow;
        float angle = 0f;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            angle = i * Mathf.PI * 2f / segments;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

}

