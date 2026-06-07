using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyConstraintManager : MonoBehaviour, IConstraintManager
{
    [SerializeField] private float weight;

    [SerializeField] private MultiRotationConstraint rotationConstraint;
    [SerializeField] private MultiRotationConstraint rotationConstraint1;
    [SerializeField] private MultiRotationConstraint rotationConstraint2;

    [SerializeField] protected Transform bodyAnchor;
    [SerializeField] protected Transform bodyRotationRef;


    [Range(0, 360)]
    public float edgeEulerRotate;

    [Range(0, 360)]
    public float limitEulerRotateHorizontal;

    [Range(0, 360)]
    public float limitEulerRotateVertical;

    public float targetEulerRotateHorizontal;
    public float eulerRotateHorizontal;

    public float targetEulerRotateVertical;
    public float eulerRotateVertical;

    public float constraintWeight { get; private set; }
    public float constraint1Weight { get; private set; }
    public float constraint2Weight { get; private set; }

    public Vector3 getOffsetConstraint => this.rotationConstraint.data.offset;
    public Vector3 getOffsetConstraint1 => this.rotationConstraint1.data.offset;
    public Vector3 getOffsetConstraint2 => this.rotationConstraint2.data.offset;

    public Quaternion SourceLookRotation => this.bodyRotationRef.rotation;

    private Vector3 targetDir;

    protected Quaternion restRotation;

    protected Quaternion curRotation;

    public void SetWeight(float w)
    {
        this.weight = Mathf.Clamp01(w);
        this.UpdateConstraintWeights();
    }
    public float GetWeight() => this.weight;

    public void SetAllConstraintWeights(float w, float w1, float w2)
    {
        this.SetConstraintWeight(w);
        this.SetConstraint1Weight(w1);
        this.SetConstraint2Weight(w2);
    }
    public void SetConstraintWeight(float w) { this.constraintWeight = w; this.UpdateConstraintWeights(); }
    public void SetConstraint1Weight(float w) { this.constraint1Weight = w; this.UpdateConstraintWeights(); }
    public void SetConstraint2Weight(float w) { this.constraint2Weight = w; this.UpdateConstraintWeights(); }

    private void UpdateConstraintWeights()
    {
        this.rotationConstraint.weight = this.constraintWeight * this.weight;
        this.rotationConstraint1.weight = this.constraint1Weight * this.weight;
        this.rotationConstraint2.weight = this.constraint2Weight * this.weight;
    }

    public void SetAllConstraintOffsetData(Vector3 offset, Vector3 offset1, Vector3 offset2)
    {
        this.SetConstraintOffsetData(offset);
        this.SetConstraint1OffsetData(offset1);
        this.SetConstraint2OffsetData(offset2);
    }
    public void SetConstraintOffsetData(Vector3 offset) => this.rotationConstraint.data.offset = offset;
    public void SetConstraint1OffsetData(Vector3 offset) => this.rotationConstraint1.data.offset = offset;
    public void SetConstraint2OffsetData(Vector3 offset) => this.rotationConstraint2.data.offset = offset;

    public void SetLookDirection(Vector3 dir)
    {
        this.targetDir = dir;
        this.UpdateTargetRotate();
    }

    public void SetLookPos(Vector3 pos)
    {
        this.targetDir = (pos - this.rotationConstraint.data.constrainedObject.position).normalized;
        this.UpdateTargetRotate();
    }
  
    protected void UpdateTargetRotate()
    {
        this.targetEulerRotateHorizontal = Quaternion.FromToRotation(
            new Vector3(this.bodyAnchor.forward.x, this.targetDir.normalized.y, this.bodyAnchor.forward.z),
            this.targetDir.normalized).eulerAngles.y;

        this.targetEulerRotateVertical = Vector3.SignedAngle(
            this.bodyAnchor.forward,
            new Vector3(this.bodyAnchor.forward.x, this.targetDir.normalized.y, this.bodyAnchor.forward.z).normalized,
            this.bodyAnchor.right);

        if (this.eulerRotateHorizontal > 0)
        {
            if (this.targetEulerRotateHorizontal > edgeEulerRotate)
                this.targetEulerRotateHorizontal -= 360;
        }
        else
        {
            this.targetEulerRotateHorizontal -= 360;
            if (this.targetEulerRotateHorizontal <= -edgeEulerRotate)
                this.targetEulerRotateHorizontal += 360;
        }

        if (this.targetEulerRotateVertical > 180)
            this.targetEulerRotateVertical -= 360;

        this.eulerRotateHorizontal = Mathf.Clamp(this.targetEulerRotateHorizontal, -this.limitEulerRotateHorizontal, this.limitEulerRotateHorizontal);

        this.eulerRotateVertical = Mathf.Clamp(this.targetEulerRotateVertical, -this.limitEulerRotateVertical, this.limitEulerRotateVertical);

        Vector3 dir = Quaternion.LookRotation(this.bodyAnchor.forward, Vector3.up)
            * Quaternion.Euler(this.eulerRotateVertical, this.eulerRotateHorizontal, 0)
            * Vector3.forward;

        this.bodyRotationRef.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
        this.curRotation = this.bodyRotationRef.rotation;
    }
    private void LateUpdate()
    {
        this.bodyRotationRef.rotation = this.curRotation;
    }
    public void AssignBone(HumanoidBone humanoidBone)
    {
        this.bodyAnchor = humanoidBone.hips;

        this.GetMultiRotationConstraint_0().data.constrainedObject = humanoidBone._spine_0_Bone;
        this.GetMultiRotationConstraint_1().data.constrainedObject = humanoidBone._spine_1_Bone;
        this.GetMultiRotationConstraint_2().data.constrainedObject = humanoidBone._spine_2_Bone;
    }

    public MultiRotationConstraint GetMultiRotationConstraint_0() => this.rotationConstraint;
    public MultiRotationConstraint GetMultiRotationConstraint_1() => this.rotationConstraint1;
    public MultiRotationConstraint GetMultiRotationConstraint_2() => this.rotationConstraint2;
}
