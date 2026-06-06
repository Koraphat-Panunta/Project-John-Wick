
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTestingSystemCommandDecision : EnemyDecision
{
    private TaskingExecuteQueue _queue = new TaskingExecuteQueue();

    private ITaskingExecute dodge;
    private ITaskingExecute crouch;

    private ITaskingExecute moveToPos1;
    private ITaskingExecute openDoor;
    private ITaskingExecute moveToPos2;
    private ITaskingExecute moveToPos3;
    private ITaskingExecute moveToPos4;

    private ITaskingExecute sprintToPos1;
    private ITaskingExecute sprintToPos2;
    private ITaskingExecute sprintToPos3;
    private ITaskingExecute sprintToPos4;
    private ITaskingExecute sprintToPos5;
    private ITaskingExecute sprintToPos6;

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
    [SerializeField] private Transform moveTransPos3;
    [SerializeField] private Transform moveTransPos4;

    [SerializeField] private Transform coverPos1;

    [SerializeField] private Transform sprintTransPos1;
    [SerializeField] private Transform sprintTransPos2;
    [SerializeField] private Transform sprintTransPos3;
    [SerializeField] private Transform sprintTransPos4;
    [SerializeField] private Transform sprintTransPos5;
    [SerializeField] private Transform sprintTransPos6;

    [SerializeField] private RangeWeapon pickedUpPrimaryWeapon;
    [SerializeField] private float freezTimer = 3;
    [SerializeField] private RangeWeapon pickedUpSecondaryWeapon;
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

        dodge = new TaskingExecute(() => enemyCommand.Dodge(enemy.transform.forward)
        , () => enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyDodgeStateNodeLeaf>());

        crouch = new TaskingExecute(() => enemyCommand.Crouch(),
            () => enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>() || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>());

        moveToPos1 = new EnemyMoveToPos(enemy.transform, this.moveTransPos1.position, true, enemyCommand);
        this.openDoor = new TaskingExecute(() => this.enemyCommand.OpenDoor(), () => true);
        moveToPos2 = new EnemyMoveToPos(enemy.transform, this.moveTransPos2.position, true, enemyCommand);
        moveToPos3 = new EnemyMoveToPos(enemy.transform, this.moveTransPos3.position, true, enemyCommand);
        moveToPos4 = new EnemyMoveToPos(enemy.transform, this.moveTransPos4.position, true, enemyCommand);

        sprintToPos1 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.sprintTransPos1.position,1,1));
        sprintToPos2 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.sprintTransPos2.position, 1,1));
        sprintToPos3 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.sprintTransPos3.position, 1, 1));
        sprintToPos4 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.sprintTransPos4.position, 1, 1));
        sprintToPos5 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.sprintTransPos5.position, 1, 1));
        sprintToPos6 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.sprintTransPos6.position, 1, 1));

        freez_3s = new TaskingExecute(
            () =>
            {
                this.freezTimer -= Time.deltaTime;
                enemyCommand.FreezPosition();
            },
            () => this.freezTimer <= 0);

        moveToWeaponPickedUpPrimary = new TaskingExecute(() => { },
            ()=>
            {
                if (enemyCommand.MoveToPositionRotateToward(this.pickedUpPrimaryWeapon.transform.position, 1, 1))
                {
                    enemyCommand.FreezPosition();
                    return true;
                }
                return false;
            });

        pickUpWeaponPrimary = new TaskingExecute(() => enemyCommand.PickUpWeapon(), () => enemy._currentWeapon ? true : false);
        holsterWeaponPrimary = new TaskingExecute(() => enemyCommand.HolsterWeapon(), () => enemy._currentWeapon == null);
        drawWeaponPrimary = new TaskingExecute(() => enemyCommand.DrawWeaponPrimary(), () => enemy._currentWeapon == enemy._weaponBelt.myPrimaryWeapon as RangeWeapon);
        dropWeaponPrimary = new TaskingExecute(() => enemyCommand.DropWeapon(), () => enemy._currentWeapon == null);

        pickUpWeaponPrimary2 = new TaskingExecute(() => enemyCommand.PickUpWeapon(),
            () =>
            {
                if (enemy._currentWeapon != null)
                {
                    debugLog += enemy._currentWeapon;
                    return true;
                }
                return false;
            });

        moveToWeaponPickedUpSecondary = new TaskingExecute(() => { },
            () =>
            {
                if (enemyCommand.MoveToPositionRotateToward(pickedUpSecondaryWeapon.transform.position, 1, 1))
                {
                    enemyCommand.FreezPosition();
                    return true;
                }
                return false;
            });

        pickUpWeaponSecondary = new TaskingExecute(() => enemyCommand.PickUpWeapon(), () => enemy._currentWeapon is SecondaryWeapon);
        switchWeaponSecondaryToPrimary = new TaskingExecute(() => enemyCommand.DrawWeaponPrimary(), () => enemy._currentWeapon is PrimaryWeapon);
        switchWeaponPrimaryToSecondary = new TaskingExecute(() => enemyCommand.DrawWeaponSecondary(), () => enemy._currentWeapon is SecondaryWeapon);

        ADS_PullTrigger = new TaskingExecute(
            () =>
            {
                enemyCommand.AimDownSight(enemy.targetKnowPos);
                if (enemy._currentWeapon.triggerState == TriggerState.Up)
                    enemyCommand.PullTrigger();
            },
            () => enemy._currentWeapon.curBulletCapacity <= (int)(enemy._currentWeapon.maxAmmoCapacity * 0.7f));

        reload = new TaskingExecute(() => enemyCommand.Reload(), () => enemy._currentWeapon.curBulletCapacity == enemy._currentWeapon.maxAmmoCapacity);

        moveToTakeCover1 = new TaskingExecute(() => { }, () => enemyCommand.SprintToPosition(this.coverPos1.position, 1, 0.5f));
        softcoverManuver = new TaskingExecute(
            () =>
            {
                enemyCommand.AutoDetectSoftCover();
                timerCoverManuver -= Time.deltaTime;
                if (timerCoverManuver <= 0)
                    timerCoverManuver = 3;

                if (timerCoverManuver > 1f)
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

        sprintToSpinKick = new TaskingExecute(() => { },
            () => enemyCommand.SprintToPosition(enemy.targetKnowPos, enemy.sprintRotateSpeed, 2f));
        spinKick = new TaskingExecute(() => enemyCommand.HeavyAttack(), () => enemy.stateManagerNode.TryGetCurNodeLeaf<Enemy_OCM_Hit_NodeLeaf>());

        _queue.Enqueue(freez_3s);//24
        //_queue.Enqueue(dodge);//23
        //_queue.Enqueue(crouch);//22

        _queue.Enqueue(moveToPos1);//21
        _queue.Enqueue(this.openDoor);
        _queue.Enqueue(moveToPos2);//21
        _queue.Enqueue(this.crouch);
        _queue.Enqueue(moveToPos3);//21
        _queue.Enqueue(moveToPos4);//21

        //_queue.Enqueue(rotateToSprintPos1);

        _queue.Enqueue(sprintToPos1);//19
        _queue.Enqueue(sprintToPos2);//19
        _queue.Enqueue(sprintToPos3);//19
        _queue.Enqueue(sprintToPos4);//19
        _queue.Enqueue(sprintToPos5);//19
        _queue.Enqueue(sprintToPos6);

        _queue.Enqueue(freez_3s);//18
        _queue.Enqueue(moveToWeaponPickedUpPrimary);//17
        _queue.Enqueue(pickUpWeaponPrimary);//16
        _queue.Enqueue(holsterWeaponPrimary);//15
        _queue.Enqueue(drawWeaponPrimary);//14
        _queue.Enqueue(dropWeaponPrimary);//13
        _queue.Enqueue(pickUpWeaponPrimary2);//12
        _queue.Enqueue(moveToWeaponPickedUpSecondary);//11
        _queue.Enqueue(pickUpWeaponSecondary);//10
        _queue.Enqueue(switchWeaponSecondaryToPrimary);//9
        _queue.Enqueue(switchWeaponPrimaryToSecondary);//8
        _queue.Enqueue(ADS_PullTrigger);//7
        _queue.Enqueue(reload);//6
        _queue.Enqueue(moveToTakeCover1);//4
        _queue.Enqueue(softcoverManuver);//3
        _queue.Enqueue(sprintToSpinKick);//2
        _queue.Enqueue(spinKick);//1
    }

    protected override void Update()
    {
        queueCount = _queue.Count;
        _queue.Update();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        _queue.FixedUpdate();
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {

    }

    private void OnDrawGizmos()
    {
        try
        {
            DrawCircle(enemy.transform.position, raduisFindCover);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.enemy.targetKnowPos, 0.25f);
        }
        catch { }
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
