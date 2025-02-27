using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SprintToCover : EnemyActionLeafNode
{
    private NavMeshAgent agent;
    public SprintToCover(EnemyCommandAPI enemyController) : base(enemyController)
    {
    }

    public SprintToCover(EnemyCommandAPI enemyController, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        agent = enemy.agent;
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {


        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
      
        enemyController.Sprint();
        enemyController.RotateToPosition(enemy.coverPos,7);

        enemyController.LowReady();
        if (Vector3.Distance(enemy.coverPos,
           new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z)) < 0.15f)
            enemyController.TakeCover();
        base.Update();
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
}
