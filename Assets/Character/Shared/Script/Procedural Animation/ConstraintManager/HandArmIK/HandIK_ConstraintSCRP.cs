using UnityEngine;

[CreateAssetMenu(fileName = "HandIK_ConstraintSCRP", menuName = "ScriptableObjects/ConstrainObject/HandIK_ConstraintSCRP/HandIK_ConstraintSCRP")]
public class HandIK_ConstraintSCRP : ScriptableObject
{
    public Vector3 positionOffset;
    public Vector3 rotationEulerOffset;

    public Vector3 hintPositionOffset;

    public float maxHorizontalHandAimDeg;
    public float maxVerticalHandAimDeg;
}
