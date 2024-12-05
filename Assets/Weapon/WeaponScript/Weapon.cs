using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Weapon : WeaponSubject ,IObserverWeapon
{
    //protected abstract WeaponTreeManager weaponTree { get; set; }
    [SerializeField] public Muzzle muzzle;
    [SerializeField] public Sight Sight;

    public Transform bulletSpawnerPos;
    public abstract int Magazine_capacity { get; set; }
    public abstract float rate_of_fire { get;  set; }
    public abstract float reloadSpeed { get;  set; }
    public abstract float Accuracy { get;  set; }
    public abstract float RecoilController { get;  set; }
    public abstract float RecoilCameraController {  get;  set; }
    public abstract float RecoilKickBack { get;  set; }
    public abstract float min_Precision { get;  set; }
    public abstract float max_Precision { get;  set; }
    public abstract float aimDownSight_speed { get;  set; }
    public abstract Bullet bullet { get;  set; }
    public abstract float movementSpeed { get;  set; }

    public bool isAiming;
    public bool isReloadCommand;
    public bool isCancelAction;
    public bool isEquip { get { return userWeapon != null; }}

    public float aimingWeight;

    public Dictionary<BulletStackType,int> bulletStore = new Dictionary<BulletStackType,int>();
    public Dictionary<AttachmentSlot,Transform> weaponSlotPos = new Dictionary<AttachmentSlot, Transform>();
    public Dictionary<AttachmentSlot,WeaponAttachment> attachment = new Dictionary<AttachmentSlot,WeaponAttachment>();

    public IWeaponAdvanceUser userWeapon;
    public ParentConstraint parentConstraint;
    public Rigidbody rb;
    public AnimatorOverrideController _weaponOverrideControllerPlayer;
    public AnimatorOverrideController _weaponOverrideControllerEnemy;
    public BulletSpawner bulletSpawner;
    public enum FireMode
    {
        Single,
        //Burst,
        FullAuto
    }
    public FireMode fireMode { get; protected set; }
   
    public TriggerState triggerState = TriggerState.Up;

    public WeaponNode currentNode { get; set; }
    public abstract WeaponSelector startNode { get; set; }

    protected virtual void Start()
    {
        InitailizedTree();
        this.AddObserver(this);
        currentNode = startNode;
        parentConstraint = GetComponent<ParentConstraint>();
        rb = GetComponent<Rigidbody>();
        bulletStore.Add(BulletStackType.Chamber, 1);

    }
    protected virtual void Update()
    {
        UpdateTree();
        isCancelAction = false;
    }
    protected virtual void FixedUpdate()
    {
        FixedUpdateTree();
    }
    public virtual void Aim()
    {
        isAiming = true;
    }
    public virtual void Fire() 
    {
    }
    public virtual void Reload() 
    {
        isReloadCommand = true;
    }
    public virtual void LowWeapon()
    {
        isAiming = false;
    }
    public void AttatchWeaponTo(IWeaponAdvanceUser WeaponUser)
    {
        this.userWeapon = WeaponUser;
        WeaponUser.currentWeapon = this;
        rb.isKinematic = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = WeaponUser.currentWeaponSocket;
        source.weight = 1;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.AddSource(source);
        parentConstraint.constraintActive = true;
        parentConstraint.translationAtRest = Vector3.zero;
        parentConstraint.rotationAtRest = Vector3.zero;
        parentConstraint.constraintActive = true;
        if(WeaponUser is Player)
        {
            Player p = WeaponUser as Player;
            p.animator.runtimeAnimatorController = _weaponOverrideControllerPlayer;
        }
        if(WeaponUser is Enemy)
        {
            Enemy enemy = WeaponUser as Enemy;
            enemy.animator.runtimeAnimatorController = _weaponOverrideControllerEnemy;
        }
        if(this is PrimaryWeapon){
            WeaponUser.weaponBelt.primaryWeapon = this as PrimaryWeapon;
        }
        else if(this is SecondaryWeapon) { 
            WeaponUser.weaponBelt.secondaryWeapon = this as SecondaryWeapon;
        }
        parentConstraint.weight = 1;
    }
    public void AttachWeaponTo(Transform weaponSocket)
    {
        userWeapon = null;
        rb.isKinematic = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = weaponSocket;
        source.weight = 1;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.AddSource(source);
        parentConstraint.constraintActive = true;
        parentConstraint.translationAtRest = Vector3.zero;
        parentConstraint.rotationAtRest = Vector3.zero;
        parentConstraint.constraintActive = true;
        
        parentConstraint.weight = 1;
    }
    public void DropWeapon()
    {
        rb.isKinematic = false;
    }
    protected virtual void FixedUpdateTree()
    {

        if (currentNode != null)
            currentNode.FixedUpdate();
    }
    protected virtual void ChangeTreeManualy(WeaponActionNode weaponActionNode)
    {
        (currentNode as WeaponActionNode).Exit();
        currentNode = weaponActionNode;
        (currentNode as WeaponActionNode).Enter();
    }

    protected abstract void InitailizedTree();


    protected virtual void UpdateTree()
    {
        if (currentNode.IsReset())
        {
            if (currentNode is WeaponActionNode)
                (currentNode as WeaponActionNode).Exit();
            currentNode = startNode;
            currentNode.Transition(out WeaponActionNode weaponActionNode);
            currentNode = weaponActionNode;
            Debug.Log("Out Node " + currentNode);
            (currentNode as WeaponActionNode).Enter();
        }
        currentNode.Update();
    }
    public void OnNotify(Weapon weapon, WeaponNotifyType weaponNotify)
    {
        
    }
}
