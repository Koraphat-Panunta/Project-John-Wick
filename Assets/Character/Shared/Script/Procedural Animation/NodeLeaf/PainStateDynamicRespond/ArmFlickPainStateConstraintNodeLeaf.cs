using System;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class ArmFlickPainStateConstraintNodeLeaf : ArmIKConstraintNodeLeaf
{
    public Vector3 painPointPosition;
    protected Vector3 root_Delta_PainPoint;

    public Vector3 painLookAtPos;

    public float timer;
    public float maxTimer = 1;
    public float painTimeNormalzied => this.painRespondCurve.Evaluate(Mathf.Clamp01(this.timer / this.maxTimer));
    public AnimationCurve painRespondCurve;
    protected float intensity;

    protected Vector3 root_DirOffset;
    public Vector3 rootDir => Quaternion.Euler(this.root_DirOffset) * base.rootIKHandRef.forward;

    public BalancePointComponent balancePointComponent { get; protected set; }

    public Vector3 beginPullPosition;
    public Vector3 root_Delta_beginPullPosition;

    public Vector3 pullPoint;
    public Vector3 root_Delta_PullPoint;

    public Vector3 middlePullPoint;
    public Vector3 root_Delta_middlePullPoint;

    public ArmFlickPainStateConstraintNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
        , TransformOffsetSCRP transformHintOffsetSCRP
        , AnimationCurve painStateRespondCurve
        , Vector3 root_DirOffset
        ) 
        : base(
            handArmIKConstraintManager
            , rootIKHandRef
            , precondition
            , transformHintOffsetSCRP
            )
    {
        this.root_DirOffset = root_DirOffset;
        this.painRespondCurve = painStateRespondCurve;
        this.balancePointComponent = new BalancePointComponent(this.rootIKHandRef,new Vector3(-.25f,-.2f,.45f),new Vector3(.05f, .05f, .1f),new Vector3(2f, .75f, 2f));
    }
    public override void Enter()
    {
        this.timer = 0;

        this.beginPullPosition = base.handArmIKConstraintManager.GetTargetHandTransform().position;
        this.root_Delta_beginPullPosition = this.rootIKHandRef.InverseTransformPoint(this.beginPullPosition);

        base.Enter();
    }
    public void SetFlickProperties(Vector3 hitPoint, Vector3 hitDirection)
    {
        this.timer = 0;
        this.painPointPosition = hitPoint;
        this.root_Delta_PainPoint = rootIKHandRef.InverseTransformPoint(this.painPointPosition);

        this.pullPoint = this.balancePointComponent.centerPosition + (Vector3.up * .2f) + (hitDirection * .5f *(Vector3.Dot(hitDirection,this.rootDir) < 0?-1:1));
        this.root_Delta_PullPoint = base.rootIKHandRef.InverseTransformPoint(this.pullPoint);

        this.middlePullPoint = Vector3.Lerp(this.rootIKHandRef.position,this.pullPoint,.5f) + (this.rootDir * .4f) + (Vector3.up * UnityEngine.Random.Range(-.35f,.35f));
        this.root_Delta_middlePullPoint = base.rootIKHandRef.InverseTransformPoint(this.middlePullPoint);

        this.beginPullPosition = base.handArmIKConstraintManager.GetTargetHandTransform().position;
        this.root_Delta_beginPullPosition = this.rootIKHandRef.InverseTransformPoint(this.beginPullPosition);

    }
    public override void UpdateNode()
    {
        timer += Time.deltaTime;

        this.balancePointComponent.UpdateBalancePoint();
        base.UpdateNode();
    }
    protected override void UpdateTargetHandPosition()
    {
        this.painPointPosition = this.rootIKHandRef.TransformPoint(this.root_Delta_PainPoint);
        this.beginPullPosition = this.rootIKHandRef.TransformPoint(this.root_Delta_beginPullPosition);
        this.pullPoint = this.rootIKHandRef.TransformPoint(this.root_Delta_PullPoint);
        this.middlePullPoint = this.rootIKHandRef.TransformPoint(this.root_Delta_middlePullPoint);

        Vector3[] ct = 
            { this.middlePullPoint
                ,this.pullPoint
        };

        this.painLookAtPos = BezierurveBehavior.GetPointOnBezierCurve(this.beginPullPosition, ct, this.balancePointComponent.balancePointLookAt, this.painTimeNormalzied);
        this.handArmIKConstraintManager.SetTargetHand(this.painLookAtPos, this.handArmIKConstraintManager.twoBoneIKConstraint.data.mid.rotation);
    }
}
