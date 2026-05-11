using System;
using Unity.Mathematics;
using UnityEngine;

public class InsistEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private Vector3 insistPos;
    private float distance;
    protected EnemyDecisionContext enemyDecisionContext;
    public InsistEnemyActionNodeLeaf(Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        , EnemyDecision enemyDecision
        ,EnemyDecisionContext enemyDecisionContext) 
        : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.enemyDecisionContext = enemyDecisionContext;
    }

    public override void Enter()
    {
        insistPos = enemy.transform.position;
        distance = UnityEngine.Random.Range(1, 3);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override void UpdateNode()
    {
        switch (this.enemyDecisionContext.combatPhase)
        {
            case CombatPhase.Alert:
                {
                    enemyCommandAPI.FreezPosition();

                    EnemyOffendCommandWeaponBased.Engage(this.enemy, this.enemyCommandAPI, this.enemy.targetKnowPos);

                    enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();

                    //if (Vector3.Distance(insistPos, enemy.transform.position) < distance 
                    //    && Vector3.Distance(enemy.transform.position, enemy.targetKnewPos) > f)
                    //    enemyCommandAPI.MoveToPosition(enemy.targetKnewPos, enemy.moveMaxSpeed);
                    //else
                    //    enemyCommandAPI.FreezPosition();

                }
                break;
            case CombatPhase.Aware:
                {
                    enemyCommandAPI.FreezPosition();
                    EnemyOffendCommandWeaponBased.Hold(this.enemy, this.enemyCommandAPI, this.enemy.targetKnowPos);
                    enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();

                }
                break;
            case CombatPhase.Chill:
                {
                    enemyCommandAPI.FreezPosition();
                    EnemyOffendCommandWeaponBased.Rest(this.enemy, this.enemyCommandAPI);
                }
                break;
        }
        base.UpdateNode();
    }
}
