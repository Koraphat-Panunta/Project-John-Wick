using UnityEngine;

[CreateAssetMenu(fileName = "LeaningRotationConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/RotationConstrain")]
public class LeaningRotaionScriptableObject : ScriptableObject
{
    [Range(0, 2)]
    public float checkDistance;
    [Range(0, 30)]
    public int numberRaycast;

    [Range(0, 100)]
    public float maxDistanceCheck;
    [Range(0, 100)]
    public float minDistanceCheck;

    public LayerMask castingCheckLayer;
    public AnimationCurve leanWeightCurve;

    public Vector3 spine0LeanOffset;
    public Vector3 spine1LeanOffset;
    public Vector3 spine2LeanOffset;

    [Range(0, 1)]
    public float leanSmoothTime;
    [Range(0, 1)]
    public float recoverySmoothTime;
}
