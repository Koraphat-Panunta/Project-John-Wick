using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;

public class PainStateProceduralBodyConstraintNodeLeaf : LookBodyConstraintNodeLeaf
{

    public Vector3 painPointPosition;       
    protected Vector3 deltaPos_RootSpace;

    public Transform root;

    private Vector3 rootPosition => root.position + (Vector3.up) ;

    public Vector3 painLookAtPos;

    public float timer;
    public float maxTimer = 2;
    public float painTimeNormalzied => this.painRespondCurve.Evaluate(Mathf.Clamp01(this.timer/this.maxTimer));
    public AnimationCurve painRespondCurve;

    public BalancePointComponent balancePointComponent;

    public PainStateProceduralBodyConstraintNodeLeaf(
        Transform root,
        BodyLookConstrain splineLookConstrain,
        AnimationCurve painRespondCurve,
        AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject,
        Func<bool> precondition
    ) : this(
        root
        , splineLookConstrain
        , painRespondCurve
        , aimSplineLookConstrainScriptableObject.offsetSpline
        , aimSplineLookConstrainScriptableObject.offsetSpline1
        , aimSplineLookConstrainScriptableObject.offsetSpline2
        , aimSplineLookConstrainScriptableObject.weightSpline
        , aimSplineLookConstrainScriptableObject.weightSpline1
        , aimSplineLookConstrainScriptableObject.weightSpline2      
        , aimSplineLookConstrainScriptableObject.offsetChangedRate
        , precondition)
    {

    }

    public PainStateProceduralBodyConstraintNodeLeaf(
        Transform root
        , BodyLookConstrain splineLookConstrain
        ,AnimationCurve painRespondCurve
       , Vector3 offsetSpline
       , Vector3 offsetSpline1
       , Vector3 offsetSpline2
       , float weightSpline
       , float weightSpline1
       , float weightSpline2
       , float offsetChangeRate
       , Func<bool> precondition) : base
        (
           splineLookConstrain
           ,offsetSpline
           ,offsetSpline1
           ,offsetSpline2
           ,weightSpline
           ,weightSpline1
           ,weightSpline2
           ,offsetChangeRate
           ,precondition)
    {
        this.painRespondCurve = painRespondCurve;
        this.root = root;
        this.balancePointComponent = new BalancePointComponent(
            this.root
            ,new Vector3(0,.5f,.5f)
            ,new Vector3(.05f,.2f,0)
            ,new Vector3(1,.75f,0));
    }


    public override void Enter()
    {
        this.painLookAtPos = bodyLookConstrain.bodyLookAtPosition.position;

        this.timer = 0;
        base.Enter();
    }
   
    public void SetPainProperties(Vector3 hitPoint, Vector3 hitDirection,float intensity)
    {
        this.timer = 0;
        float distancRootToHitPoint = Vector3.Distance(hitPoint, rootPosition); 
        Vector3 rootToHitPointDir = (hitPoint - rootPosition).normalized;

        this.painPointPosition = rootPosition + rootToHitPointDir * (distancRootToHitPoint + .25f);


        float dot = Vector3.Dot((this.painPointPosition - rootPosition).normalized, root.forward);

        if (dot < 0)
            this.painPointPosition = rootPosition + (new Vector3( - rootToHitPointDir.x, rootToHitPointDir.y, - rootToHitPointDir.z));

        // WORLD ? ROOT SPACE
        Vector3 targetDirection = (painPointPosition - root.position).normalized;
        deltaPos_RootSpace = root.InverseTransformPoint(painPointPosition);

        this.intensity = intensity;
    }

    public override void UpdateNode()
    {
        this.timer += Time.deltaTime;
        this.balancePointComponent.UpdateBalancePoint();
        base.UpdateNode();
    }
    protected override void UpdateLookAtTarget()
    {

        this.painPointPosition = root.TransformPoint(deltaPos_RootSpace);

        Vector3[] cts = new Vector3[]{GetHitPullPointPosition()};

        this.painLookAtPos = BezierurveBehavior.GetPointOnBezierCurve
            (this.painLookAtPos
            , cts
            , this.balancePointComponent.balancePointLookAt
            , this.painTimeNormalzied
            );
       
        this.bodyLookConstrain.SetLookAtPosition
            (
            painLookAtPos
            );
    }



    private Vector3 GetHitPullPointPosition()
    {
        Vector3 rootToPainDir = (painPointPosition - rootPosition).normalized;

        Vector3 rootForwardPoint = rootPosition + root.forward;

        Vector3 hitPoint = rootPosition + rootToPainDir;

        Vector3 rootForwardPointToHitPointDir = (hitPoint - rootForwardPoint).normalized;

        Vector3 hitPullPosition = rootForwardPoint + (rootForwardPointToHitPointDir * 1);

        return hitPullPosition;
    }

    protected float intensity;
    protected override void UpdateWeight()
    {
        bodyLookConstrain.SetWeight(Mathf.Clamp(bodyLookConstrain.GetWeight() + (base.getOffsetChangedRate * Time.deltaTime),0,this.intensity));
    }

   
}
