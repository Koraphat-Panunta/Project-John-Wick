using UnityEngine;

[CreateAssetMenu(fileName = "MicroOpticWeaponAttachmentScriptableObject", menuName = "ScriptableObject/WeaponAttachment/MicroOpticWeaponAttachmentScriptableObject")]
public class MicroOpticWeaponAttachmentScriptableObject : ScriptableObject
{
    [Range(-50, 50)]
    public float min_Precision_PN;
    [Range(-50, 50)]
    public float max_Precision_PN;
    [Range(-100, 100)]
    public float accuracy_PN;
    [Range(-50, 0)]
    public float aimDownSightSpeed_N;
}
