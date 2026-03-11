using UnityEngine;

[CreateAssetMenu(fileName = "LegsIKConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/TwoBoneIK_ConstraintSCRP/LegsIKConstrainScriptableObject/LegsIKConstrainScriptableObject")]
public class LegsIKConstrainScriptableObject : ScriptableObject
{
    public Vector3 leftLegPositionOffset;
    public Vector3 leftLegRotationEulerOffset;

    public Vector3 leftLegHintPositionOffset;

    public Vector3 rightLegPositionOffset;
    public Vector3 rightLegRotationEulerOffset;

    public Vector3 rightLegHintPositionOffset;

    [SerializeField] public Vector3 rotateRefDirOffset;
    public float maxHorizontalHandAimDeg;
    public float maxVerticalHandAimDeg;
}
