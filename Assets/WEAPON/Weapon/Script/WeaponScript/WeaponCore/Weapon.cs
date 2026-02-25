using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public abstract partial class Weapon : WeaponSubject ,IObserverWeapon,IInitializedAble 
{

    public Transform _mainHandGripTransform;
    public Transform _SecondHandGripTransform;

    [SerializeField] public WeaponDataScriptableObject weaponStatsScriptableObject;
    public virtual int maxAmmoCapacity { get => weaponStatsScriptableObject.bulletCapacity + this.maxAmmoCapacityAdditional; }
    public int maxAmmoCapacityAdditional = 0;
    
    public virtual float rate_of_fire { get => weaponStatsScriptableObject.rate_of_fire + this.rate_of_fire_Additional; }
    public float rate_of_fire_Additional = 0;
    public virtual float reloadTime { get => weaponStatsScriptableObject.reloadTime + this.reloadTimeAdditional; }
    public float reloadTimeAdditional = 0;
    public virtual float Recovery_CrosshairBloomSpeed { get => weaponStatsScriptableObject.Recovery_CrosshairBloomSpeed + this.Recovery_CrosshairBloomSpeed_Additional; }
    public float Recovery_CrosshairBloomSpeed_Additional = 0;
    public virtual float Recovery_CrosshairPositionSpeed { get => weaponStatsScriptableObject.Recovery_CrosshairPositionSpeed + this.Recovery_CrosshairPositionSpeed_Additional; }
    public float Recovery_CrosshairPositionSpeed_Additional = 0;
    public virtual float Recoil_CrosshairBloomController { get => weaponStatsScriptableObject.Recoil_CrosshairBloomController + this.Recoil_CrosshairBloomController_Additional; }
    public float Recoil_CrosshairBloomController_Additional = 0;
    public virtual float Recoil_KickPositionCrosshairController { get => weaponStatsScriptableObject.Recoil_KickPositionPositionCrosshairController + this.Recoil_KickPositionCrosshairController_Additional; }
    public float Recoil_KickPositionCrosshairController_Additional = 0;
    public virtual float Recoil_CameraControlController { get => weaponStatsScriptableObject.Recoil_CameraControlController + this.Recoil_CameraControlController_Additional; }
    public float Recoil_CameraControlController_Additional = 0;
    public virtual float Recoil_VisualImpulseControl { get => weaponStatsScriptableObject.Recoil_VisualImpulseControl + this.Recoil_VisualImpulseControl_Additional; }
    public float Recoil_VisualImpulseControl_Additional = 0;
    public virtual float RecoilKickBack { get => 1; }
    public virtual float min_CrosshairSize { get => weaponStatsScriptableObject.min_CrosshairSize + this.min_CrosshairSize_Additional; }
    public float min_CrosshairSize_Additional = 0;
    public virtual float max_CrosshairSize { get => weaponStatsScriptableObject.max_CrosshairSize + this.max_CrosshairSize_Additional; }
    public float max_CrosshairSize_Additional = 0;
    public virtual float aimDownSight_speed { get => weaponStatsScriptableObject.aimDownSight_speed + this.aimDownSight_speed_Additional; }
    public float aimDownSight_speed_Additional = 0;

    public float Recoil_CrosshairBloom { get => RecoilKickBack - Recoil_CrosshairBloomController; }
    public float Recoil_CrosshairPosition { get => RecoilKickBack - Recoil_KickPositionCrosshairController; }
    public float Recoil_Camera { get => RecoilKickBack - Recoil_CameraControlController; }
    public float Recoil_VisualImpulse { get => RecoilKickBack - Recoil_VisualImpulseControl; }

    public abstract Bullet bullet { get;  set; }

    public bool isPullTrigger { get; protected set; }
    public bool isEquiped { get 
        {
           if(curAttatch == null)
                return false;

           if(curAttatch is MainHandSocket)
                return true;
           return false;
        } 
    }

    public abstract Chamber  chamber { get;protected set; }
    protected abstract BulletCapacity bulletCap { get; set; }
    public int curBulletCapacity { get => bulletCap != null ? this.bulletCap.curCount : 0; }


    public IWeaponAdvanceUser userWeapon { 
        get
        {
            if(curAttatch == null)
                return null;

            return this.curAttatch.weaponAdvanceUser;
        } }
    public WeaponSocket curAttatch { get; private set; }
    [SerializeField] private WeaponMountComponent WeaponAttacherComponent;
    public WeaponMountComponent _weaponAttacherComponent { get => WeaponAttacherComponent; protected set => WeaponAttacherComponent =value; }
    public Rigidbody rb;
    public BulletSpawner bulletSpawner;
    public enum FireMode
    {
        Single,
        Burst,
        FullAuto
    }
    public FireMode fireMode { get; protected set; }
    public TriggerState triggerState { get; protected set; }
    public LayerMask weaponLayerMask { get; private set; }
    public Vector3 shootingPosition 
    {
        get
        {
            if(this.userWeapon != null)
                return userWeapon._shootingPos;

            Ray ray = new Ray(this.bulletSpawner.transform.position, this.bulletSpawner.transform.forward);

            if(Physics.Raycast(ray,out RaycastHit hitInfo, 1000, 0, QueryTriggerInteraction.Ignore))
            {
                return hitInfo.point;
            }
            else
            {
                return ray.GetPoint(1000);
            }
        } 
    }

    public FiringNode fire;

    #region weaponAttachment
    public WeaponAttachmentSocket[] weaponAttachmentSocket;
    
    private void UpdateAdditionalStats()
    {
        this.maxAmmoCapacityAdditional = 0;
        this.rate_of_fire_Additional = 0;
        this.reloadTimeAdditional = 0;
        this.Recovery_CrosshairBloomSpeed_Additional = 0;
        this.Recovery_CrosshairPositionSpeed_Additional = 0;
        this.Recoil_CrosshairBloomController_Additional = 0;
        this.Recoil_KickPositionCrosshairController_Additional = 0;
        this.Recoil_CameraControlController_Additional = 0;
        this.Recoil_VisualImpulseControl_Additional = 0;

        this.min_CrosshairSize_Additional = 0;
        this.max_CrosshairSize_Additional = 0;
        this.aimDownSight_speed_Additional = 0;

        if(this.weaponAttachmentSocket == null
            || this.weaponAttachmentSocket.Length <= 0)
            return;

        for (int i = 0; i < this.weaponAttachmentSocket.Length; i++) 
        {
            WeaponAttachment weaponAttachment = this.weaponAttachmentSocket[i].curWeaponAttach;

            if (weaponAttachment == null)
                continue;

            this.maxAmmoCapacityAdditional += weaponAttachment.maxAmmoCapacityAdditional;
            this.rate_of_fire_Additional += weaponAttachment.rate_of_fire_Additional;
            this.reloadTimeAdditional += weaponAttachment.reloadTimeAdditional;
            this.Recovery_CrosshairBloomSpeed_Additional += weaponAttachment.Recovery_CrosshairBloomSpeed_Additional;
            this.Recovery_CrosshairPositionSpeed_Additional += weaponAttachment.Recovery_CrosshairPositionSpeed_Additional;
            this.Recoil_CrosshairBloomController_Additional += weaponAttachment.Recoil_CrosshairBloomController_Additional;
            this.Recoil_KickPositionCrosshairController_Additional += weaponAttachment.Recoil_KickPositionCrosshairController_Additional;
            this.Recoil_CameraControlController_Additional += weaponAttachment.Recoil_CameraControlController_Additional;
            this.Recoil_VisualImpulseControl_Additional += weaponAttachment.Recoil_VisualImpulseControl_Additional;

            this.min_CrosshairSize_Additional += weaponAttachment.min_CrosshairSize_Additional;
            this.max_CrosshairSize_Additional += weaponAttachment.max_CrosshairSize_Additional;
            this.aimDownSight_speed_Additional += weaponAttachment.aimDownSight_speed_Additional;
        }
    }
    #endregion

    public virtual void Initialized()
    {

        weaponLayerMask = gameObject.layer;

        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        triggerState = TriggerState.Up;
        this.SetDefaultAttribute();
        this.InitailizedNode();
        this.AddObserver(this);
    }
  
    protected virtual void Update()
    {

        //TriggerUpdate
        if (isPullTrigger)
            switch (triggerState)
            {
                case (TriggerState.Up):
                    triggerState = TriggerState.IsDown;
                    break;
                case (TriggerState.IsDown):
                    triggerState = TriggerState.Down;
                    break;
                default:
                    triggerState = TriggerState.Down;
                    break;
            }
        else
            triggerState = TriggerState.Up;

        this.UpdateNode();
        isPullTrigger = false;
        _isTriggerThrow = false;
    }
   
    protected virtual void FixedUpdate()
    {
        this.FixedUpdateNode();
    }
    protected virtual void LateUpdate()
    {
        this.UpdateAdditionalStats();
    }
    public virtual void PullTrigger() 
    {
        isPullTrigger = true;
    }

    protected virtual void OnEnable()
    {
        SetDefaultAttribute();
    }

    protected virtual void SetDefaultAttribute()
    {
        triggerState = TriggerState.Up;
    }

    private void OnValidate()
    {

        this.Collider = GetComponent<Collider>();   
    }

    public bool TryGetBulletCapacity(out BulletCapacity bulletCapacity)
    {
        bulletCapacity = this.bulletCap;
        if(bulletCapacity == null)
            return false;

        return true;
    }

    public void SetCurAttatchAble(WeaponSocket weaponAttachingAble)
    {
        this.curAttatch = weaponAttachingAble;

        if(this.curAttatch != null)
        {
            this.rb.isKinematic = true;
            this._collider.isTrigger = true;
        }
        else
        {
            this.rb.isKinematic = false;
            this._collider.isTrigger = false;
        }
    }

    public void OnNotify<T>(Weapon weapon, T weaponNotify)
    {
        
    }

}
