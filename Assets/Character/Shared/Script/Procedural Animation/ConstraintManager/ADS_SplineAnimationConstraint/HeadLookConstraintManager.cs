using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadLookConstraintManager : MonoBehaviour, IConstraintManager
{
    [SerializeField] private float weight;

    [SerializeField] private MultiAimConstraint headMultiAimConstraint;
    private Vector3 headOffset;

    [SerializeField] private HeadLookConstrainScriptableObject headLookConstrainSCRP;

    public void SetWeight(float w, HeadLookConstrainScriptableObject headLookConstrainSCRP)
    {
        SetWeight(w);
        this.headLookConstrainSCRP = headLookConstrainSCRP;
    }
    public void SetWeight(float w)
    {
        this.weight = Mathf.Clamp01(w);
    }
    public float GetWeight() => this.weight;

    void Update()
    {
        if (this.weight <= 0)
            return;

        headMultiAimConstraint.data.offset = Vector3.MoveTowards(headOffset, headLookConstrainSCRP.offsetLook, headLookConstrainSCRP.offsetChangedRate * Time.deltaTime);
        

        headOffset = headMultiAimConstraint.data.offset;

        //spline.data.offset = headLookConstrainSCRP.offsetSpline;
        //spline1.data.offset = headLookConstrainSCRP.offsetSpline1;
        //spline2.data.offset = headLookConstrainSCRP.offsetSpline2;

        weight = Mathf.Clamp(weight, 0f, 1f);

        headMultiAimConstraint.weight = headLookConstrainSCRP.weight * this.weight;
    }
}
