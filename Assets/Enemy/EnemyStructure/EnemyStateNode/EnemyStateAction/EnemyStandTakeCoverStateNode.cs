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
        if(enemy.isInCover == false)
            return true;

        if(enemy.isAiming)
            return true;

        if(enemy.isPainTrigger)
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
        //coverUseable.userCover.transform.rotation = rotateObject.RotateToward(
        //    coverUseable.coverPos - coverUseable.userCover.transform.position, coverUseable.userCover.transform, 6);

        //agent.Move(coverUseable.coverPos);

        NavMeshHit hit;
        float maxDistance = 1;
        Vector3 CoverPos = coverUseable.coverPos;

        if (NavMesh.SamplePosition(CoverPos, out hit, maxDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        agent.SetDestination(CoverPos);
        enemy.weaponCommand.LowReady();

        Vector3 moveDir = (agent.steeringTarget - enemy.transform.position).normalized * Time.deltaTime * 2;
        agent.Move(moveDir);

        enemy.enemyFiringPattern.Performing();
        new RotateObjectToward().RotateToward(enemy.lookRotation, enemy.gameObject, 6);

        base.Update();
    }
}
