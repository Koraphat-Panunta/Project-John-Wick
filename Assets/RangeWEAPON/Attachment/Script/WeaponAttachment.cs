
using UnityEngine;
using UnityEngine.Animations;

public abstract class WeaponAttachment : MonoBehaviour, IPickupEffect
{
    [SerializeField] private AttachmentType _attachmentType;
    public AttachmentType attachmentType => _attachmentType;
    public bool isAttaching { get => this.weaponAttachmentSocket != null ? true : false ; }
    public WeaponAttachmentSocket weaponAttachmentSocket { get; set; }
    public abstract AttachmentDataScriptableObject attachmentDataScriptableObject { get; }

    public virtual int maxAmmoCapacityAdditional { get; }
    public virtual float rate_of_fire_Additional { get; }
    public virtual float reloadTimeAdditional { get; }
    public virtual float Recovery_CrosshairBloomSpeed_Additional { get; }
    public virtual float Recovery_CrosshairPositionSpeed_Additional { get; }
    public virtual float Recoil_CrosshairBloomController_Additional { get; }
    public virtual float Recoil_KickPositionCrosshairController_Additional { get; }
    public virtual float Recoil_CameraControlController_Additional { get; }
    public virtual float Recoil_VisualImpulseControl_Additional { get; }

    public virtual float min_CrosshairSize_Additional { get; }
    public virtual float max_CrosshairSize_Additional { get; }
    public virtual float aimDownSight_speed_Additional { get; }

    public virtual void SetToSocket(WeaponAttachmentSocket weaponAttachmentSocket)
    {
        if (this.isAttaching)
        {
            Debug.LogError("WeaponAttachment Conflict");
            return;
        }

        this.weaponAttachmentSocket = weaponAttachmentSocket;

        this.transform.SetParent(this.weaponAttachmentSocket.transform,false);

        this.transform.localPosition = this.attachmentDataScriptableObject.offsetAnchorAttachPos;
        this.transform.localRotation = Quaternion.Euler(this.attachmentDataScriptableObject.offsetAnchorAttachRot);
    } 


    public virtual void DetatchFormSocket()
    {
        this.transform.SetParent(null);
        this.weaponAttachmentSocket = null;
    }

    // --- IPickupEffect -------------------------------------------------------

    public bool CanBeReceivedBy(IItemReceiver receiver)
    {
        if (receiver is not IRangeWeaponAdvanceUser user) return false;
        var weapon = user._currentWeapon;
        if (weapon == null || weapon.weaponAttachmentSocket == null) return false;
        foreach (var socket in weapon.weaponAttachmentSocket)
            if (socket != null && socket.socketAttachmentType == attachmentType) return true;
        return false;
    }

    public void Apply(IItemReceiver receiver, object source)
    {
        if (receiver is not IRangeWeaponAdvanceUser user) return;
        var weapon = user._currentWeapon;
        if (weapon == null || weapon.weaponAttachmentSocket == null) return;

        WeaponAttachmentSocket targetSocket = null;
        foreach (var socket in weapon.weaponAttachmentSocket)
        {
            if (socket != null && socket.socketAttachmentType == attachmentType)
            {
                targetSocket = socket;
                break;
            }
        }

        if (targetSocket == null) return;

        if (targetSocket.curWeaponAttach != null)
            targetSocket.Detach();

        targetSocket.Attach(this);
    }
}

public enum AttachmentType
{
    Optic,
    Muzzle,
    Grip,
    Laser,
}
