using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeCoverStateNode : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    RotateObjectToward rotateObject;
    NavMeshAgent agent;
    public EnemyStandTakeCoverStateNode(Enemy enemy,ICoverUseable coverUseable) : base(enemy)
    {
        this.coverUseable = coverUseable;
        rotateObject = new RotateObjectToward();
        agent = enemy.agent;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
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

        if(enemy.isAiming)
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
        {
            enemy.curMoveVelocity_World = Vector3.MoveTowards(enemy.curMoveVelocity_World, moveDir * enemy._moveMaxSpeed, enemy._moveAccelerate * Time.deltaTime);
            agent.Move(enemy.curMoveVelocity_World * Time.deltaTime);
        }
        else
        {
            enemy.curMoveVelocity_World = Vector3.MoveTowards(enemy.curMoveVelocity_World, Vector3.zero, enemy._moveAccelerate*3 * Time.deltaTime);
            agent.Move(enemy.curMoveVelocity_World * Time.deltaTime);
        }
       
        if (coverUseable.coverPoint == null)
        {
            Debug.Log("this Null");
        }
        new RotateObjectToward().RotateToward(coverUseable.coverPoint.coverDir, enemy.gameObject, 6);

        base.Update();
    }
}
