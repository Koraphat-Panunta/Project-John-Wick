using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser, IMotionDriven,
    ICombatOffensiveInstinct, IFindingTarget, ICoverUseable,
    IHearingComponent, IPatrolComponent,
    IPainStateAble,IFallDownGetUpAble,IGunFuGotAttackedAble,
    IFriendlyFirePreventing,IThrowAbleObjectVisitable
{
    [Range(0,100)]
    public float intelligent;
    [Range(0, 100)]
    public float strength;

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public MultiRotationConstraint rotationConstraint;

    public LayerMask targetMask;
    public FieldOfView enemyFieldOfView;


    public EnemyGetShootDirection enemyGetShootDirection;
    public EnemyComunicate enemyComunicate;
    public IMovementCompoent enemyMovement;
    public EnemyStateManagerNode enemyStateManagerNode;


    public IBulletDamageAble bulletDamageAbleBodyPartBehavior { get; set; }
    public Vector3 forceSave;

    public readonly float maxCost = 100;
    public readonly float lowestCost = 0;
    public float cost;
    public float myHP;
    private float posture;


    [SerializeField] public bool isImortal;
    public Transform rayCastPos;


    protected override void Awake()
    {
        base.Awake();

        enemyFieldOfView = new FieldOfView(120, 225, rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        enemyComunicate = new EnemyComunicate(this);

        enemyMovement = new EnemyMovement(agent, this);

        MotionControlInitailized();

        Initialized_IWeaponAdvanceUser();
        InitailizedCombatOffensiveInstinct();
        InitailizedFindingTarget();
        InitailizedCoverUsable();
        InitailizedHearingComponent();
        friendlyFirePreventingBehavior = new FriendlyFirePreventingBehavior(this);

        new WeaponFactorySTI9mm().CreateWeapon(this);
        cost = Random.Range(50, 70);
        posture = 100;

        base.HP = 100;
        base.maxHp = 100;
        enemyStateManagerNode = new EnemyStateManagerNode(this);
    }
    

    void Update()
    {
        myHP = base.HP;
        findingTargetComponent.FindTarget(out GameObject target);
        combatOffensiveInstinct.UpdateSening();

        enemyStateManagerNode.UpdateNode();
        weaponManuverManager.UpdateNode();
        enemyMovement.MovementUpdate();

        BlackBoardUpdate();
        BlackBoardBufferUpdate();

    }
   
    private void FixedUpdate()
    {
        enemyStateManagerNode.FixedUpdateNode();
        weaponManuverManager.FixedUpdateNode();
        enemyMovement.MovementFixedUpdate();
    }
   
    public void TakeDamage(float Damage)
    {
        if(isImortal == false)
        HP -= Damage;
       
        if(base.HP <= 0 && isImortal == false)
        {
            NotifyObserver(this, EnemyEvent.Dead);
        }
    }
    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetKnewPos, 0.5f);
    }

    private void BlackBoardUpdate()
    {
        moveInputVelocity_LocalCommand = TransformWorldToLocalVector(moveInputVelocity_WorldCommand, transform.forward);

        //posture = Mathf.Clamp(posture, 0, 100);
    }
    public void BlackBoardBufferUpdate()
    {
        isSwitchWeaponCommand = false;
        isAimingCommand = false;
        isReloadCommand = false;
        _isPainTrigger = false;
        _triggerHitedGunFu = false;
        _tiggerThrowAbleObjectHit = false;

    }
   

    #region Initailized WeaponAdvanceUser
    [SerializeField] private Transform weaponMainSocket;
    [SerializeField] private Transform primaryWeaponHoster;
    [SerializeField] private Transform secondaryWeaponHoster;

    public bool isSwitchWeaponCommand { get; set; }
    public bool isPullTriggerCommand { get ; set ; }
    public bool isAimingCommand { get;  set ; }
    public bool isReloadCommand { get ; set ; }

    public Animator weaponUserAnimator { get; set; }
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }
    public Vector3 shootingPos { 
        get { return enemyGetShootDirection.GetShootingPos(); }
        set { } 
    }
    public Vector3 pointingPos { get => enemyGetShootDirection.GetPointingPos() ;
        set { } }

    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public Character userWeapon => this;
    public WeaponManuverManager weaponManuverManager { get; set; }
    public void Initialized_IWeaponAdvanceUser()
    {
        weaponUserAnimator = animator;
        currentWeaponSocket = weaponMainSocket;
        weaponBelt = new WeaponBelt(primaryWeaponHoster, secondaryWeaponHoster, new AmmoProuch(1000, 1000, 360, 360));
        weaponAfterAction = new WeaponAfterActionEnemy(this);
        weaponCommand = new WeaponCommand(this);
        weaponManuverManager = new EnemyWeaponManuver(this,this);
    }
    #endregion

    #region InitializedMotionControl

    [SerializeField] GameObject head;
    [SerializeField] GameObject spline;
    [SerializeField] GameObject hip;
    [SerializeField] GameObject right_upperLeg;
    [SerializeField] GameObject right_lowerLeg;
    [SerializeField] GameObject left_upperLeg;
    [SerializeField] GameObject left_lowerLeg;
    [SerializeField] GameObject right_upperArm;
    [SerializeField] GameObject right_lowerArm;
    [SerializeField] GameObject left_upperArm;
    [SerializeField] GameObject left_lowerArm;

    public List<GameObject> bones { get ; set ; }
    public GameObject hips { get ; set ; }
    Animator IMotionDriven.animator { get => animator; set => animator = value; }
    public MotionControlManager motionControlManager { get; set; }
    public void MotionControlInitailized()
    {
        hips = this.hip;
        bones = new List<GameObject>();
        bones.Add(head);
        bones.Add(spline);
        bones.Add(hip);
        bones.Add(right_upperLeg);
        bones.Add (right_lowerLeg);
        bones.Add(left_upperLeg);
        bones.Add(left_lowerLeg);
        bones.Add(right_upperArm);
        bones.Add(right_lowerArm);
        bones.Add(left_upperArm);
        bones.Add(left_lowerArm);

        motionControlManager = new MotionControlManager(bones,hips,animator);
    }
    #endregion

    #region InitailizedCombatInstinct
    public CombatOffensiveInstinct combatOffensiveInstinct { get; set ; }
    public FieldOfView fieldOfView { get => this.enemyFieldOfView; }
    public GameObject objInstict { get ; set ; }
    public LayerMask targetLayer { get => this.targetMask; set => targetMask = value; }

    public void InitailizedCombatOffensiveInstinct()
    {
        objInstict = gameObject;
        combatOffensiveInstinct = new CombatOffensiveInstinct(fieldOfView,this,base.My_environment,this);
    }
    #endregion

    #region InitailizedFindingTarget
    public GameObject userObj { get => gameObject; }
    FieldOfView IFindingTarget.fieldOfView { get => this.enemyFieldOfView; set => this.enemyFieldOfView = value; }
    LayerMask IFindingTarget.targetLayer { get => targetMask; set => targetMask = value; }
    public FindingTarget findingTargetComponent { get ; set; }
    public Vector3 targetKnewPos { get ; set ; }
    public void InitailizedFindingTarget()
    {
        findingTargetComponent = new FindingTarget(targetLayer, fieldOfView, this);
    }
    #endregion

    #region InitailizedCoverUsable
    public Vector3 peekPos { get ; set ; }
    public Vector3 coverPos { get ; set ; }
    public CoverPoint coverPoint { get; set; }
    public Character userCover { get ; set; }
    public FindingCover findingCover { get; set; }
    public bool isInCover { get ; set ; }

    public void InitailizedCoverUsable()
    {
        userCover = this;
        findingCover = new FindingCover(this, this);
    }


    #endregion

    #region InitailizedHearingComponent
    public GameObject userHearing { get ; set ; }
    public Environment environment { get => My_environment ; }
    public HearingSensing hearingSensing { get; set; }
   

    public void InitailizedHearingComponent()
    {
        userHearing = gameObject;
        hearingSensing = new HearingSensing(this, this.environment, 50);
    }
    public void GotHearding(GameObject souceSound)
    {
        
        if (souceSound.TryGetComponent<Player>(out Player player) == false) 
        return;

        if(Vector3.Distance(souceSound.transform.position,transform.position)>10)
            return;

        if(Vector3.Distance(souceSound.transform.position, transform.position) < 5)
        {
            NotifyObserver(this, EnemyEvent.HeardingGunShoot);

            targetKnewPos = new Vector3(souceSound.transform.position.x, souceSound.transform.position.y, souceSound.transform.position.z);

            if (combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Chill
                || combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Suspect)
            {
                combatOffensiveInstinct.myCombatPhase = CombatOffensiveInstinct.CombatPhase.Alert;
            }
            return;
        }
        if (Vector3.Distance(souceSound.transform.position, transform.position) < 10)
        {
            Ray ray = new Ray(transform.position
                ,(souceSound.transform.position-transform.position).normalized);

            if (Physics.Raycast(ray,out RaycastHit hitInfo, 1000, 0))
            {
                if(hitInfo.collider.gameObject != souceSound)
                    return;

                NotifyObserver(this, EnemyEvent.HeardingGunShoot);

                targetKnewPos = new Vector3(souceSound.transform.position.x, souceSound.transform.position.y, souceSound.transform.position.z);

                if (combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Chill
                    || combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Suspect)
                {
                    combatOffensiveInstinct.myCombatPhase = CombatOffensiveInstinct.CombatPhase.Alert;
                }
                return;

            }
          
        }





    }

    #endregion 

    #region InitailizedMovementComponent
    public Vector3 moveInputVelocity_WorldCommand;
    public Vector3 moveInputVelocity_LocalCommand;
    public Vector3 lookRotationCommand;

    [Range(0, 10)]
    public float moveAccelerate;
    [Range(0, 10)]
    public float moveMaxSpeed;
    [Range(0, 10)]
    public float moveRotateSpeed;

    [Range(0, 10)]
    public float sprintAccelerate;
    [Range(0, 10)]
    public float sprintMaxSpeed;
    [Range(0, 10)]
    public float sprintRotateSpeed;

    [Range(0,10)]
    public float breakAccelerate;
    [Range(0,10)]
    public float breakMaxSpeed;

    [Range(0, 10)]
    public float aimingRotateSpeed;

    [Range(0, 100)]
    public float hitedForcePush;
    [Range(0, 100)]
    public float hitedForceStop;

    [Range(0, 100)]
    public float painStateForceStop;

    public bool isSprintCommand { get; set; }


    #endregion

    #region InitilizedPainState
    public bool _isPainTrigger { get ; set ; }
    public bool _isInPain { get 
        {
            if(_painPart == IPainStateAble.PainPart.None)
                return false;
            else return true;
        } set { } }
    public float _posture { get => posture ; set => posture = value ; }
    [Range(0, 100)]
    [SerializeField] private float postureLight;
    public float _postureLight { get => postureLight ; set => postureLight = value ; }
    [Range(0, 100)]
    [SerializeField] private float postureMedium;
    public float _postureMedium { get => postureMedium ; set => postureMedium = value ; }
    [Range(0, 100)]
    [SerializeField] private float postureHeavy;
    public float _postureHeavy { get => postureHeavy ; set => postureHeavy = value; }

    public IPainStateAble.PainPart _painPart { get ; set ; }

    [SerializeField] private PainStateDurationScriptableObject painDurScrp;
    public PainStateDurationScriptableObject _painDurScrp { get => painDurScrp; }
    public void InitializedPainState()
    {
        _painPart = IPainStateAble.PainPart.None;
    }
   

    #endregion

    #region InitailizedPatrol
    public GameObject userPatrol { get ; set ; }
    public List<PatrolPoint> patrolPoints { get => this.PatrolPoints ; set => PatrolPoints = value ; }
    public int Index { get ; set ; }

    
    [SerializeField] private List<PatrolPoint> PatrolPoints = new List<PatrolPoint>();
    public void InitailizedPatrolComponent()
    {
        this.userPatrol = gameObject;
        Index = 0;
    }

    #endregion

    #region InitailizedFallDownGetUp Properties
    [SerializeField] private AnimationClip standUpClip;
    [SerializeField] private AnimationClip pushUpClip;
    public AnimationClip _standUpClip => standUpClip;

    public AnimationClip _pushUpClip => pushUpClip;

    Animator IFallDownGetUpAble._animator => animator;

    [SerializeField] private Transform hipsBone;
    public Transform _hipsBone => hipsBone;

    [SerializeField] private Transform rootModel;
    public Transform _root => rootModel;
    public Transform[] _bones => hipsBone.GetComponentsInChildren<Transform>();

    public Rigidbody[] _ragdollRigidbodies => rootModel.GetComponentsInChildren<Rigidbody>();

    #endregion

    #region ImplementGunFuGotHitAble
    public bool _triggerHitedGunFu { get; set ; }
    public Vector3 attackedPos { get; set; }
    public Transform _gunFuHitedAble { get{ return transform; } set { } }
    public IGunFuAble gunFuAbleAttacker { get; set ; }
    public IGunFuNode curAttackerGunFuNode { get ; set ; }
    public INodeLeaf curNodeLeaf { get => enemyStateManagerNode.curNodeLeaf; set { } }

    bool IGunFuGotAttackedAble._isDead { get => this.isDead; set { } }
    [SerializeField] public GunFu_GotHit_ScriptableObject GotHit1;
    [SerializeField] public GunFu_GotHit_ScriptableObject GotHit2;
    [SerializeField] public GunFu_GotHit_ScriptableObject KnockDown;
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attacker)
    {
        _triggerHitedGunFu = true;
        curAttackerGunFuNode = gunFu_NodeLeaf;
        attackedPos = attacker._gunFuUserTransform.position;
        gunFuAbleAttacker = attacker;
    }
    #endregion

    #region ImplementIFriendlyFire
    public IFriendlyFirePreventing.FriendlyFirePreventingMode curFriendlyFireMode { get ; set ; }
    public int allieID { get ; set ; }
    public FriendlyFirePreventingBehavior friendlyFirePreventingBehavior { get; set; }

    #endregion

    #region ImplementIThrowAbleVisitable
    [SerializeField] public bool _tiggerThrowAbleObjectHit { get;private set; }
    

    public void GotVisit(IThrowAbleObjectVisitor throwAbleObjectVisitor)
    {
        Debug.Log("Enemy Got _tiggerThrowAbleObjectHit");
        _tiggerThrowAbleObjectHit = true;
    }
    #endregion

    #region TransformLocalWorld
    private Vector3 TransformLocalToWorldVector(Vector3 dirChild, Vector3 dirParent)
    {
        float zeta;

        Vector3 Direction;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
    private Vector3 TransformWorldToLocalVector(Vector3 dirChild, Vector3 dirParent)
    {
        Vector3 Direction = Vector3.zero;
        float zeta;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        zeta = -zeta;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }

    



    #endregion



}
