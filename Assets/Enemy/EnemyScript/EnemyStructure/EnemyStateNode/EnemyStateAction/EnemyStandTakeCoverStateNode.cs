using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeCoverStateNode : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    RotateObjectToward rotateObject;
    NavMeshAgent agent;
    IMovementCompoent movementCompoent;
    public EnemyStandTakeCoverStateNode(Enemy enemy,ICoverUseable coverUseable) : base(enemy)
    {
        this.coverUseable = coverUseable;
        rotateObject = new RotateObjectToward();
        agent = enemy.agent;
        movementCompoent = enemy.enemyMovement;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TakeCover);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (enemy.isDead)
            return true;

        if (enemy.isInCover == false)
            return true;

        if(enemy.isAimingCommand)
            return true;

        if(enemy._isPainTrigger)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if(enemy.isInCover)
            return true;

        return false;
    }

    public override void Update()
    {
        
        Vector3 CoverPos = coverUseable.coverPos;

      
        enemy.weaponCommand.LowReady();

        Vector3 moveDir = (CoverPos - enemy.transform.position).normalized;

        if (Vector3.Distance(enemy.transform.position, CoverPos) > 0.15f)
            movementCompoent.MoveToDirWorld(moveDir, enemy.moveAccelerate, enemy.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        else
            movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakAccelerate, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
              

        movementCompoent.RotateToDirWorld(coverUseable.coverPoint.coverDir, 6);

        base.Update();
    }
}
