using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyTestingSystemCommandDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    private Queue<IEnemyTestingCommand> enemyTestingCommands = new Queue<IEnemyTestingCommand>();

    private IEnemyTestingCommand moveToPos1;
    private IEnemyTestingCommand rotateToPos2;
    private IEnemyTestingCommand sprintToPos3;
    private IEnemyTestingCommand freez_3s;
    private IEnemyTestingCommand moveToWeaponPickedUpPromary;
    private IEnemyTestingCommand pickUpWeapon;
    private IEnemyTestingCommand holsterWeaponPrimary;
    private IEnemyTestingCommand drawWeapon;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        if(enemyTestingCommands == null || enemyTestingCommands.Count <= 0)
            return;

        if (enemyTestingCommands.Peek().IsComplete())
            enemyTestingCommands.Dequeue();

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
    private abstract class EnemyTestingCommand:IEnemyTestingCommand
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

