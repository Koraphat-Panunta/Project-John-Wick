using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemySprintStateNode : EnemyStateLeafNode
{
    Animator animator;
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    
    
    public EnemySprintStateNode(Enemy enemy) : base(enemy)
    {
        animator = enemy.animator;
        objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        
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
        enemy.curMoveVelocity_World = Vector3.MoveTowards(enemy.curMoveVelocity_World, enemy.transform.forward * enemy._sprintMaxSpeed, enemy._sprintAccelerate * Time.deltaTime);
        objectToward.RotateToward(enemy.lookRotation, enemy.gameObject, enemy._rotateSpeed);
        agent.Move(enemy.curMoveVelocity_World * Time.deltaTime);
        enemy.weaponCommand.LowReady();
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (enemy.isDead)
            return true;

        if(enemy._isPainTrigger)
            return true;

        if(enemy.isSprint == false)
            return true;

        return false;

    }

    public override bool PreCondition()
    {
        if(enemy.isSprint)
            return true;

        return false;
    }

    public override void Update()
    {

       
        
        base.Update();
    }
}
