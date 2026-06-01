using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandRecoilSCRP", menuName = "ScriptableObjects/ConstrainObject/HandIK/WeaponHandRecoilSCRP")]
public class WeaponHandRecoilSCRP : ScriptableObject
{
    public Vector3 additionalPositionOffset;
    public Vector3 additionalRotationEulerOffset;

    public float positionRecoverySpeed = 4f;
    public float rotationRecoverySpeed = 10f;
}
