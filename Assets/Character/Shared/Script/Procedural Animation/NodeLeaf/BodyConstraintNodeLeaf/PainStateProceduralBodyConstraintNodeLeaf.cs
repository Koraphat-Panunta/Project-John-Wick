using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;

public class PainStateProceduralBodyConstraintNodeLeaf : LookBodyConstraintNodeLeaf
{

    // Input data
    public Vector3 painPointPosition;        // world-space position where the hit happened
    public Vector3 pullBackPainPointPosition; // exaggerated pull-back position

    // Stored deltas relative to root
    protected Vector3 deltaPos_RootSpace;
    protected Quaternion deltaRot_RootSpace;

    protected Vector3 deltaPosPull_RootSpace;
    protected Quaternion deltaRotPull_RootSpace;

    public Transform root;

    private Vector3 rootPosition => root.position + (Vector3.up) ;

    public float pullWeight;

    public Vector3 painLookAtPos;

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
        this.painPointPosition = hitPoint;
        float dot = Vector3.Dot((this.painPointPosition - rootPosition).normalized, root.forward);
        Debug.Log("Dot = " + dot);
        if (dot < 0)
            this.painPointPosition = rootPosition + ((this.painPointPosition - rootPosition).normalized * -2.5f);

        // WORLD ? ROOT SPACE
        Vector3 targetDirection = (painPointPosition - root.position).normalized;
        deltaPos_RootSpace = root.InverseTransformPoint(painPointPosition);
        deltaRot_RootSpace = Quaternion.FromToRotation(Vector3.forward, targetDirection);

        // Pull-back exaggerated point
        pullBackPainPointPosition = painPointPosition + hitDirection.normalized * pullBackDistance;

        Vector3 pullBackDir = (pullBackPainPointPosition - root.position).normalized;
        deltaPosPull_RootSpace = root.InverseTransformPoint(pullBackPainPointPosition);
        deltaRotPull_RootSpace = Quaternion.FromToRotation(Vector3.forward, pullBackDir);
    }

    protected override void UpdateLookAtTarget()
    {
        pullWeight = Mathf.Clamp01(pullWeight + (Time.deltaTime));
        this.painPointPosition = root.TransformPoint(deltaPos_RootSpace);
        this.pullBackPainPointPosition = root.TransformPoint(deltaPosPull_RootSpace);

        Vector3 rootToPainDir = (painPointPosition - rootPosition).normalized;

        Vector3 rootForwardPoint = rootPosition + root.forward;

        Vector3 hitPoint = rootPosition + rootToPainDir;

        Vector3 rootForwardPointToHitPointDir = (hitPoint - rootForwardPoint).normalized;

        Vector3 hitPullPosition = rootForwardPoint + (rootForwardPointToHitPointDir * 1);

        painLookAtPos = Vector3.Lerp(painLookAtPos, hitPullPosition,(Time.deltaTime * 5.6f));
        //bodyLookConstrain.SetLookAtPosition
        //    (
        //    Vector3.Lerp(painPointPosition, pullBackPainPointPosition, pullWeight)
        //    );
        bodyLookConstrain.SetLookAtPosition
            (
            painLookAtPos
            );
    }

    protected override void UpdateWeight()
    {
        bodyLookConstrain.SetWeight(bodyLookConstrain.GetWeight() + (base.getOffsetChangedRate * Time.deltaTime));
    }
}
