using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Weapon : WeaponSubject ,IObserverWeapon
{

    public Transform bulletSpawnerPos;
    public abstract Transform mainHandGripTransform { get; set; }
    public abstract Transform SecondHandGripTransform { get; set; }
    public WeaponAnimationStateOverrideScriptableObject weaponAnimationStateOverrideScriptableObject { get; set; }
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
    public bool isEquiped;


    public Dictionary<BulletStackType,int> bulletStore = new Dictionary<BulletStackType,int>();

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
    }
   
    protected virtual void FixedUpdate()
    {
        FixedUpdateTree();
    }
   
    public virtual void PullTrigger() 
    {
        isPullTrigger = true;
    }

    #region InitailizedWeaponTree
   
    public WeaponLeafNode currentEventNode { get; set; }
    public abstract WeaponSelector startEventNode { get; set; }

    public abstract RestNode restNode { get; set; }
    public abstract NodeSelector _reloadSelecotrOverriden { get; }
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
