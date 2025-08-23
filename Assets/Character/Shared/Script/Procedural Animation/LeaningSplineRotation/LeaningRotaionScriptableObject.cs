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

    [Range(0, 2)]
    [SerializeField] public float checkDistance;
    [Range(0, 30)]
    [SerializeField] public int numberRaycast;
    [Range(0, 10)]
    [SerializeField] public float leaningSpeed;

    [Range(0, 100)]
    [SerializeField] public float distanceCheck;

    [SerializeField] public LayerMask castingCheckLayer;

    public AnimationCurve leanWeightCurve;
}
