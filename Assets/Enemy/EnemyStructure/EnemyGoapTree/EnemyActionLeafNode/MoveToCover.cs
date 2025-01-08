using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToCover : EnemyActionLeafNode
{
    private ICoverUseable coverUseable;
    private NavMeshAgent agent;
    private NormalFiringPattern firingPattern;
    public MoveToCover(EnemyCommandAPI enemyController) : base(enemyController)
    {
    }

    public MoveToCover(EnemyCommandAPI enemyController,ICoverUseable coverUseable, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        this.coverUseable = coverUseable;
        this.agent = enemy.agent;
        firingPattern = new NormalFiringPattern(enemyController);
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        agent.SetDestination(enemy.coverPos);
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
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        Vector3 dir = agent.steeringTarget - enemy.transform.position;
        enemyController.Move(dir, 1);

        switch (enemy.combatOffensiveInstinct.myCombatPhase) 
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert: 
                {
                    enemyController.AimDownSight();
                    enemyController.RotateToPosition(enemy.targetKnewPos, 7);
                    firingPattern.Performing();
                };
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert: 
                {
                    enemyController.AimDownSight();
                    enemyController.RotateToPosition(enemy.targetKnewPos, 7);
                    firingPattern.Performing();
                };
                break;
            case CombatOffensiveInstinct.CombatPhase.SemiAlert: 
                {
                    enemyController.AimDownSight();
                    enemyController.RotateToPosition(enemy.targetKnewPos, 7);
                };
                break;
        }

        if(Vector3.Distance(coverUseable.coverPos,
            new Vector3(enemy.transform.position.x,0,enemy.transform.position.z))<0.15f)
            enemyController.TakeCover();

        base.Update();
    }
}
