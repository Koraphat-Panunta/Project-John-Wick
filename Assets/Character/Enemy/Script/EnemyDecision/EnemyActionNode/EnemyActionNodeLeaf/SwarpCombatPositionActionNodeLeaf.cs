using System;
using UnityEngine;

public class SwarpCombatPositionActionNodeLeaf : EnemyActionNodeLeaf
{
    private float moveToSwarpPositionTimer;
    private const float MIN_RandomMoveToSwarpPositionTime = 4;
    private readonly float MAX_RandomMoveToSwarpPositionTime =8;

    private float moveToTargetTimer;
    private readonly float MIN_RandomMoveToTargetTimer = 6;
    private readonly float MAX_RandomMoveToTargetTimer = 8;

    public enum SwarPositionPhase
    {
        moveToSwarpPosition,
        moveToTarget
    }
    public SwarPositionPhase curSwarpPhase { get; protected set; }
    public Vector3 swarpPosition { get; protected set; }

    private float randomMaxDegreesOverwatchZone = 60;

    private float randomMaxRangeOverwatchZone = 12;
    private float randomMinRangeOverwatchZone = 7f;

    private bool isComplete;

    private EnemyDecisionContext enemyDecisionContext;

    public SwarpCombatPositionActionNodeLeaf(Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        ,EnemyDecisionContext enemyDecisionContext
        , EnemyDecision enemyDecision) : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.enemyDecisionContext = enemyDecisionContext;
    }

    public override void Enter()
    {
        curSwarpPhase = SwarPositionPhase.moveToSwarpPosition;
        moveToSwarpPositionTimer = UnityEngine.Random.Range(MIN_RandomMoveToSwarpPositionTime,MAX_RandomMoveToSwarpPositionTime);
        moveToTargetTimer = UnityEngine.Random.Range(MIN_RandomMoveToTargetTimer,MAX_RandomMoveToTargetTimer);
        this.swarpPosition = this.GetRandomSwarpPosition();
        this.isComplete = false;
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
        return this.isComplete;
    }

    public override bool IsReset()
    {
        return this.IsComplete();
    }

    public override void UpdateNode()
    {
        switch (curSwarpPhase)
        {
            case SwarPositionPhase.moveToSwarpPosition:
                {
                    this.moveToSwarpPositionTimer -= Time.deltaTime;

                    if (this.moveToSwarpPositionTimer <= 0)
                    {

                        curSwarpPhase = SwarPositionPhase.moveToTarget;
                    }

                    switch (this.enemyDecisionContext.combatPhase)
                    {
                        case CombatPhase.Aware:
                            {
                                if (enemyCommandAPI.SprintToPosition(this.swarpPosition, 1, 2))
                                {
                                    curSwarpPhase = SwarPositionPhase.moveToTarget;
                                }
                            }
                            break;
                        case CombatPhase.Alert:
                            {
                                if (enemyCommandAPI.MoveToPosition(this.swarpPosition, 1, 2))
                                {
                                    curSwarpPhase = SwarPositionPhase.moveToTarget;
                                }

                                enemyCommandAPI.AimDownSight(this.enemy.targetKnowPos);
                                enemyCommandAPI.NormalFiringPattern.Performing();
                                enemyCommandAPI.AutoDetectSoftCover();
                                enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();
                            }
                            break;
                    }
                }
                break;
            case SwarPositionPhase.moveToTarget:
                {
                    moveToTargetTimer -= Time.deltaTime;

                    if(Physics.Raycast(enemy.transform.position
                        ,(this.enemy.targetKnowPos - enemy.transform.position).normalized
                        , (this.enemy.targetKnowPos - enemy.transform.position).magnitude
                        ,LayerMask.GetMask("Default")
                        , QueryTriggerInteraction.Ignore) == false || moveToTargetTimer <= 0)
                    {
                        isComplete = true;
                    }



                    if(enemyCommandAPI.MoveToPositionRotateToward(this.enemy.targetKnowPos, 1, 1,2))
                        isComplete = true;

                    enemyCommandAPI.AimDownSight();
                }
                break;
        }
        base.UpdateNode();
    }
    private Vector3 GetRandomSwarpPosition()
    {
        float rotateOverwatch = UnityEngine.Random.Range(-this.randomMaxDegreesOverwatchZone, this.randomMaxDegreesOverwatchZone);
        float raduisOverwatch = UnityEngine.Random.Range(this.randomMinRangeOverwatchZone, this.randomMaxRangeOverwatchZone);

        Vector3 targetToEnemyDir =  enemy.transform.position - this.enemy.targetKnowPos;
        targetToEnemyDir.y = 0;
        targetToEnemyDir.Normalize();



        Vector3 assignPos = this.enemy.targetKnowPos + ((Quaternion.AngleAxis(rotateOverwatch, Vector3.up) * targetToEnemyDir) * raduisOverwatch);
        if (Physics.Raycast(this.enemy.targetKnowPos
            , (assignPos - this.enemy.targetKnowPos).normalized
            , out RaycastHit hit
            , (assignPos - this.enemy.targetKnowPos).magnitude
            , LayerMask.GetMask("Default")
            , QueryTriggerInteraction.Ignore)
            && Vector3.Distance(hit.point, this.enemy.targetKnowPos) >= randomMinRangeOverwatchZone)
        {
            assignPos = hit.point;
        }

        return assignPos;
    }
}
