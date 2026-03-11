using UnityEngine;

[CreateAssetMenu(fileName = "TwoBoneIK_ConstraintSCRP", menuName = "ScriptableObjects/ConstrainObject/TwoBoneIK_ConstraintSCRP/TwoBoneIK_ConstraintSCRP")]
public class TwoBoneIK_ConstraintSCRP : ScriptableObject
{
    public Vector3 positionOffset;
    public Vector3 rotationEulerOffset;

    public Vector3 hintPositionOffset;

    [SerializeField] public Vector3 rotateRefDirOffset;
    public float maxHorizontalHandAimDeg;
    public float maxVerticalHandAimDeg;
}
