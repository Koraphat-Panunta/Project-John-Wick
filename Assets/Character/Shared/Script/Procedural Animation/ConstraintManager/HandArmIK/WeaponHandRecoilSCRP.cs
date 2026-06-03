using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandRecoilSCRP", menuName = "ScriptableObjects/ConstrainObject/HandIK/WeaponHandRecoilSCRP")]
public class WeaponHandRecoilSCRP : ScriptableObject
{
    public Vector3 additionalPositionOffset;
    public Vector3 additionalRotationEulerOffset;

    public AnimationCurve positionRecoilCurve;
    public AnimationCurve rotationRecoilCurve;

    public float positionRecoilDurattion;
    public float rotationRecoilDurattion;
}
