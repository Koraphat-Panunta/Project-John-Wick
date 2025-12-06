using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;

public class PainStateProceduralBodyConstraintNodeLeaf : LookBodyConstraintNodeLeaf
{

    // Input data
    public Vector3 painPointPosition;        // world-space position where the hit happened

    // Stored deltas relative to root
    protected Vector3 deltaPos_RootSpace;
    protected Quaternion deltaRot_RootSpace;

    public Transform root;

    private Vector3 rootPosition => root.position + (Vector3.up) ;

    public float pullWeight;

    public Vector3 painLookAtPos;

    public Vector3 balancePointLookAt;

    public PainStateProceduralBodyConstraintNodeLeaf(
        Transform root,
        BodyLookConstrain splineLookConstrain,
        AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject,
        Func<bool> precondition
    ) : this(
        root
        , splineLookConstrain
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
        this.root = root;
    }

    public override void Enter()
    {
        this.painLookAtPos = bodyLookConstrain.bodyLookAtPosition.position;
        pullWeight = 0;


        base.Enter();
    }
   


    public void SetPainPointPosition(Vector3 hitPoint, Vector3 hitDirection, float pullBackDistance)
    {
        float distancRootToHitPoint = Vector3.Distance(hitPoint, rootPosition); 
        Vector3 rootToHitPointDir = (hitPoint - rootPosition).normalized;

        this.painPointPosition = rootPosition + rootToHitPointDir * (distancRootToHitPoint + .25f);


        float dot = Vector3.Dot((this.painPointPosition - rootPosition).normalized, root.forward);
        Debug.Log("Dot = " + dot);
        if (dot < 0)
            this.painPointPosition = rootPosition + (new Vector3( - rootToHitPointDir.x, rootToHitPointDir.y, - rootToHitPointDir.z));

        // WORLD ? ROOT SPACE
        Vector3 targetDirection = (painPointPosition - root.position).normalized;
        deltaPos_RootSpace = root.InverseTransformPoint(painPointPosition);
        deltaRot_RootSpace = Quaternion.FromToRotation(Vector3.forward, targetDirection);


    }

    protected override void UpdateLookAtTarget()
    {
        pullWeight = Mathf.Clamp01(pullWeight + (Time.deltaTime));
        this.painPointPosition = root.TransformPoint(deltaPos_RootSpace);

        Vector3 rootToPainDir = (painPointPosition - rootPosition).normalized;

        Vector3 rootForwardPoint = rootPosition + root.forward;

        Vector3 hitPoint = rootPosition + rootToPainDir;

        Vector3 rootForwardPointToHitPointDir = (hitPoint - rootForwardPoint).normalized;

        Vector3 hitPullPosition = rootForwardPoint + (rootForwardPointToHitPointDir * 1);

        painLookAtPos = Vector3.Lerp(painLookAtPos, hitPullPosition,(Time.deltaTime * 5.6f));
       
        bodyLookConstrain.SetLookAtPosition
            (
            painLookAtPos
            );
    }

    protected virtual void UpdateBalancePoint()
    {

    }

    protected override void UpdateWeight()
    {
        bodyLookConstrain.SetWeight(bodyLookConstrain.GetWeight() + (base.getOffsetChangedRate * Time.deltaTime));
    }
}
