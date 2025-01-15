using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser, IMotionDriven,
    ICombatOffensiveInstinct, IFindingTarget, ICoverUseable,
    IHearingComponent, IMovementCompoent, IPatrolComponent,
    IPainState
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

    public readonly float maxCost = 100;
    public readonly float lowestCost = 0;
    public float cost;
    public float myHP;
    public float posture;



  

    [SerializeField] private bool isImortal;
    public Transform rayCastPos;

    

    void Start()
    {
     
        enemyFieldOfView = new FieldOfView(120, 225,this.gameObject.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        enemyComunicate = new EnemyComunicate(this);


    

        MotionControlInitailized();



        Initialized_IWeaponAdvanceUser();
        InitailizedCombatOffensiveInstinct();
        InitailizedFindingTarget();
        InitailizedCoverUsable();
        InitailizedHearingComponent();
        new WeaponFactorySTI9mm().CreateWeapon(this);
        cost = Random.Range(50,70);
        posture = 100;

        base.HP = 100;
        InitailizedStateNode();
    }

    void Update()
    {
        myHP = base.HP;
        findingTargetComponent.FindTarget(out GameObject target);
        combatOffensiveInstinct.UpdateSening();
       
        UpdateState();
      
    }
    private void FixedUpdate()
    {
     
        FixedUpdateState();
    }
    private void LateUpdate()
    {
        BlackBoardBufferUpdate();
    }
    public void TakeDamage(float Damage)
    {
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
        moveInputVelocity_Local = TransformWorldToLocalVector(moveInputVelocity_World, transform.forward);
        curMoveVelocity_Local = TransformWorldToLocalVector(curMoveVelocity_World, transform.forward);
    }
    public void BlackBoardBufferUpdate()
    {
        isReload = false;
        isSwapShoulder = false;
        isSwitchWeapon = false;
        _isPainTrigger = false;
    }

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


    #region Initailized State Node
    public EnemyStateLeafNode curStateLeaf;

    private EnemyStateSelectorNode startSelector;

    private EnemyStateSelectorNode standSelector;
    private EnemyStateSelectorNode takeCoverSelector;

    private EnemyStateSelectorNode painStateSelector;

    private EnemyDeadStateNode enemtDeadState;
    private EnemySprintStateNode enemySprintState;
    private EnemyStandIdleStateNode enemyStandIdleState;
    private EnemyStandMoveStateNode enemyStandMoveState;
    private EnemyStandTakeCoverStateNode enemyStandTakeCoverState;
    private EnemyStandTakeAimStateNode enemyStandTakeAimState;

    private LightPainStateFrontBody bodyHit;
    private LightPainStateRightLeg legsHit;
    //private EnemyStateLeafNode miniFlich;
    private FallDown ragDoll;
    
    private void InitailizedStateNode() 
    {
        startSelector = new EnemyStateSelectorNode(this,()=>true);
        
        standSelector = new EnemyStateSelectorNode(this,() =>true);
        takeCoverSelector = new EnemyStateSelectorNode(this, 
            ()=> 
            {
                if(isInCover)
                    { return true; }
                return false;
            }
            );

        painStateSelector = new EnemyStateSelectorNode(this, 
            () =>
            {
                if(_isPainTrigger)
                    { return true; }
                return false;
            }
            );

        enemtDeadState = new EnemyDeadStateNode(this);
        enemySprintState = new EnemySprintStateNode(this);
        enemyStandIdleState = new EnemyStandIdleStateNode(this,
            ()=>true, //PreCondition
            ()=>
            {
                if(isSprint)
                    { return true; }

                if (isDead)
                    return true;

                if (moveInputVelocity_World.magnitude > 0)
                    return true;
                
                return false;
            }
            );
        enemyStandMoveState = new EnemyStandMoveStateNode(this,()=> moveInputVelocity_World.magnitude > 0, 
            ()=> 
            {
                if (isDead)
                    return true;

                if (moveInputVelocity_World.magnitude <= 0)
                    { return true; }

                if(isSprint)
                    { return true; }

                if(_isPainTrigger)
                    { return true; }

                return false;
            }
            );
        enemyStandTakeCoverState = new EnemyStandTakeCoverStateNode(this,this);
        enemyStandTakeAimState = new EnemyStandTakeAimStateNode(this, this);

        ragDoll = new FallDown(this);
        bodyHit = new LightPainStateFrontBody(this);
        legsHit = new LightPainStateRightLeg(this);
        //miniFlich = new EnemyStateLeafNode(this,
        //    () => true, //PreCondition
        //    () => enemyMiniFlinch.TriggerFlich(), //Enter
        //    () => { },  //Exit
        //    () => { }, //Update
        //    () => { }, //FixedUpdate
        //    () => 
        //    {
        //        if (isDead)
        //            return true;

        //        if (enemyMiniFlinch.IsFliching()) 
        //        { return true; }

        //        return false;
        //    }
        //    ); //IsReset


        startSelector.AddChildNode(enemtDeadState);
        //startSelector.AddChildNode(ragDoll);
        startSelector.AddChildNode(painStateSelector);
        startSelector.AddChildNode(standSelector);

        painStateSelector.AddChildNode(bodyHit);
        painStateSelector.AddChildNode(legsHit);
        //painStateSelector.AddChildNode(miniFlich);

        standSelector.AddChildNode(enemySprintState);
        standSelector.AddChildNode(takeCoverSelector);
        standSelector.AddChildNode(enemyStandMoveState);
        standSelector.AddChildNode(enemyStandIdleState);

        takeCoverSelector.AddChildNode(enemyStandTakeAimState);
        takeCoverSelector.AddChildNode(enemyStandTakeCoverState);

        startSelector.Transition(out EnemyStateLeafNode enemyStateActionNode);
        curStateLeaf = enemyStateActionNode;
        curStateLeaf.Enter();

    }

    private void UpdateState() 
    {
        if (curStateLeaf.IsReset())
        {
            curStateLeaf.Exit();
            startSelector.Transition(out EnemyStateLeafNode enemyStateLeafNode);
            curStateLeaf = enemyStateLeafNode;
            curStateLeaf.Enter();
        }

        if(curStateLeaf != null)
            curStateLeaf.Update();
    }
    private void FixedUpdateState() 
    {
        if (curStateLeaf != null)
            curStateLeaf.FixedUpdate();
    }

    #endregion

    #region Initailized WeaponAdvanceUser
    [SerializeField] private Transform weaponMainSocket;
    [SerializeField] private Transform primaryWeaponHoster;
    [SerializeField] private Transform secondaryWeaponHoster;
    public Animator weaponUserAnimator { get; set; }
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }
    public Vector3 pointingPos { 
        get { return enemyGetShootDirection.GetDir(); }
        set { } 
    }
    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public Character userWeapon => this;
    public bool isAiming { get ; set ; }
    public bool isPullTrigger { get ; set ; }
    public bool isReload { get; set; }
    public bool isSwapShoulder { get; set; }
    public bool isSwitchWeapon { get; set; }
    public void Initialized_IWeaponAdvanceUser()
    {
        weaponUserAnimator = animator;
        currentWeaponSocket = weaponMainSocket;
        weaponBelt = new WeaponBelt(primaryWeaponHoster, secondaryWeaponHoster, new AmmoProuch(90, 90, 360, 360));
        weaponAfterAction = new WeaponAfterActionEnemy(this);
        weaponCommand = new WeaponCommand(this);
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

        targetKnewPos = new Vector3(souceSound.transform.position.x, souceSound.transform.position.y, souceSound.transform.position.z);
        Debug.Log("Got Hearding");
        if (combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Chill
            || combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.Suspect)
        {
            combatOffensiveInstinct.myCombatPhase = CombatOffensiveInstinct.CombatPhase.Alert;
        }
    }

    #endregion 

    #region InitailizedMovementComponent
    public GameObject userMovement { get; set; }
    public Vector3 moveInputVelocity_World { get ; set; }
    public Vector3 moveInputVelocity_Local { get ; set ; }
    public Vector3 curMoveVelocity_World { get ; set ; }
    public Vector3 curMoveVelocity_Local { get ; set; }
    public Vector3 lookRotation { get ; set ; }
    [Range(0, 10)]
    public float moveAccelerate;
    [Range(0, 10)]
    public float moveMaxSpeed;
    [Range(0, 10)]
    public float sprintAccelerate;
    [Range(0, 10)]
    public float sprintMaxSpeed;
    public float _moveAccelerate { get => this.moveAccelerate; set => this.moveAccelerate = value; }
    public float _moveMaxSpeed { get => this.moveMaxSpeed ; set => this.moveMaxSpeed = value ; }
    public float _sprintAccelerate { get => this.sprintAccelerate; set => this.sprintAccelerate = value; }
    public float _sprintMaxSpeed { get => this.sprintMaxSpeed; set=> this.sprintMaxSpeed = value; }
    public float _rotateSpeed { get ; set ; }
    public EnemyStateSelectorNode stanceSelector { get; set; }
    public EnemyStateSelectorNode standStateSelector { get; set; }
    public EnemyStateSelectorNode crouchStateSelector { get; set; }
    public EnemyStandIdleStateNode standIdleState { get; set; }
    public EnemyStandMoveStateNode standMoveState { get; set; }
    public EnemySprintStateNode sprintState { get; set; }
    public IMovementCompoent.Stance curStance { get; set; }
    public bool isSprint { get ; set ; }
   

    public void InitailizedMovementComponent()
    {
        this.userMovement = gameObject;

        curStance = IMovementCompoent.Stance.Stand;
    }


    #endregion

    #region InitilizedPainState
    public bool _isPainTrigger { get ; set ; }
    public bool _isInPain { get 
        {
            if(_painPart == IPainState.PainPart.None)
                return false;
            else return true;
        } set { } }
    public float _posture { get ; set ; }
    public IPainState.PainPart _painPart { get ; set ; }
    public void InitializedPainState()
    {
        _painPart = IPainState.PainPart.None;
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
}
