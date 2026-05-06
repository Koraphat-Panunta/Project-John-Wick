using UnityEngine;

public class WeaponAttachmentSocket : MonoBehaviour
{

    public AttachmentType socketAttachmentType;
    public WeaponAttachment curWeaponAttach;

    public RangeWeapon weapon;

    public Transform socketTransform => this.transform;

    public void Attach(WeaponAttachment weaponAttachment)
    {
        if(weaponAttachment.attachmentType != this.socketAttachmentType)
        {
            Debug.LogWarning("not same type weaponAttachment socket");
            return;
        }

        this.curWeaponAttach = weaponAttachment;
        this.curWeaponAttach.SetToSocket(this);
    }

    public void Detach()
    {
        this.curWeaponAttach.DetatchFormSocket();
        this.curWeaponAttach = null;
    }

    private void OnValidate()
    {
        if(this.curWeaponAttach != null
            && this.curWeaponAttach.isAttaching == false)
            this.Attach(this.curWeaponAttach);
    }
}
