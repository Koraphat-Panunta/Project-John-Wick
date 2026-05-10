using UnityEngine;

[CreateAssetMenu(fileName = "BodyRotationConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/BodyRotationConstrainScriptableObject")]
public class BodyRotationConstrainScriptableObject : ScriptableObject
{
    [Range(0, 1)]
    public float weightConstraint;
    public Vector3 offsetConstraint;

    [Range(0, 1)]
    public float weightConstraint1;
    public Vector3 offsetConstraint1;

    [Range(0, 1)]
    public float weightConstraint2;
    public Vector3 offsetConstraint2;

    public Vector3 rotateRefDirOffset;

    [Range(0, 180)]
    public float maxHorizontalDeg;
    [Range(0, 180)]
    public float maxVerticalDeg;

    [Range(0, 500)]
    public float offsetChangedRate;
}
