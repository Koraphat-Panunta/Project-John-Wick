using System;
using UnityEngine;

public class PlayerLeaningRotationConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    public LeaningRotaionScriptableObject leaningScriptableObject { get; protected set; }

    private BodyConstraintManager _bodyConstraintManager;
    private BodyRotationConstraintNodeLeaf _bodyRotationNode;
    private IRangeWeaponAdvanceUser _weaponAdvanceUser;
    private Player _player;

    private float _targetLeanWeight;
    private float _currentLean;
    private float _leanVelocity;
    private float _distance;
    private float _checkTimer;

    private const float CHECK_INTERVAL = 0.067f;

    private int numberRaycast       => leaningScriptableObject.numberRaycast;
    private float checkDistance      => leaningScriptableObject.checkDistance;
    private Vector3 castAnchorPos    => _player.RayCastPos.position;
    private float multipleTargetWeight => Mathf.Clamp01(1 - Mathf.Clamp01(
        (_distance - leaningScriptableObject.minDistanceCheck) /
        (leaningScriptableObject.maxDistanceCheck - leaningScriptableObject.minDistanceCheck)));

    public PlayerLeaningRotationConstrainNodeLeaf(
        Player player,
        LeaningRotaionScriptableObject leaningScriptableObject,
        BodyConstraintManager bodyConstraintManager,
        BodyRotationConstraintNodeLeaf bodyRotationNode,
        IRangeWeaponAdvanceUser weaponAdvanceUser,
        Func<bool> precondition) : base(precondition)
    {
        _player               = player;
        this.leaningScriptableObject = leaningScriptableObject;
        _bodyConstraintManager = bodyConstraintManager;
        _bodyRotationNode      = bodyRotationNode;
        _weaponAdvanceUser     = weaponAdvanceUser;
    }

    public override void Enter()
    {
        _targetLeanWeight = 0;
        _currentLean      = 0;
        _leanVelocity     = 0;
        base.Enter();
    }

    public override void UpdateNode()
    {
        Debug.Log("LeaningUpdate");

        float goal = _targetLeanWeight * multipleTargetWeight;
        float smoothTime = Mathf.Approximately(goal, 0f)
            ? leaningScriptableObject.recoverySmoothTime
            : leaningScriptableObject.leanSmoothTime;

        _currentLean = Mathf.SmoothDamp(_currentLean, goal, ref _leanVelocity, smoothTime);

        _bodyConstraintManager.SetAllConstraintOffsetData(
            _bodyRotationNode.currentSmoothedOffset  + leaningScriptableObject.spine0LeanOffset * _currentLean,
            _bodyRotationNode.currentSmoothedOffset1 + leaningScriptableObject.spine1LeanOffset * _currentLean,
            _bodyRotationNode.currentSmoothedOffset2 + leaningScriptableObject.spine2LeanOffset * _currentLean);

        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        UpdateLeanDetection();
        base.FixedUpdateNode();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void UpdateLeanDetection()
    {
        _checkTimer += Time.fixedDeltaTime;
        if (_checkTimer < CHECK_INTERVAL) return;
        _checkTimer = 0f;

        Vector3 castDir = _weaponAdvanceUser._pointingPos - castAnchorPos;

        if (_player.curShoulderSide == Side.Left)
        {
            Vector3 perpDir = Vector3.Cross(castDir.normalized, Vector3.down);
            _targetLeanWeight = -leaningScriptableObject.leanWeightCurve.Evaluate(CalculateSideWeight(perpDir, castDir));
        }
        else
        {
            Vector3 perpDir = Vector3.Cross(castDir.normalized, Vector3.up);
            _targetLeanWeight = leaningScriptableObject.leanWeightCurve.Evaluate(CalculateSideWeight(perpDir, castDir));
        }
    }

    private float CalculateSideWeight(Vector3 perpDir, Vector3 castDir)
    {
        Vector3 weightBeginPos = castAnchorPos;
        Vector3 weightEndPos   = castAnchorPos;

        for (int i = 0; i <= numberRaycast; i++)
        {
            float offset    = checkDistance * ((float)i / numberRaycast);
            Vector3 castPos = castAnchorPos + perpDir * offset;

            if (i == 0) weightBeginPos = castPos;

            if (!PointingBlock(castPos, castDir, out RaycastHit hit))
            {
                weightEndPos = castPos;
                continue;
            }

            _distance       = Vector3.Distance(hit.point, castPos);
            weightBeginPos += castDir.normalized * (_distance + 0.05f);
            weightEndPos   += castDir.normalized * (_distance + 0.05f);

            bool tooClose      = Vector3.Distance(hit.point, _weaponAdvanceUser._pointingPos) < 0.15f;
            bool hitBeyondGoal = _distance > Vector3.Distance(castPos, _weaponAdvanceUser._pointingPos);

            if (tooClose || hitBeyondGoal) return 0f;

            if (PointingBlock(weightEndPos, (hit.point - weightEndPos).normalized, out RaycastHit hit2))
                return 1f - Mathf.Clamp01(Vector3.Distance(weightBeginPos, hit2.point) / checkDistance);

            return 1f - ((float)i / numberRaycast);
        }

        return 0f;
    }

    private bool PointingBlock(Vector3 startPos, Vector3 dir, out RaycastHit hit) =>
        Physics.Raycast(startPos, dir.normalized, out hit,
            leaningScriptableObject.maxDistanceCheck,
            leaningScriptableObject.castingCheckLayer.value);

    public void SetLeaningRotaionSCRP(LeaningRotaionScriptableObject scrp) => leaningScriptableObject = scrp;
    public void SetTargetLeanWeight(float w) => _targetLeanWeight = w;
}
