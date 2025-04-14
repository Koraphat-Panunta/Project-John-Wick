using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SplineLookConstrain : MonoBehaviour
{
    [SerializeField] private float weight;

    [SerializeField] private MultiAimConstraint spline;
    [SerializeField] private MultiAimConstraint spline1;
    [SerializeField] private MultiAimConstraint spline2;

    private Vector3 splineOffset;
    private Vector3 spline1Offset;
    private Vector3 spline2Offset;

    [SerializeField] private AimSplineLookConstrainScriptableObject aimDownSightSplineConstrainScriptableObject;

    // Update is called once per frame
    public void SetWeight(float w, AimSplineLookConstrainScriptableObject aimDownSightSplineConstrainScriptableObject) 
    {
        SetWeight(w);
        this.aimDownSightSplineConstrainScriptableObject = aimDownSightSplineConstrainScriptableObject;
    }
    public void SetWeight(float w)
    {
        this.weight = Mathf.Clamp01(w);
    }
    public float GetWeight() => this.weight;

    void Update()
    {
        if(this.weight <= 0)
            return;

        spline.data.offset = Vector3.MoveTowards(splineOffset, aimDownSightSplineConstrainScriptableObject.offsetSpline , aimDownSightSplineConstrainScriptableObject.offsetChangedRate * Time.deltaTime); 
        spline1.data.offset = Vector3.MoveTowards(spline1Offset, aimDownSightSplineConstrainScriptableObject.offsetSpline1, aimDownSightSplineConstrainScriptableObject.offsetChangedRate * Time.deltaTime);
        spline2.data.offset = Vector3.MoveTowards(spline2Offset, aimDownSightSplineConstrainScriptableObject.offsetSpline2, aimDownSightSplineConstrainScriptableObject.offsetChangedRate * Time.deltaTime);

        splineOffset = spline.data.offset;
        spline1Offset = spline1.data.offset;
        spline2Offset = spline2.data.offset;

        //spline.data.offset = aimDownSightSplineConstrainScriptableObject.offsetSpline;
        //spline1.data.offset = aimDownSightSplineConstrainScriptableObject.offsetSpline1;
        //spline2.data.offset = aimDownSightSplineConstrainScriptableObject.offsetSpline2;

        weight = Mathf.Clamp(weight, 0f, 1f);

        spline.weight = aimDownSightSplineConstrainScriptableObject.weightSpline * this.weight;
        spline1.weight = aimDownSightSplineConstrainScriptableObject.weightSpline1 * this.weight;
        spline2.weight = aimDownSightSplineConstrainScriptableObject.weightSpline2 * this.weight;
    }
}
