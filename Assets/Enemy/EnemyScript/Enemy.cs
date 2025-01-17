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

    

    protected override void Start()
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
        BlackBoardUpdate();
      
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

        //posture = Mathf.Clamp(posture, 0, 100);
    }
    public void BlackBoardBufferUpdate()
    {
        isReload = false;
        isSwapShoulder = false;
        isSwitchWeapon = false;
        _isPainTrigger = false;
    }




    #region Initailized State Node
    public EnemyStateLeafNode curStateLeaf { get;private set; }

    public EnemyStateSelectorNode startSelector { get; private set; }

    public EnemyStateSelectorNode standSelector { get; private set; }
    public EnemyStateSelectorNode takeCoverSelector { get; private set; }

    public EnemyDeadStateNode enemtDeadState { get; private set; }
    public EnemySprintStateNode enemySprintState { get; private set; }
    public EnemyStandIdleStateNode enemyStandIdleState { get; private set; }
    public EnemyStandMoveStateNode enemyStandMoveState { get; private set; }
    public EnemyStandTakeCoverStateNode enemyStandTakeCoverState { get; private set; }
    public EnemyStandTakeAimStateNode enemyStandTakeAimState { get; private set; }


    #region PainState Node
    public EnemyStateSelectorNode painStateSelector { get; private set; }
    public EnemyStateSelectorNode head_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Body_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Arm_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Leg_PainState_Selector { get; private set; }

    //Head PainState LeafNode
    public HeavyPainStateHeadNode enemy_Head_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateHeadNode enemy_Head_PainState_Light_NodeLeaf { get; private set; }
    
    //BodyFront PainSate LeafNode
    public HeavyPainStateFrontBody enemy_BodyFront_PainState_Heavy_NodeLeaf { get; private set; }
    public MeduimPainStateFrontBody enemy_BodyFront_PainState_Medium_NodeLeaf { get; private set; }
    public LightPainStateFrontBody enemy_BodyFront_PainState_Light_NodeLeaf { get; private set; }

    //BodyBack PainState LeafNode
    public HeavyPainStateBackBody enemy_BodyBack_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateBackBody enemy_BodyBack_PainState_Light_NodeLeaf { get; private set; }

    //ArmLeft PainState LeafNode
    public HeavyPainStateLeftArmNode enemy_LeftArm_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateLeftArmNode enemy_LeftArm_PainState_Light_NodeLeaf { get; private set; }

    //ArmRight PainState LeafNode
    public HeavyPainStateRightArmNode enemy_RightArm_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateRightArmNode enemy_RightArm_PainState_Light_NodeLeaf { get; private set; }

    //LegLeft PainState LeafNode
    public HeavyPainStateLeftLeg enemy_LeftLeg_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateLeftLeg enemy_LeftLeg_PainState_Light_NodeLeaf { get; private set; }

    //LegRight PainState LeafNode
    public HeavyPainStateRightLeg enemy_RightLeg_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateRightLeg enemy_RightLeg_PainState_Light_NodeLeaf { get; private set; }

    private void InitailizedPainStateNode()
    {
        painStateSelector = new EnemyStateSelectorNode(this,
            () =>
            {
                if (_isPainTrigger)
                { return true; }
                return false;
            }
            );

        head_PainState_Selector = new EnemyStateSelectorNode(this, () =>
        {
            if(_painPart == IPainState.PainPart.Head)
                return true;
            return false;
        });

        Body_PainState_Selector = new EnemyStateSelectorNode(this, () =>
        {
            if (_painPart == IPainState.PainPart.BodyBack
            ||_painPart == IPainState.PainPart.BodyFornt)
                return true;
            return false;
        });

        Arm_PainState_Selector = new EnemyStateSelectorNode(this, () =>
        {
            if(_painPart == IPainState.PainPart.ArmLeft
            ||_painPart == IPainState.PainPart.ArmRight)
                return true;
            return false;
        });

        Leg_PainState_Selector = new EnemyStateSelectorNode(this, () => 
        {
            if(_painPart == IPainState.PainPart.LegLeft
            || _painPart == IPainState.PainPart.LegRight)
                return true;
            return false;
        });

        painStateSelector.AddChildNode(head_PainState_Selector);
        painStateSelector.AddChildNode(Body_PainState_Selector);
        painStateSelector.AddChildNode(Arm_PainState_Selector);
        painStateSelector.AddChildNode(Leg_PainState_Selector);

        enemy_Head_PainState_Heavy_NodeLeaf = new HeavyPainStateHeadNode(this);
        enemy_Head_PainState_Light_NodeLeaf = new LightPainStateHeadNode(this);

        head_PainState_Selector.AddChildNode(enemy_Head_PainState_Heavy_NodeLeaf);
        head_PainState_Selector.AddChildNode(enemy_Head_PainState_Light_NodeLeaf);

        enemy_BodyFront_PainState_Heavy_NodeLeaf = new HeavyPainStateFrontBody(this);
        enemy_BodyFront_PainState_Medium_NodeLeaf = new MeduimPainStateFrontBody(this);
        enemy_BodyFront_PainState_Light_NodeLeaf = new LightPainStateFrontBody(this);

        enemy_BodyBack_PainState_Heavy_NodeLeaf = new HeavyPainStateBackBody(this);
        enemy_BodyBack_PainState_Light_NodeLeaf = new LightPainStateBackBody(this);

        Body_PainState_Selector.AddChildNode(enemy_BodyFront_PainState_Heavy_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyFront_PainState_Medium_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyFront_PainState_Light_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyBack_PainState_Heavy_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyBack_PainState_Light_NodeLeaf);

        enemy_LeftArm_PainState_Heavy_NodeLeaf = new HeavyPainStateLeftArmNode(this);
        enemy_LeftArm_PainState_Light_NodeLeaf = new LightPainStateLeftArmNode(this);

        enemy_RightArm_PainState_Heavy_NodeLeaf = new HeavyPainStateRightArmNode(this);
        enemy_RightArm_PainState_Light_NodeLeaf = new LightPainStateRightArmNode(this);

        Arm_PainState_Selector.AddChildNode(enemy_LeftArm_PainState_Heavy_NodeLeaf);
        Arm_PainState_Selector.AddChildNode(enemy_LeftArm_PainState_Light_NodeLeaf);
        Arm_PainState_Selector.AddChildNode(enemy_RightArm_PainState_Heavy_NodeLeaf);
        Arm_PainState_Selector.AddChildNode(enemy_RightArm_PainState_Light_NodeLeaf);


        enemy_LeftLeg_PainState_Heavy_NodeLeaf = new HeavyPainStateLeftLeg(this);
        enemy_LeftLeg_PainState_Light_NodeLeaf = new LightPainStateLeftLeg(this);

        enemy_RightLeg_PainState_Light_NodeLeaf = new LightPainStateRightLeg(this);
        enemy_RightLeg_PainState_Heavy_NodeLeaf = new HeavyPainStateRightLeg(this);

        Leg_PainState_Selector.AddChildNode(enemy_LeftLeg_PainState_Heavy_NodeLeaf);
        Leg_PainState_Selector.AddChildNode(enemy_LeftLeg_PainState_Light_NodeLeaf);
        Leg_PainState_Selector.AddChildNode(enemy_RightLeg_PainState_Light_NodeLeaf);
        Leg_PainState_Selector.AddChildNode(enemy_RightLeg_PainState_Heavy_NodeLeaf);
    }
    #endregion
    public FallDown ragDoll { get; private set; }

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

        InitailizedPainStateNode();

        enemtDeadState = new EnemyDeadStateNode(this);
        enemySprintState = new EnemySprintStateNode(this);
        enemyStandIdleState = new EnemyStandIdleStateNode(this,
            ()=>true, //PreCondition
            ()=>
            {
                if(_isPainTrigger)
                    return true;    

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

      


        startSelector.AddChildNode(enemtDeadState);
        startSelector.AddChildNode(painStateSelector);
        startSelector.AddChildNode(standSelector);

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
    [Range(0, 100)]
    [SerializeField] private float postureLight;
    public float _postureLight { get => postureLight ; set => postureLight = value ; }
    [Range(0, 100)]
    [SerializeField] private float postureMedium;
    public float _postureMedium { get => postureMedium ; set => postureMedium = value ; }
    [Range(0, 100)]
    [SerializeField] private float postureHeavy;
    public float _postureHeavy { get => postureHeavy ; set => postureHeavy = value; }

    public IPainState.PainPart _painPart { get ; set ; }

    [SerializeField] private PainStateDurationScriptableObject painDurScrp;
    public PainStateDurationScriptableObject _painDurScrp { get => painDurScrp; }
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
