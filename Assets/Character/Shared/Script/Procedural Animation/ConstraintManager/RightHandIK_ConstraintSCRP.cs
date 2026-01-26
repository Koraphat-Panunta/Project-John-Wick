using UnityEngine;

[CreateAssetMenu(fileName = "RightHandIK_ConstraintSCRP", menuName = "ScriptableObjects/ConstrainObject/RightHandIK_ConstraintSCRP")]
public class RightHandIK_ConstraintSCRP : ScriptableObject
{
    public Vector3 positionOffset;
    public Vector3 rotationEulerOffset;

    public Vector3 hintPositionOffset;
}
