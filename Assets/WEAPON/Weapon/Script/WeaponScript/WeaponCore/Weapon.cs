using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract partial class Weapon : WeaponSubject ,IObserverWeapon,IInitializedAble
{

    public Transform _mainHandGripTransform;
    public Transform _SecondHandGripTransform;
    public abstract WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject { get; set; }
    [SerializeField] protected WeaponStatsScriptableObject weaponStatsScriptableObject;
    public virtual int bulletCapacity { get => weaponStatsScriptableObject.bulletCapacity; }
    public virtual float rate_of_fire { get => weaponStatsScriptableObject.rate_of_fire; }
    public virtual float reloadTime { get => weaponStatsScriptableObject.reloadTime; }
    public virtual float Recovery_CrosshairBloomSpeed { get => weaponStatsScriptableObject.Recovery_CrosshairBloomSpeed; }
    public virtual float Recovery_CrosshairPositionSpeed { get => weaponStatsScriptableObject.Recovery_CrosshairPositionSpeed; }
    public virtual float Recoil_CrosshairBloomController { get => weaponStatsScriptableObject.Recoil_CrosshairBloomController; }
    public virtual float Recoil_KickPositionCrosshairController { get => weaponStatsScriptableObject.Recoil_KickPositionPositionCrosshairController; }
    public virtual float Recoil_CameraControlController { get => weaponStatsScriptableObject.Recoil_CameraControlController; }
    public virtual float Recoil_VisualImpulseControl { get => weaponStatsScriptableObject.Recoil_VisualImpulseControl; }
    public virtual float RecoilKickBack { get => 1; }
    public virtual float min_CrosshairSize { get => weaponStatsScriptableObject.min_CrosshairSize; }
    public virtual float max_CrosshairSize { get => weaponStatsScriptableObject.max_CrosshairSize; }
    public virtual float aimDownSight_speed { get => weaponStatsScriptableObject.aimDownSight_speed; }

    public float Recoil_CrosshairBloom { get => RecoilKickBack - Recoil_CrosshairBloomController; }
    public float Recoil_CrosshairPosition { get => RecoilKickBack - Recoil_KickPositionCrosshairController; }
    public float Recoil_Camera { get => RecoilKickBack - Recoil_CameraControlController; }
    public float Recoil_VisualImpulse { get => RecoilKickBack - Recoil_VisualImpulseControl; }

    public abstract Bullet bullet { get;  set; }

    public bool isPullTrigger { get; protected set; }
    public bool isEquiped;


    public Dictionary<BulletStackType,int> bulletStore = new Dictionary<BulletStackType,int>();

    public IWeaponAdvanceUser userWeapon;
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
    }
   
    protected virtual void FixedUpdate()
    {
        this.FixedUpdateNode();
    }
    protected virtual void LateUpdate()
    {
        _isTriggerThrow = false;
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

    public void OnNotify(Weapon weapon, WeaponNotifyType weaponNotify)
    {
        
    }

}
