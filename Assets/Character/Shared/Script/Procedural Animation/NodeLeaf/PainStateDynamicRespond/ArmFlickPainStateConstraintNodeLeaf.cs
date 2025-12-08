using System;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class ArmFlickPainStateConstraintNodeLeaf : ArmIKConstraintNodeLeaf
{
    public Vector3 painPointPosition;
    protected Vector3 root_Delta_PainPoint;

    public Vector3 painLookAtPos;

    public float timer;
    public float maxTimer = 3;
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
        this.balancePointComponent = new BalancePointComponent
            (this.rootIKHandRef
            ,new Vector3(.05f * (Vector3.Dot(base.rootIKHandRef.right,this.rootDir) > 0?1:-1),0,.25f)
            ,new Vector3(0, .05f, .25f)
            ,new Vector3(0, 1f, 1f)
            );
    }

    int dirMulltiply => (Vector3.Dot(base.rootIKHandRef.right, this.rootDir) > 0 ? 1 : -1);

    public override void Enter()
    {
        this.timer = 0;

        this.beginPullPosition = base.handArmIKConstraintManager.GetTargetHandTransform().position;
        this.root_Delta_beginPullPosition = this.rootIKHandRef.InverseTransformPoint(this.beginPullPosition);

        base.Enter();
    }
    private Vector3 offsetAnchorRootToSwing = new Vector3(.35f,-.1f,.125f);
    public Vector3 anchorPositionFlickSwing => this.rootIKHandRef.position
        + (this.rootIKHandRef.right * (this.offsetAnchorRootToSwing.x) * (Vector3.Dot(this.rootIKHandRef.right,this.rootDir) > 0 ?1:-1))
        + (this.rootIKHandRef.forward * (this.offsetAnchorRootToSwing.z))
        + (this.rootIKHandRef.up * (this.offsetAnchorRootToSwing.y))
        ;
    public void SetFlickProperties(
        Vector3 flickPosition
        ,float pullPointDistance
        ,float midPullDistance
        ,float maxTime
        )
    {
        this.timer = 0;
        this.maxTimer = maxTime;

        this.painPointPosition = flickPosition;
        this.root_Delta_PainPoint = rootIKHandRef.InverseTransformPoint(this.painPointPosition);

        this.beginPullPosition = base.handArmIKConstraintManager.GetTargetHandTransform().position;
        this.root_Delta_beginPullPosition = this.rootIKHandRef.InverseTransformPoint(this.beginPullPosition);

        this.pullPoint = flickPosition;
        this.root_Delta_PullPoint = base.rootIKHandRef.InverseTransformPoint(this.pullPoint);



        this.middlePullPoint = Vector3.Lerp(this.beginPullPosition, this.pullPoint, .5f) + Vector3.Cross((this.pullPoint - this.beginPullPosition).normalized,Vector3.up) * .5f * dirMulltiply;
        this.root_Delta_middlePullPoint = base.rootIKHandRef.InverseTransformPoint(this.middlePullPoint);



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

        //Debug.Log("painTimeNormalzied = " + this.painTimeNormalzied);

        this.painLookAtPos = BezierurveBehavior.GetPointOnBezierCurve(this.beginPullPosition, ct, this.balancePointComponent.balancePointLookAt, this.painTimeNormalzied);
        this.handArmIKConstraintManager.SetTargetHand(this.painLookAtPos, Quaternion.LookRotation((rootIKHandRef.position - base.handArmIKConstraintManager.GetTargetHandTransform().position).normalized,rootIKHandRef.right * dirMulltiply * -1));
    }

}

