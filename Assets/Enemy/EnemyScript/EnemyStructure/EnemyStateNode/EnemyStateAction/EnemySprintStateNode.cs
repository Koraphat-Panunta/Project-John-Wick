using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySprintStateNode : EnemyStateLeafNode
{
    Animator animator;
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    IMovementCompoent enemyMovement;
    
    public EnemySprintStateNode(Enemy enemy) : base(enemy)
    {
        animator = enemy.animator;
        objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        enemyMovement = enemy.enemyMovement;
        
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Sprint);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {

        enemyMovement.MoveToDirWorld(enemyMovement.forwardDir, enemy.sprintAccelerate, enemy.sprintMaxSpeed);
        enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.sprintRotateSpeed);

        enemy.weaponCommand.LowReady();
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (enemy.isDead)
            return true;

        if(enemy._isPainTrigger)
            return true;

        if(enemy.isSprintCommand == false)
            return true;

        return false;

    }

    public override bool PreCondition()
    {
        if(enemy.isSprintCommand)
            return true;

        return false;
    }

    public override void Update()
    {

       
        
        base.Update();
    }
}
