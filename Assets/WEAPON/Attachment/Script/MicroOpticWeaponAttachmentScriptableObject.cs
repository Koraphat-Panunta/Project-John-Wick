using UnityEngine;

[CreateAssetMenu(fileName = "MicroOpticWeaponAttachmentScriptableObject", menuName = "ScriptableObjects/WeaponAttachment/MicroOpticWeaponAttachmentScriptableObject")]
public class MicroOpticWeaponAttachmentScriptableObject : AttachmentDataScriptableObject
{
    [Range(-50, 50)]
    public float min_Precision_PN;
    [Range(-50, 50)]
    public float max_Precision_PN;
    [Range(-50, 0)]
    public float aimDownSightSpeed_N;
}
