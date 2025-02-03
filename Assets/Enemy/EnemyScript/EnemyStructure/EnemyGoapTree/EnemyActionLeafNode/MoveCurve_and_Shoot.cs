using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCurve_and_Shoot : EnemyActionLeafNode
{


    //private CurvePath path;
    private float costDrainRate;
    private NormalFiringPattern firingPattern;
    public MoveCurve_and_Shoot(EnemyCommandAPI enemyController) : base(enemyController)
    {
        //path = new CurvePath();
        firingPattern = new NormalFiringPattern(enemyController);
    }
    public MoveCurve_and_Shoot(
        EnemyCommandAPI enemyController, 
        Func<bool> preCondition,
        Func<bool> isReset) 
        : base(enemyController, preCondition, isReset)
    {
    }
    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        costDrainRate = UnityEngine.Random.Range(9, 15);
        //path.ResetPath();
        //path.GenaratePath(enemy.transform.position, enemy.targetKnewPos);
     
        base.Enter();
    }

    public override void Exit()
    {
        //path.ResetPath();

        base.Exit();
    }

    public override void FixedUpdate()
    {
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
        firingPattern.Performing();

        NavMeshAgent agent = enemy.agent;

        enemy.cost -= costDrainRate * Time.deltaTime;

        enemyController.AimDownSight();

      
        enemyController.Rotate(enemy.targetKnewPos,7);

        //if (path._markPoint.Count <= 0)
        //    return;

        if (agent.hasPath == false)
            return;

        Vector3 dir = agent.steeringTarget - enemy.transform.position;
        enemyController.Move(dir, 1);

        //path.UpdateTargetPos(enemy.targetKnewPos, enemy.transform.position);
      
        base.Update();
    }
}
