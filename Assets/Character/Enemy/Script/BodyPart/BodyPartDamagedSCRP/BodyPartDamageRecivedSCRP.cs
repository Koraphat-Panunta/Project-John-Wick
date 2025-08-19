using UnityEngine;

[CreateAssetMenu(fileName = "BodyPartDamageRecivedSCRP", menuName = "ScriptableObjects/BodyPartDamageRecivedSCRP/BodyPartDamageRecivedSCRP")]
public class BodyPartDamageRecivedSCRP : ScriptableObject
{
    [Range(0, 10)]
    public float _hpReciverMultiplyRate;
    [Range(0, 10)]
    public float _postureReciverRate;
    [Range(0, 10)]
    public float _staggerReciverRate;
}
