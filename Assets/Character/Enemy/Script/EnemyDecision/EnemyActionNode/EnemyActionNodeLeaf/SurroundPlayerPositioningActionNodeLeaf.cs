using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Positions non-engaging (Support role) enemies in a ring around the player,
// distributing angles evenly between all surrounding enemies.
// Enemies drift laterally and face the player until the director commands them to engage.
public class SurroundPlayerPositioningActionNodeLeaf : EnemyActionNodeLeaf
{
    private static readonly List<SurroundPlayerPositioningActionNodeLeaf> s_activeInstances
        = new List<SurroundPlayerPositioningActionNodeLeaf>();

    private EnemyDecisionContext enemyDecisionContext;

    private const float SURROUND_RADIUS_MIN = 5f;
    public const float SURROUND_RADIUS_MAX = 8f;
    private const float POSITION_REACH_DISTANCE = 1.5f;
    private const float REPOSITION_INTERVAL_MIN = 3f;
    private const float REPOSITION_INTERVAL_MAX = 12f;
    private const float DRIFT_SPEED_DEG_PER_SEC = 12f;
    private const float PLAYER_MOVE_REPOSITION_THRESHOLD = 3f;
    private const float NAVMESH_SAMPLE_RADIUS = 3f;
    private const float MAX_MOVE_VELOCITY_SCALE = 0.5f;

    private float curMoveVelocityScale = 0;
    private float targetMoveVelocityScale = 0;

    private Vector3 targetPosition;
    private Vector3 curTargetPosition;

    public float currentAngle { get; private set; }
    private float currentRadius;
    private float repositionTimer;
    private Vector3 lastKnownPlayerPos;
    private float driftDirection;

    public SurroundPlayerPositioningActionNodeLeaf(
        Enemy enemy,
        EnemyCommandAPI enemyCommandAPI,
        Func<bool> preCondition,
        EnemyDecision enemyDecision,
        EnemyDecisionContext enemyDecisionContext)
        : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.enemyDecisionContext = enemyDecisionContext;
    }

    public override void Enter()
    {
        Debug.Log("Enter");
        this.curTargetPosition = this.enemy.transform.position;
        s_activeInstances.Add(this);
        driftDirection = UnityEngine.Random.value > 0.5f ? 1f : -1f;
        currentRadius = UnityEngine.Random.Range(SURROUND_RADIUS_MIN, SURROUND_RADIUS_MAX);
        currentAngle = FindBestSurroundAngle();
        lastKnownPlayerPos = enemy.targetKnowPos;
        repositionTimer = UnityEngine.Random.Range(REPOSITION_INTERVAL_MIN, REPOSITION_INTERVAL_MAX);
        RefreshTargetPosition();
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("Exit");
        s_activeInstances.Remove(this);
        base.Exit();
    }

    public override void UpdateNode()
    {
        currentAngle += driftDirection * DRIFT_SPEED_DEG_PER_SEC * Time.deltaTime;

        if (Vector3.Distance(enemy.targetKnowPos, lastKnownPlayerPos) > PLAYER_MOVE_REPOSITION_THRESHOLD)
        {
            lastKnownPlayerPos = enemy.targetKnowPos;
            RefreshTargetPosition();
        }

        repositionTimer -= Time.deltaTime;
        if (repositionTimer <= 0f)
        {
            currentAngle = FindBestSurroundAngle();
            repositionTimer = UnityEngine.Random.Range(REPOSITION_INTERVAL_MIN, REPOSITION_INTERVAL_MAX);
            RefreshTargetPosition();
        }

        this.curMoveVelocityScale = Mathf.MoveTowards(this.curMoveVelocityScale, this.targetMoveVelocityScale, Time.deltaTime * .5f);
        this.curTargetPosition = Vector3.MoveTowards(this.curTargetPosition, this.targetPosition, Time.deltaTime * 5);

        Debug.DrawLine(this.curTargetPosition, this.targetPosition, Color.yellow);

        if(this.enemyCommandAPI.MoveToPosition(this.targetPosition, this.curMoveVelocityScale, POSITION_REACH_DISTANCE))
        {
            this.enemyCommandAPI.FreezPosition();
        }


        switch (enemyDecisionContext.combatPhase)
        {
            case CombatPhase.Alert:
                EnemyOffendCommandWeaponBased.Engage(this.enemy, this.enemyCommandAPI, this.enemy.targetKnowPos);
                enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();
                break;
            case CombatPhase.Aware:
                EnemyOffendCommandWeaponBased.Hold(this.enemy, this.enemyCommandAPI, this.enemy.targetKnowPos);
                break;
            default:
                EnemyOffendCommandWeaponBased.Rest(this.enemy, this.enemyCommandAPI);
                break;
        }

        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete() => false;

    public override bool IsReset() => base.IsReset();

    private void RefreshTargetPosition()
    {
        this.curMoveVelocityScale = 0;
        this.targetMoveVelocityScale = UnityEngine.Random.Range(0, MAX_MOVE_VELOCITY_SCALE);
        Vector3 offset = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward * currentRadius;
        Vector3 desired = enemy.targetKnowPos + offset;

        if (NavMesh.SamplePosition(desired, out NavMeshHit hit, NAVMESH_SAMPLE_RADIUS, NavMesh.AllAreas))
            targetPosition = hit.position;
        else
            targetPosition = desired;
    }

    // Finds the angle that sits at the midpoint of the largest angular gap between other surrounding enemies.
    private float FindBestSurroundAngle()
    {
        var otherAngles = new List<float>();
        foreach (var inst in s_activeInstances)
        {
            if (inst != this)
                otherAngles.Add(((inst.currentAngle % 360f) + 360f) % 360f);
        }

        if (otherAngles.Count == 0)
            return UnityEngine.Random.Range(0f, 360f);

        otherAngles.Sort();

        float bestMidAngle = otherAngles[0] + 180f;
        float bestGap = 0f;

        for (int i = 0; i < otherAngles.Count; i++)
        {
            float a = otherAngles[i];
            float b = i + 1 < otherAngles.Count
                ? otherAngles[i + 1]
                : otherAngles[0] + 360f;

            float gap = b - a;
            if (gap > bestGap)
            {
                bestGap = gap;
                bestMidAngle = a + gap * 0.5f;
            }
        }

        return bestMidAngle % 360f;
    }
}
