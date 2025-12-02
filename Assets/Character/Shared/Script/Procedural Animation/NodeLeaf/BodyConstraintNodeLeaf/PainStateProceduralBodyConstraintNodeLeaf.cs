using System;
using UnityEngine;

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

    

    public PainStateProceduralBodyConstraintNodeLeaf(
        Transform root,
        BodyLookConstrain splineLookConstrain,
        AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject,
        Func<bool> precondition
    ) : this(
        root
        ,splineLookConstrain
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
        this .root = root;
    }

    // *********************************************
    // MAIN UPDATE
    // *********************************************
    public override void UpdateNode()
    {
        this.painPointPosition = root.TransformPoint(deltaPos_RootSpace);
        this.pullBackPainPointPosition = root.TransformPoint(deltaPosPull_RootSpace);
        base.UpdateNode();
    }


    // *********************************************
    // INITIAL HIT CALCULATION
    // *********************************************
    public void SetPainPointPosition(Vector3 hitPoint, Vector3 hitDirection, float pullBackDistance)
    {
        this.painPointPosition = hitPoint;

        // WORLD ? ROOT SPACE
        Vector3 targetDirection = (hitPoint - root.position).normalized;
        deltaPos_RootSpace = root.InverseTransformPoint(hitPoint);
        deltaRot_RootSpace = Quaternion.FromToRotation(Vector3.forward, targetDirection);

        // Pull-back exaggerated point
        pullBackPainPointPosition = hitPoint + hitDirection.normalized * pullBackDistance;

        Vector3 pullBackDir = (pullBackPainPointPosition - root.position).normalized;
        deltaPosPull_RootSpace = root.InverseTransformPoint(pullBackPainPointPosition);
        deltaRotPull_RootSpace = Quaternion.FromToRotation(Vector3.forward, pullBackDir);
    }


    // *********************************************
    // UPDATE LOOK-AT TARGET
    // *********************************************
    protected override void UpdateLookAtTarget()
    {
        
    }


    // *********************************************
    // UPDATE WEIGHT SMOOTHLY
    // *********************************************
    protected override void UpdateWeight()
    {
        
    }
}
