using System;
using UnityEngine;

public abstract class BodyRotationConstraintNodeLeaf : AnimationConstrainNodeLeaf
{
    protected BodyConstraintManager bodyConstraint;

    public BodyRotationConstrainScriptableObject bodyRotationConstrainScriptableObject;

    public virtual Vector3 getOffsetConstraint => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.offsetConstraint : this._offsetConstraint;
    public virtual Vector3 getOffsetConstraint1 => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.offsetConstraint1 : this._offsetConstraint1;
    public virtual Vector3 getOffsetConstraint2 => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.offsetConstraint2 : this._offsetConstraint2;

    private Vector3 _offsetConstraint;
    private Vector3 _offsetConstraint1;
    private Vector3 _offsetConstraint2;

    public float getWeightConstraint => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.weightConstraint : this._weightConstraint;
    public float getWeightConstraint1 => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.weightConstraint1 : this._weightConstraint1;
    public float getWeightConstraint2 => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.weightConstraint2 : this._weightConstraint2;

    private float _weightConstraint;
    private float _weightConstraint1;
    private float _weightConstraint2;

    public float getOffsetChangedRate => this.bodyRotationConstrainScriptableObject ? this.bodyRotationConstrainScriptableObject.offsetChangedRate : this._offsetChangedRate;
    private float _offsetChangedRate;

    private Vector3 _smoothedOffset;
    private Vector3 _smoothedOffset1;
    private Vector3 _smoothedOffset2;

    public Vector3 currentSmoothedOffset  => _smoothedOffset;
    public Vector3 currentSmoothedOffset1 => _smoothedOffset1;
    public Vector3 currentSmoothedOffset2 => _smoothedOffset2;

    protected float weight;

    public BodyRotationConstraintNodeLeaf(
        BodyConstraintManager bodyConstraint
        , BodyRotationConstrainScriptableObject bodyRotationConstrainScriptableObject
        , Func<bool> precondition)
        : this(
              bodyConstraint
              , bodyRotationConstrainScriptableObject.offsetConstraint
              , bodyRotationConstrainScriptableObject.offsetConstraint1
              , bodyRotationConstrainScriptableObject.offsetConstraint2
              , bodyRotationConstrainScriptableObject.weightConstraint
              , bodyRotationConstrainScriptableObject.weightConstraint1
              , bodyRotationConstrainScriptableObject.weightConstraint2
              , bodyRotationConstrainScriptableObject.offsetChangedRate
              , precondition)
    {
        this.bodyRotationConstrainScriptableObject = bodyRotationConstrainScriptableObject;
    }

    public BodyRotationConstraintNodeLeaf(
        BodyConstraintManager bodyConstraint
        , Vector3 offsetConstraint
        , Vector3 offsetConstraint1
        , Vector3 offsetConstraint2
        , float weightConstraint
        , float weightConstraint1
        , float weightConstraint2
        , float offsetChangedRate
        , Func<bool> precondition) : base(precondition)
    {
        this.bodyConstraint = bodyConstraint;

        this._offsetConstraint = offsetConstraint;
        this._offsetConstraint1 = offsetConstraint1;
        this._offsetConstraint2 = offsetConstraint2;

        this._weightConstraint = weightConstraint;
        this._weightConstraint1 = weightConstraint1;
        this._weightConstraint2 = weightConstraint2;

        this._offsetChangedRate = offsetChangedRate;
    }

    public override void Enter()
    {
        _smoothedOffset  = getOffsetConstraint;
        _smoothedOffset1 = getOffsetConstraint1;
        _smoothedOffset2 = getOffsetConstraint2;
        this.weight = 0;
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

        _smoothedOffset  = Vector3.MoveTowards(_smoothedOffset,  getOffsetConstraint,  Time.deltaTime * getOffsetChangedRate);
        _smoothedOffset1 = Vector3.MoveTowards(_smoothedOffset1, getOffsetConstraint1, Time.deltaTime * getOffsetChangedRate);
        _smoothedOffset2 = Vector3.MoveTowards(_smoothedOffset2, getOffsetConstraint2, Time.deltaTime * getOffsetChangedRate);

        this.bodyConstraint.SetAllConstraintOffsetData(_smoothedOffset, _smoothedOffset1, _smoothedOffset2);
        this.bodyConstraint.SetAllConstraintWeights(
            this.getWeightConstraint,
            this.getWeightConstraint1,
            this.getWeightConstraint2
        );

        base.UpdateNode();
    }

    public void SetBodyRotationConstrainSCRP(BodyRotationConstrainScriptableObject scrp)
    {
        this.bodyRotationConstrainScriptableObject = scrp;
    }
    public void SetWeight(float w) => this.weight = w;

    protected abstract void UpdateWeight();
    protected abstract void UpdateLookAtTarget();
}
