using UnityEngine;
using UnityEngine.Animations;

public class OpticWeaponAttachment : WeaponAttachment
{
    public override float min_CrosshairSize_Additional { get => this.microOpticWeaponAttachmentScriptableObject.min_Precision_PN; }
    public override float max_CrosshairSize_Additional { get => this.microOpticWeaponAttachmentScriptableObject.max_Precision_PN; }
    public override float aimDownSight_speed_Additional { get => this.microOpticWeaponAttachmentScriptableObject.aimDownSightSpeed_N; }

    public override AttachmentDataScriptableObject attachmentDataScriptableObject { get => this.microOpticWeaponAttachmentScriptableObject as AttachmentDataScriptableObject; }

    public override void SetToSocket(WeaponAttachmentSocket weaponAttachmentSocket)
    {


        base.SetToSocket(weaponAttachmentSocket);
    }

    public override void DetatchFormSocket()
    {


        base.DetatchFormSocket();
    }

    [SerializeField] protected MicroOpticWeaponAttachmentScriptableObject microOpticWeaponAttachmentScriptableObject;

  
}


