using System;
using UnityEngine;

public abstract class LookBodyConstraintNodeLeaf : AnimationConstrainNodeLeaf
{

    protected BodyLookConstrain bodyLookConstrain;

    protected AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject;
                         
    private Vector3 getOffsetSpline { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.offsetSpline : this._offsetSpline; }
    private Vector3 getOffsetSpline1 { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.offsetSpline1 : this._offsetSpline1; }
    private Vector3 getOffsetSpline2 { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.offsetSpline2 : this._offsetSpline2; }

    private Vector3 _offsetSpline;
    private Vector3 _offsetSpline1;
    private Vector3 _offsetSpline2;

    private float getWeightSpline { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.weightSpline : this._weightSpline; }
    private float getWeightSpline1 { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.weightSpline1 : this._weightSpline1; }
    private float getWeightSpline2 { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.weightSpline2 : this._weightSpline2; }

    private float _weightSpline;
    private float _weightSpline1;
    private float _weightSpline2;

    private float getOffsetChangedRate { get => aimSplineLookConstrainScriptableObject ? aimSplineLookConstrainScriptableObject.offsetChangedRate : this._offsetChangedRate; }
    private float _offsetChangedRate;

    public LookBodyConstraintNodeLeaf(
        BodyLookConstrain splineLookConstrain
        , AimBodyConstrainScriptableObject aimSplineLookConstrainScriptableObject
        , Func<bool> precondition) 
        : this(
              splineLookConstrain
              ,aimSplineLookConstrainScriptableObject.offsetSpline
              ,aimSplineLookConstrainScriptableObject.offsetSpline1
              ,aimSplineLookConstrainScriptableObject .offsetSpline2
              ,aimSplineLookConstrainScriptableObject.weightSpline
              ,aimSplineLookConstrainScriptableObject.weightSpline1
              ,aimSplineLookConstrainScriptableObject.weightSpline2
              ,aimSplineLookConstrainScriptableObject.offsetChangedRate
              ,precondition
              )
    {
        this.aimSplineLookConstrainScriptableObject = aimSplineLookConstrainScriptableObject;
    }

    public LookBodyConstraintNodeLeaf(
        BodyLookConstrain splineLookConstrain
        ,Vector3 offsetSpline
        , Vector3 offsetSpline1
        , Vector3 offsetSpline2
        ,float weightSpline
        ,float weightSpline1
        ,float weightSpline2
        ,float offsetChangeRate
        , Func<bool> precondition) : base(precondition)
    {
        this.bodyLookConstrain = splineLookConstrain;

        this._offsetSpline = offsetSpline;
        this._offsetSpline1 = offsetSpline1;
        this._offsetSpline2 = offsetSpline2;

        this._weightSpline = weightSpline;
        this._weightSpline1 = weightSpline1;
        this._weightSpline2 = weightSpline2;

        this._offsetChangedRate = offsetChangeRate;
    }

    public override void Enter()
    {
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

    public override void UpdateNode()
    {
        this.UpdateWeight();
        this.UpdateLookAtTarget();

        this.bodyLookConstrain.SetAllSplineOffsetData
            (

            Vector3.MoveTowards
            (bodyLookConstrain.getOffsetSpline
            , aimSplineLookConstrainScriptableObject.offsetSpline
            , aimSplineLookConstrainScriptableObject.offsetChangedRate * Time.deltaTime)

            , Vector3.MoveTowards
            (bodyLookConstrain.getOffsetSpline1
            , aimSplineLookConstrainScriptableObject.offsetSpline1
            , aimSplineLookConstrainScriptableObject.offsetChangedRate * Time.deltaTime)

            , Vector3.MoveTowards(bodyLookConstrain.getOffsetSpline2
            , aimSplineLookConstrainScriptableObject.offsetSpline2
            , aimSplineLookConstrainScriptableObject.offsetChangedRate * Time.deltaTime)

            );
        this.bodyLookConstrain.SetAllSplineWeight
            (
            this.aimSplineLookConstrainScriptableObject.weightSpline
            , this.aimSplineLookConstrainScriptableObject.weightSpline1
            , this.aimSplineLookConstrainScriptableObject.weightSpline2
            );
        base.UpdateNode();
    }

    protected abstract void UpdateWeight();
    protected abstract void UpdateLookAtTarget();
    

    
}
