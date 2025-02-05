using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandIdleStateNode : EnemyStateLeafNode
{
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    public float decelerate = 4;
    IMovementCompoent enemyMovement;
    public EnemyStandIdleStateNode(Enemy enemy) : base(enemy)
    {
        objectToward = new RotateObjectToward();
        agent = enemy.agent;
        enemyMovement = enemy.enemyMovement;
       

    }

    public EnemyStandIdleStateNode(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        objectToward = new RotateObjectToward();
        agent = enemy.agent;
        enemyMovement = enemy.enemyMovement;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Idle);
        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void FixedUpdate()
    {
        enemyMovement.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed,IMovementCompoent.MoveMode.MaintainMomentum);
        enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.moveRotateSpeed);

        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        base.Update();
    }
}
