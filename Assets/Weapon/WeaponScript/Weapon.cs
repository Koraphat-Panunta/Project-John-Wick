using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Weapon : WeaponSubject ,IObserverWeapon
{
    //protected abstract WeaponTreeManager weaponTree { get; set; }
    public Muzzle muzzle;
    public Sight Sight;

    [SerializeField] protected List<WeaponAttachment> weaponAttachments = new List<WeaponAttachment>();

    public Transform bulletSpawnerPos;
    public abstract int bulletCapacity { get; set; }
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
    public bool isEquip;

    public float aimingWeight;

    public Dictionary<BulletStackType,int> bulletStore = new Dictionary<BulletStackType,int>();
    public Dictionary<AttachmentSlot,Transform> weaponSlotPos = new Dictionary<AttachmentSlot, Transform>();

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

    public  WeaponActionNode currentStanceNode { get; set; }
    public  WeaponActionNode currentEventNode { get; set; }
    public abstract WeaponSelector startStanceNode { get; set; }
    public abstract WeaponSelector startEventNode { get; set; }

    protected virtual void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
        rb = GetComponent<Rigidbody>();
        bulletStore.Add(BulletStackType.Chamber, 1);
        InitailizedTree();

        weaponAttachments.Add(muzzle);
        weaponAttachments.Add(Sight);
    }
    protected virtual void Start()
    {
        this.AddObserver(this);
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
        isEquip = true;
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
        isEquip = false;
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
        if(currentStanceNode != null)
            currentStanceNode.FixedUpdate();
        if (currentEventNode != null)
            currentEventNode.FixedUpdate();

    }
    protected virtual void ChangeStanceManualy(WeaponActionNode weaponStanceNode)
    {
        if (currentStanceNode != null)
        currentStanceNode.Exit();
        currentStanceNode = weaponStanceNode;
        currentStanceNode.Enter();
    }
    protected virtual void ChangeActionManualy(WeaponActionNode weaponEventNode)
    {
        if (currentEventNode != null)
        currentEventNode.Exit();
        currentEventNode = weaponEventNode;
        currentEventNode.Enter();
    }

    protected abstract void InitailizedTree();


    protected virtual void UpdateTree()
    {
        if (currentStanceNode.IsReset() /*|| currentStanceNode==null*/)
        {
            currentStanceNode.Exit();
            currentStanceNode = null;
            startStanceNode.Transition(out WeaponActionNode weaponActionNode);
            currentStanceNode = weaponActionNode;
            //Debug.Log("Out stance Node " + currentStanceNode);
            currentStanceNode.Enter();
        }
        if (currentEventNode.IsReset() /*|| currentStanceNode == null*/)
        {
            currentEventNode.Exit();
            currentEventNode = null;
            startEventNode.Transition(out WeaponActionNode weaponActionNode);
            currentEventNode = weaponActionNode;
            //Debug.Log("Out Event Node " + currentEventNode);
            currentEventNode.Enter();
        }
        currentStanceNode.Update();
        currentEventNode?.Update();
    }
    public void OnNotify(Weapon weapon, WeaponNotifyType weaponNotify)
    {
        
    }
}
