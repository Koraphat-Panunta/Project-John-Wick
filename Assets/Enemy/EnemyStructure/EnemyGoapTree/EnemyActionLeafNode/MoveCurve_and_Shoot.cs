using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCurve_and_Shoot : EnemyActionLeafNode
{


    private EnemyPath path;
    private float costDrainRate;

    public MoveCurve_and_Shoot(EnemyControllerAPI enemyController) : base(enemyController)
    {
        path = new EnemyPath(enemy.agent);
    }
    public MoveCurve_and_Shoot(
        EnemyControllerAPI enemyController, 
        Func<bool> preCondition,
        Func<float> getCost,
        Func<bool> isReset) 
        : base(enemyController, preCondition,getCost, isReset)
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
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        NavMeshAgent agent = enemy.agent;

        enemy.cost -= costDrainRate * Time.deltaTime;

        enemyController.AimDownSight();

        Quaternion rotarion = new RotateObjectToward().RotateToward(enemy.targetKnewPos - enemy.transform.position,
            enemy.transform,
            7);
        enemyController.Rotate(rotarion);

        if (path._markPoint.Count <= 0)
            return;

        if (agent.hasPath == false)
            return;

        Vector3 dir = agent.steeringTarget - enemy.transform.position;
        enemyController.Move(dir, 1);


        EnemyDebuger.curPos = agent.destination;
      
        base.Update();
    }
}
