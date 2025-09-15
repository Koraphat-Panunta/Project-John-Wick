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

    private IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision;

    public SwarpCombatPositionActionNodeLeaf(Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        ,IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision
        , EnemyDecision enemyDecision) : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.enemyActionNodeManagerImplementDecision = enemyActionNodeManagerImplementDecision;
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

                    switch (this.enemyActionNodeManagerImplementDecision._curCombatPhase)
                    {
                        case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware:
                            {
                                if (enemyCommandAPI.SprintToPosition(this.swarpPosition, 1, 2))
                                {
                                    curSwarpPhase = SwarPositionPhase.moveToTarget;
                                }
                            }
                            break;
                        case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert:
                            {
                                if (enemyCommandAPI.MoveToPosition(this.swarpPosition, 1, 2))
                                {
                                    curSwarpPhase = SwarPositionPhase.moveToTarget;
                                }

                                enemyCommandAPI.AimDownSight(enemy.targetKnewPos);
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
                        ,(enemy.targetKnewPos - enemy.transform.position).normalized
                        , (enemy.targetKnewPos - enemy.transform.position).magnitude
                        ,LayerMask.GetMask("Default")
                        , QueryTriggerInteraction.Ignore) == false || moveToTargetTimer <= 0)
                    {
                        isComplete = true;
                    }



                    if(enemyCommandAPI.MoveToPositionRotateToward(enemy.targetKnewPos, 1, 1,2))
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

        Vector3 targetToEnemyDir =  enemy.transform.position - enemy.targetKnewPos;
        targetToEnemyDir.y = 0;
        targetToEnemyDir.Normalize();



        Vector3 assignPos = enemy.targetKnewPos + ((Quaternion.AngleAxis(rotateOverwatch, Vector3.up) * targetToEnemyDir) * raduisOverwatch);
        if (Physics.Raycast(enemy.targetKnewPos
            , (assignPos - enemy.targetKnewPos).normalized
            , out RaycastHit hit
            , (assignPos - enemy.targetKnewPos).magnitude
            , LayerMask.GetMask("Default")
            , QueryTriggerInteraction.Ignore)
            && Vector3.Distance(hit.point, enemy.targetKnewPos) >= randomMinRangeOverwatchZone)
        {
            assignPos = hit.point;
        }

        return assignPos;
    }
}
