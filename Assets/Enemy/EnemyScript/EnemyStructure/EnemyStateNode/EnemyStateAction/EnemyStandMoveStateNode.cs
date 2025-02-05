using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandMoveStateNode : EnemyStateLeafNode
{
    
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    IMovementCompoent enemyMovement;
  
    public EnemyStandMoveStateNode(Enemy enemy) : base(enemy)
    {
        this.objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        this.enemyMovement = enemy.enemyMovement;
    }

    public EnemyStandMoveStateNode(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        this.objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        this.enemyMovement = enemy.enemyMovement;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Move);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        this.enemyMovement.MoveToDirWorld(enemy.moveInputVelocity_WorldCommand, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        this.enemyMovement.RotateToDirWorld(enemy.lookRotationCommand, enemy.moveRotateSpeed);

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
