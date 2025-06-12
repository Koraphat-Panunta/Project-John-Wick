using UnityEngine;

[CreateAssetMenu(fileName = "LeaningRotationConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/RotationConstrain")]
public class LeaningRotaionScriptableObject : ScriptableObject
{
    [Range(0, 1)]
    [SerializeField] public float leaningLeftRightSplineMax;
    public Vector3 leaningLeftRightSplineOffset;

    public Vector3 leaningLeftRightSpline1Offset;

    [Range(0, 2)]
    [SerializeField] public float multiplyleaningLeftRightSpline1;

    [Range(0, 100)]
    [SerializeField] public float weightAdd;

    [Range(0, 100)]
    [SerializeField] public float recoveryStepCheck;

    [Range(0, 100)]
    [SerializeField] public float distanceCheck;

    [SerializeField] public LayerMask castingCheckLayer;

}
