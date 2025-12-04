using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BodyLookConstrain : MonoBehaviour, IConstraintManager
{
    [SerializeField] private float weight;

    [SerializeField] private MultiAimConstraint spline;
    [SerializeField] private MultiAimConstraint spline1;
    [SerializeField] private MultiAimConstraint spline2;

    [SerializeField] public Transform bodyLookAtPosition;

    public Vector3 getOffsetSpline => this.spline.data.offset;
    public Vector3 getOffsetSpline1 => this.spline1.data.offset;
    public Vector3 getOffsetSpline2 => this.spline2.data.offset;
    public void SetWeight(float w)
    {
        this.weight = Mathf.Clamp01(w);
        this.UpdateSplineWeight();
    }
    public float GetWeight() => this.weight;

    public void SetLookAtPosition(Vector3 lookAtPosition) => this.bodyLookAtPosition.position = lookAtPosition;

    public void SetAllSplineOffsetData(Vector3 offsetSpline,Vector3 offsetSpline1,Vector3 offsetSpline2)
    {
        this.SetSplineOffsetData(offsetSpline);
        this.SetSpline1OffsetData(offsetSpline1);
        this.SetSpline2OffsetData(offsetSpline2);
    }
    public void SetSplineOffsetData(Vector3 offset) => this.spline.data.offset = offset;
    public void SetSpline1OffsetData(Vector3 offset) => this.spline1.data.offset = offset;
    public void SetSpline2OffsetData(Vector3 offset) => this.spline2.data.offset = offset;

    public void SetAllSplineWeight(
        float weight
        ,float weight1
        ,float weight2)
    {
        this.SetSplineWeight(weight);
        this.SetSpline1Weight(weight1);
        this.SetSpline2Weight(weight2);
    }
    public void SetSplineWeight(float weight) 
    {
        this.splineWeight = weight; 
        this.UpdateSplineWeight();
    }
    public void SetSpline1Weight(float weight)
    {
        this.spline1Weight = weight;
        this.UpdateSplineWeight();
    }
    public void SetSpline2Weight(float weight) 
    {
        this.spline2Weight = weight;
        this.UpdateSplineWeight();
    }
    private void UpdateSplineWeight()
    {
        this.spline.weight = this.splineWeight * this.weight;
        this.spline1.weight = this.spline1Weight * this.weight;
        this.spline2.weight = this.spline2Weight * this.weight;
    }

    public float splineWeight { get; protected set; }
    public float spline1Weight { get; protected set; }
    public float spline2Weight { get; protected set; }

    
}
