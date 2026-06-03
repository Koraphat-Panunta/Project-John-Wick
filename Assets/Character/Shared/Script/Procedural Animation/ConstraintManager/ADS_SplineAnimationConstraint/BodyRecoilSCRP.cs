using UnityEngine;

[CreateAssetMenu(fileName = "BodyRecoilSCRP", menuName = "ScriptableObjects/ConstrainObject/BodyRecoilSCRP")]
public class BodyRecoilSCRP : ScriptableObject
{
    public Vector3 additionalRotationOffset;
    public AnimationCurve recoilCurve;
    public float recoilDuration;
}
