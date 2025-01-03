using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSuspectPos : EnemyActionLeafNode
{
    IFindingTarget findingTarget;
    NavMeshAgent agent;
    public MoveToSuspectPos(EnemyControllerAPI enemyController) : base(enemyController)
    {
    }

    public MoveToSuspectPos(EnemyControllerAPI enemyController,IFindingTarget findingTarget, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        this.findingTarget = findingTarget;
        this.agent = enemy.agent;
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
        Vector3 suspectPos = this.findingTarget.findingTargetComponent.suspectPos;
        agent.SetDestination(suspectPos);

        Vector3 moveDir = agent.steeringTarget - enemy.transform.position;

        enemyController.Move(moveDir, 1);

        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        switch (combatPhase)
        {
            case CombatOffensiveInstinct.CombatPhase.SemiAlert: 
                {
                    enemyController.AimDownSight();
                    enemyController.RotateToPos(suspectPos, 7);
                }
                break;
            case CombatOffensiveInstinct.CombatPhase.Suspect:
                {
                    enemyController.LowReady();

                    if (agent.hasPath)
                        enemyController.RotateToPos(agent.steeringTarget, 7);
                    else
                        enemyController.RotateToPos(suspectPos, 7);
                }
                break;
        }
        
        base.Update();
    }
}
