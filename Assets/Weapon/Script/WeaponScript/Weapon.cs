using System.Collections;
using System.Collections.Generic;
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

    public bool isPullTrigger { get; protected set; }
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
    public TriggerState triggerState { get; protected set; }

    public LayerMask weaponLayerMask { get; private set; }
    protected virtual void Awake()
    {
        triggerState = TriggerState.Up;
        weaponLayerMask = gameObject.layer;
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
        WeaponUser._currentWeapon = this;
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
            WeaponAnimatorOverrider.OverrideAnimator(p.animator, _weaponOverrideControllerPlayer);
        }
        if(WeaponUser is Enemy)
        {
            Enemy enemy = WeaponUser as Enemy;
            WeaponAnimatorOverrider.OverrideAnimator(enemy.animator, _weaponOverrideControllerPlayer);
        }
        if(this is PrimaryWeapon){
            if (WeaponUser.weaponBelt.primaryWeapon == null)
                WeaponUser.weaponBelt.primaryWeapon = this as PrimaryWeapon;
        }
        else if(this is SecondaryWeapon) { 
            if(WeaponUser.weaponBelt.secondaryWeapon == null)
            WeaponUser.weaponBelt.secondaryWeapon = this as SecondaryWeapon;
        }
        parentConstraint.weight = 1;
    }
    public void AttatchWeaponToNoneOverrideAnimator(IWeaponAdvanceUser WeaponUser)
    {
        isEquiped = true;
        this.userWeapon = WeaponUser;
        WeaponUser._currentWeapon = this;
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
       
        if (this is PrimaryWeapon)
        {
            if (WeaponUser.weaponBelt.primaryWeapon == null)
                WeaponUser.weaponBelt.primaryWeapon = this as PrimaryWeapon;
        }
        else if (this is SecondaryWeapon)
        {
            if (WeaponUser.weaponBelt.secondaryWeapon == null)
                WeaponUser.weaponBelt.secondaryWeapon = this as SecondaryWeapon;
        }
        parentConstraint.weight = 1;
    }
    public void AttachWeaponToSocket(Transform weaponSocket)
    {
        isEquiped = false;
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
       
        if (userWeapon._currentWeapon == this)
        {
            if (userWeapon is Player)
            {
                Player p = userWeapon as Player;
                WeaponAnimatorOverrider.OverrideAnimator(p.animator, _weaponOverrideControllerPlayer);
            }
            if (userWeapon is Enemy)
            {
                Enemy enemy = userWeapon as Enemy;
                WeaponAnimatorOverrider.OverrideAnimator(enemy.animator, _weaponOverrideControllerPlayer);
            }
            userWeapon._currentWeapon = null;
        }
    }
    public void AttachWeaponToSocketNoneAnimatorOverride(Transform weaponSocket)
    {
        isEquiped = false;
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

        if (userWeapon._currentWeapon == this)
        {
            userWeapon._currentWeapon = null;
        }
    }
    public void AttachWeaponToSecondHand(Transform secondHandSocket)
    {
        isEquiped = false;
        //userWeapon = null;
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
        isEquiped = false ;
        rb.isKinematic = false;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
            parentConstraint.constraintActive = true;
            parentConstraint.constraintActive = true;
            parentConstraint.weight = 1;
        }
        if (userWeapon._currentWeapon == this )
        {
            if (userWeapon.userWeapon.isDead == false)
            {
                if (userWeapon is Player)
                {
                    Player p = userWeapon as Player;
                    WeaponAnimatorOverrider.OverrideAnimator(p.animator, _weaponOverrideControllerPlayer);
                }
                if (userWeapon is Enemy)
                {
                    Enemy enemy = userWeapon as Enemy;
                    WeaponAnimatorOverrider.OverrideAnimator(enemy.animator, _weaponOverrideControllerPlayer);
                }
            }
            userWeapon._currentWeapon = null;
        }
        if (this is PrimaryWeapon)
        {
            if(userWeapon.weaponBelt.primaryWeapon == this as PrimaryWeapon)
            userWeapon.weaponBelt.primaryWeapon = null;
        }
        else if (this is SecondaryWeapon)
        {
            if(userWeapon.weaponBelt.secondaryWeapon == this as SecondaryWeapon)
            userWeapon.weaponBelt.secondaryWeapon =null;
        }
        
        userWeapon = null;

    }

    #region InitailizedWeaponTree
   
    public WeaponLeafNode currentEventNode { get; set; }
    public abstract WeaponSelector startEventNode { get; set; }

    public abstract RestNode restNode { get; set; }
    protected virtual void FixedUpdateTree()
    {
        if (currentEventNode != null)
            currentEventNode.FixedUpdateNode();
    }
   
    public virtual void ChangeActionManualy(WeaponLeafNode weaponEventNode)
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
            //Debug.Log("curWeaponNode ="+ currentEventNode+" is reset ");
            currentEventNode.Exit();
            currentEventNode = null;
            startEventNode.FindingNode(out INodeLeaf weaponActionNode);
            currentEventNode = weaponActionNode as WeaponLeafNode;
            //Debug.Log("Out Event Node " + currentEventNode);
            currentEventNode.Enter();
        }
        currentEventNode?.UpdateNode();
        //Debug.Log("curWeaponNode = " + currentEventNode);
    }
    #endregion

    public void OnNotify(Weapon weapon, WeaponNotifyType weaponNotify)
    {
        
    }
}
