using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCurve_and_Aim : EnemyActionLeafNode
{
    private CurvePath path;
    private float costDrainRate;
   
    public MoveCurve_and_Aim(EnemyCommandAPI enemyController, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
 
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        costDrainRate = UnityEngine.Random.Range(9, 15);
        path.ResetPath();
        path.GenaratePath(enemy.transform.position, enemy.targetKnewPos);
        base.Enter();
    }

    public override void Exit()
    {
        path.ResetPath();

        base.Exit();
    }

    public override void FixedUpdate()
    {
        
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
       return isReset.Invoke();
    }

    public override bool PreCondition()
    {
        return preCondidtion.Invoke();
    }

  
    
    public override void Update()
    {
        NavMeshAgent agent = enemy.agent;

        enemy.cost -= costDrainRate * Time.deltaTime;

        enemyController.AimDownSight();


        enemyController.RotateToPosition(enemy.targetKnewPos, 7);

        //if(path._markPoint.Count <= 0)
        //    return;

        //if(agent.hasPath == false)
        //    return;

        Vector3 dir = agent.steeringTarget - enemy.transform.position;
        enemyController.Move(dir, 1);

        //path.UpdateTargetPos(enemyBody.targetKnewPos,enemyBody.transform.position);
        //EnemyDebuger.curPos = agent.destination;
        base.Update();
    }
}
