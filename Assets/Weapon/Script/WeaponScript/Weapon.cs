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

    public Transform bulletSpawnerPos;

    public abstract Transform gripPos { get; set; }
    public abstract Transform SecondHandgripPos { get; set; }

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
    public abstract float drawSpeed { get; set; }

    public bool isPullTrigger;
    public bool isReloadCommand;
    public bool isEquiped;


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


    protected virtual void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
        rb = GetComponent<Rigidbody>();
        bulletStore.Add(BulletStackType.Chamber, 1);
        InitailizedTree();

    }
    protected virtual void Start()
    {
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

        UpdateTree();
    }
    protected virtual void LateUpdate()
    {
        isPullTrigger = false;
        isReloadCommand = false;
    }
    protected virtual void FixedUpdate()
    {
        FixedUpdateTree();
    }
   
    public virtual void PullTrigger() 
    {
        isPullTrigger = true;
    }
    public virtual void Reload() 
    {
        isReloadCommand = true;
    }
    
    public void AttatchWeaponTo(IWeaponAdvanceUser WeaponUser)
    {
        isEquiped = true;
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
        isEquiped = false;
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
    public void AttachWeaponToSecondHand(Transform secondHandSocket)
    {
        isEquiped = false;
        userWeapon = null;
        rb.isKinematic = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = secondHandSocket;
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

    #region InitailizedWeaponTree
   
    public WeaponActionNode currentEventNode { get; set; }
    public abstract WeaponSelector startEventNode { get; set; }

    public abstract RestNode restNode { get; set; }
    protected virtual void FixedUpdateTree()
    {
        if (currentEventNode != null)
            currentEventNode.FixedUpdate();
    }
   
    public virtual void ChangeActionManualy(WeaponActionNode weaponEventNode)
    {
        if (currentEventNode != null)
        currentEventNode.Exit();
        currentEventNode = weaponEventNode;
        currentEventNode.Enter();
    }

    protected abstract void InitailizedTree();
    protected virtual void UpdateTree()
    {
       
        if (currentEventNode.IsReset() /*|| currentStanceNode == null*/)
        {
            currentEventNode.Exit();
            currentEventNode = null;
            startEventNode.Transition(out WeaponActionNode weaponActionNode);
            currentEventNode = weaponActionNode;
            //Debug.Log("Out Event Node " + currentEventNode);
            currentEventNode.Enter();
        }
        currentEventNode?.Update();
    }
    #endregion

    public void OnNotify(Weapon weapon, WeaponNotifyType weaponNotify)
    {
        
    }
}
