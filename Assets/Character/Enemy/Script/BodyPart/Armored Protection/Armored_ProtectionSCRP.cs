using UnityEngine;

[CreateAssetMenu(fileName = "Armored_ProtectionSCRP", menuName = "ScriptableObjects/BodyPartDamageRecivedSCRP/Armored_ProtectionSCRP")]
public class Armored_ProtectionSCRP : BodyPartDamageRecivedSCRP
{
    [Range(0,500)]
    public float armorHP;


}
