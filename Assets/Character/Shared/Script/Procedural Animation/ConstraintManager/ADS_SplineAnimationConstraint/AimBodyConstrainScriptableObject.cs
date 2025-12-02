using UnityEngine;

[CreateAssetMenu(fileName = "AimBodyConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/AimBodyConstrainScriptableObject")]
public class AimBodyConstrainScriptableObject : ScriptableObject
{
    [Range(0, 1)]
    [SerializeField] public float weightSpline;
    public Vector3 offsetSpline;

    [Range(0, 1)]
    [SerializeField] public float weightSpline1;
    public Vector3 offsetSpline1;

    [Range(0, 1)]
    [SerializeField] public float weightSpline2;
    public Vector3 offsetSpline2;

    [Range(0, 500)]
    [SerializeField] public float offsetChangedRate;

}
