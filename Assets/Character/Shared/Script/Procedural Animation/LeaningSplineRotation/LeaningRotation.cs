using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeaningRotation : MonoBehaviour, IConstraintManager
{
    // Start is called before the first frame update
    [Range(0, 1)]
    public float weight;

    [Range(-1, 1)]
    [SerializeField] private float leaningLeftRightSpline;
    public Vector3 leaningLeftRightSplineOffset => leaningRotaionScriptable.leaningLeftRightSplineOffset;

    [Range(-1, 1)]
    [SerializeField] private float leaningLeftRightSpline1;
    public Vector3 leaningLeftRightSpline1Offset => leaningRotaionScriptable.leaningLeftRightSpline1Offset;

    [Range(0, 2)]
    [SerializeField] private float multiplyleaningLeftRightSpline1 => leaningRotaionScriptable.multiplyleaningLeftRightSpline1;

    [SerializeField] private MultiRotationConstraint rotationConstraintSpline;
    [SerializeField] private MultiRotationConstraint rotationConstraintSpline1;

    [SerializeField] private LeaningRotaionScriptableObject leaningRotaionScriptable;

    void Start()
    {

    }
    public void SetWeight(float weight, LeaningRotaionScriptableObject leaningRotaionScriptable)
    {
        SetWeight(weight);
        this.leaningRotaionScriptable = leaningRotaionScriptable;
    }
    public void SetWeight(float weight)
    {
        this.weight = weight;
    }
    public float GetWeight() => this.weight;
    public void SetLeaningLeftRight(float weight)
    {
        leaningLeftRightSpline = Mathf.Clamp(weight, -1, 1);
    }
    public float GetLeaningLeftRight()
    {
        return leaningLeftRightSpline;
    }
    // Update is called once per frame
    void Update()
    {
        weight = Mathf.Clamp01(weight);

        if(weight <= 0)
            return;

        leaningLeftRightSpline = Mathf.Clamp(leaningLeftRightSpline, -leaningRotaionScriptable.leaningLeftRightSplineMax, leaningRotaionScriptable.leaningLeftRightSplineMax);

        rotationConstraintSpline.weight = weight;
        rotationConstraintSpline1.weight = weight;

        float w = 1 - ((1 - leaningLeftRightSpline) / 2);

        WeightedTransformArray leanRef = rotationConstraintSpline.data.sourceObjects;
        leanRef.SetWeight(0, 1 - w);
        leanRef.SetWeight(1, w);

        rotationConstraintSpline.data.sourceObjects = leanRef;
        rotationConstraintSpline.data.offset = Vector3.Lerp(Vector3.zero, leaningRotaionScriptable.leaningLeftRightSplineOffset, weight);

        leaningLeftRightSpline1 = leaningLeftRightSpline * multiplyleaningLeftRightSpline1;
        w = 1 - ((1 - leaningLeftRightSpline1) / 2);

        leanRef.SetWeight(0, 1 - w);
        leanRef.SetWeight(1, w);

        rotationConstraintSpline1.data.sourceObjects = leanRef;
        rotationConstraintSpline1.data.offset = Vector3.Lerp(Vector3.zero, leaningRotaionScriptable.leaningLeftRightSpline1Offset, weight);
    }
}


