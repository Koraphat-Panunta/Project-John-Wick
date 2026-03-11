using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandIK_ConstraintSCRP", menuName = "ScriptableObjects/ConstrainObject/TwoBoneIK_ConstraintSCRP/WeaponHandIK_ConstraintSCRP")]
public class WeaponHandIK_ConstraintSCRP : TwoBoneIK_ConstraintSCRP
{
    public Vector3 onBlocked_positionOffset;
    public Vector3 onBlocked_rotationEulerOffset;

    public Vector3 onBlocked_hintPositionOffset;

    public Vector3 recoil_Additional_Position_Offset;
    public Vector3 recoil_Additional_Rotation_Offset;
}
