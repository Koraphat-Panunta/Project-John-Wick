
using System;
using UnityEngine;
using UnityEngine.Animations;

public abstract class WeaponAttachment : 
    MonoBehaviour
    , IPickupItem
    , I_Interactable
{
    [SerializeField] private AttachmentType _attachmentType;
    public AttachmentType attachmentType => _attachmentType;
    public bool isAttaching { get => this.weaponAttachmentSocket != null ? true : false ; }
    public WeaponAttachmentSocket weaponAttachmentSocket { get; set; }
    public abstract AttachmentDataScriptableObject attachmentDataScriptableObject { get; }

    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected BoxCollider _collider;

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

        this._rigidbody.isKinematic = true;
        this._collider.isTrigger = true;
    } 


    public virtual void DetatchFormSocket()
    {
        this.transform.SetParent(null);
        this.weaponAttachmentSocket = null;

        this._rigidbody.isKinematic = false;
        this._collider.isTrigger = false;
    }

    // --- IPickupItem -------------------------------------------------------

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

    private void OnValidate()
    {
        if (this._rigidbody == null)
            this._rigidbody = GetComponent<Rigidbody>();

        if (this._collider == null)
            this._collider = GetComponent<BoxCollider>();
    }

    Collider I_Interactable._collider { get => this._collider; set { } }
    public Transform _transform { get => this.transform; set { } }
    public bool isBeenInteractAble { get => true; set { } }

    public Action<I_Interactable> onDoInteract { get; set; }

    public void DoInteract(I_Interacter i_Interacter)
    {
        if (i_Interacter is IItemReceiver itemReceiver == false)
        {
            this.OnDoInteract();
            return;
        }

        if(this.CanBeReceivedBy(itemReceiver))
        {
            this.Apply(itemReceiver, this);
            this.OnDoInteract();
        }
      
    }

    private void OnDoInteract()
    {
        if(this.onDoInteract != null)
            this.onDoInteract.Invoke(this);
    }
}

public enum AttachmentType
{
    Optic,
    Muzzle,
    Grip,
    Laser,
}
