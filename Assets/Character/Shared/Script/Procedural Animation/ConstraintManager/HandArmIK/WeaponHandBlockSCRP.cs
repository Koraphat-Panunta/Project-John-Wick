using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandBlockSCRP", menuName = "ScriptableObjects/ConstrainObject/HandIK/WeaponHandBlockSCRP")]
public class WeaponHandBlockSCRP : ScriptableObject
{
    public Vector3 positionOffset;
    public Vector3 rotationEulerOffset;
    public Vector3 hintPositionOffset;

    public float raiseSpeed = 2f;
    public float lowerSpeed = 5f;
    public float smoothingSpeed = 80f;
    public float clearBuffer = .15f;

    public LayerMask blockMask;
}
