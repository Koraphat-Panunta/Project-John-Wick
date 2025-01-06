using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeAimStateNode : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    NavMeshAgent agent;
    public EnemyStandTakeAimStateNode(Enemy enemy,ICoverUseable coverUseable) : base(enemy)
    {
        this.coverUseable = coverUseable;
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
            
        if(enemy.isAiming == false)
            return true;

        if(enemy.isPainTrigger)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if(enemy.isInCover
            &&enemy.isAiming)
            return true;

        return false;
    }

    public override void Update()
    {
        //switch (coverUseable.coverPoint) 
        //{
        //    case CoverPointTallSingleSide coverPointTallSingle: 
        //        {
        //            agent.Move(coverUseable.peekPos);
        //        }
        //        break;

        //    case CoverPointTallDoubleSide coverPointTallDouble: 
        //        {
        //            if(coverUseable.coverPoint.CheckingTargetInCoverView(coverUseable,enemy.targetLayer,coverPointTallDouble.peekPosL,out GameObject target))
        //            {
        //                coverPointTallDouble.TakeThisCover(coverUseable,coverPointTallDouble.peekPosL);
        //                agent.Move(coverUseable.peekPos);
        //            }
        //            else
        //            {
        //                coverPointTallDouble.TakeThisCover(coverUseable, coverPointTallDouble.peekPosR);
        //                agent.Move(coverUseable.peekPos);
        //            }
        //        }
        //        break;

        //    case CoverPointShort coverPointShort: 
        //        {
        //            agent.Move(coverUseable.peekPos);
        //        }
        //        break;
        //}

        NavMeshHit hit;
        float maxDistance = 1;
        Vector3 peekPos = coverUseable.peekPos;
        if (NavMesh.SamplePosition(peekPos, out hit, maxDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        agent.SetDestination(peekPos);
        enemy.weaponCommand.AimDownSight();
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._moveWithAgent);

        Vector3 moveDir = (agent.steeringTarget - enemy.transform.position).normalized * Time.deltaTime * 2;
        agent.Move(moveDir);

        enemy.enemyFiringPattern.Performing();
        new RotateObjectToward().RotateToward(enemy.lookRotation, enemy.gameObject, 6);
        //if (enemy.findingTargetComponent.FindTarget(out GameObject target) == false)
        //{
        //    enemy.cost += costRate * Time.deltaTime;
        //}
        base.Update();
    }
}
