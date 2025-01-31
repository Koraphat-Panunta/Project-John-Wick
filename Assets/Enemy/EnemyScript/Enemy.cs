using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser, IMotionDriven,
    ICombatOffensiveInstinct, IFindingTarget, ICoverUseable,
    IHearingComponent, IPatrolComponent,
    IPainState,IFallDownGetUpAble,IGunFuDamagedAble
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

    public readonly float maxCost = 100;
    public readonly float lowestCost = 0;
    public float cost;
    public float myHP;
    private float posture;


    [SerializeField] private bool isImortal;
    public Transform rayCastPos;

    

    protected override void Start()
    {
     
        enemyFieldOfView = new FieldOfView(120, 225,rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        enemyComunicate = new EnemyComunicate(this);

        enemyMovement = new EnemyMovement(agent,this);


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
        BlackBoardBufferUpdate();
        weaponManuverManager.UpdateNode();

    }
    private void FixedUpdate()
    {
        FixedUpdateState();
        weaponManuverManager.FixedUpdateNode();
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

    public FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf { get; private set; }
    public EnemyDeadStateNode enemtDeadState { get; private set; }
    public EnemySprintStateNode enemySprintState { get; private set; }
    public EnemyStandIdleStateNode enemyStandIdleState { get; private set; }
    public EnemyStandMoveStateNode enemyStandMoveState { get; private set; }
    public EnemyStandTakeCoverStateNode enemyStandTakeCoverState { get; private set; }
    public EnemyStandTakeAimStateNode enemyStandTakeAimState { get; private set; }

    public GotHit1_GunFuGotHitNodeLeaf gotHit1_GunFuHitNodeLeaf { get; private set; }
    public GotHit2_GunFuGotHitNodeLeaf gotHit2_GunFuHitNodeLeaf { get; private set; }
    public GotKnockDown_GunFuGotHitNodeLeaf gotKnockDown_GunFuNodeLeaf { get; private set; }

    public HumandShield_GotInteract_NodeLeaf gotHumandShielded_GunFuNodeLeaf { get; private set; }

    #region PainState Node
    public EnemyStateSelectorNode painStateSelector { get; private set; }
    public EnemyStateSelectorNode head_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Body_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Arm_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Leg_PainState_Selector { get; private set; }

    //Head PainState LeafNode
    //public HeavyPainStateHeadNode enemy_Head_PainState_Heavy_NodeLeaf { get; private set; }
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
                //Debug.Log("isPainTrigger = " + _isPainTrigger);
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

        //enemy_Head_PainState_Heavy_NodeLeaf = new HeavyPainStateHeadNode(this,animator);
        enemy_Head_PainState_Light_NodeLeaf = new LightPainStateHeadNode(this, animator);

        //head_PainState_Selector.AddChildNode(enemy_Head_PainState_Heavy_NodeLeaf);
        head_PainState_Selector.AddChildNode(enemy_Head_PainState_Light_NodeLeaf);

        enemy_BodyFront_PainState_Heavy_NodeLeaf = new HeavyPainStateFrontBody(this, animator);
        enemy_BodyFront_PainState_Medium_NodeLeaf = new MeduimPainStateFrontBody(this, animator);
        enemy_BodyFront_PainState_Light_NodeLeaf = new LightPainStateFrontBody(this, animator);

        enemy_BodyBack_PainState_Heavy_NodeLeaf = new HeavyPainStateBackBody(this, animator);
        enemy_BodyBack_PainState_Light_NodeLeaf = new LightPainStateBackBody(this, animator);

        Body_PainState_Selector.AddChildNode(enemy_BodyFront_PainState_Heavy_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyBack_PainState_Heavy_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyFront_PainState_Medium_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyFront_PainState_Light_NodeLeaf);
        Body_PainState_Selector.AddChildNode(enemy_BodyBack_PainState_Light_NodeLeaf);

        enemy_LeftArm_PainState_Heavy_NodeLeaf = new HeavyPainStateLeftArmNode(this, animator);
        enemy_LeftArm_PainState_Light_NodeLeaf = new LightPainStateLeftArmNode(this, animator);

        enemy_RightArm_PainState_Heavy_NodeLeaf = new HeavyPainStateRightArmNode(this, animator);
        enemy_RightArm_PainState_Light_NodeLeaf = new LightPainStateRightArmNode(this, animator);

        Arm_PainState_Selector.AddChildNode(enemy_LeftArm_PainState_Heavy_NodeLeaf);
        Arm_PainState_Selector.AddChildNode(enemy_RightArm_PainState_Heavy_NodeLeaf);
        Arm_PainState_Selector.AddChildNode(enemy_LeftArm_PainState_Light_NodeLeaf);
        Arm_PainState_Selector.AddChildNode(enemy_RightArm_PainState_Light_NodeLeaf);


        enemy_LeftLeg_PainState_Heavy_NodeLeaf = new HeavyPainStateLeftLeg(this, animator);
        enemy_LeftLeg_PainState_Light_NodeLeaf = new LightPainStateLeftLeg(this, animator);

        enemy_RightLeg_PainState_Light_NodeLeaf = new LightPainStateRightLeg(this, animator);
        enemy_RightLeg_PainState_Heavy_NodeLeaf = new HeavyPainStateRightLeg(this, animator);

        Leg_PainState_Selector.AddChildNode(enemy_LeftLeg_PainState_Heavy_NodeLeaf);
        Leg_PainState_Selector.AddChildNode(enemy_RightLeg_PainState_Heavy_NodeLeaf);
        Leg_PainState_Selector.AddChildNode(enemy_LeftLeg_PainState_Light_NodeLeaf);
        Leg_PainState_Selector.AddChildNode(enemy_RightLeg_PainState_Light_NodeLeaf);

    }
    #endregion

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
        fallDown_EnemyState_NodeLeaf = new FallDown_EnemyState_NodeLeaf(this, this, 
            () => //PreCondition
        {
            if (_isPainTrigger && posture <= 0)
                return true;
            return false;
        }
       );
        enemySprintState = new EnemySprintStateNode(this);
        enemyStandIdleState = new EnemyStandIdleStateNode(this,
            ()=>true, //PreCondition
            ()=>
            {
                if(_isPainTrigger)
                    return true;    

                if(isSprintCommand)
                    { return true; }

                if (isDead)
                    return true;

                if (moveInputVelocity_WorldCommand.magnitude > 0)
                    return true;
                
                return false;
            }
            );
        enemyStandMoveState = new EnemyStandMoveStateNode(this,()=> moveInputVelocity_WorldCommand.magnitude > 0, 
            ()=> 
            {
                if (isDead)
                    return true;

                if (moveInputVelocity_WorldCommand.magnitude <= 0)
                    { return true; }

                if(isSprintCommand)
                    { return true; }

                if(_isPainTrigger)
                    { return true; }

                return false;
            }
            );
        enemyStandTakeCoverState = new EnemyStandTakeCoverStateNode(this,this);
        enemyStandTakeAimState = new EnemyStandTakeAimStateNode(this, this);

        gotHit1_GunFuHitNodeLeaf = new GotHit1_GunFuGotHitNodeLeaf(this,GotHit1);
        gotHit2_GunFuHitNodeLeaf = new GotHit2_GunFuGotHitNodeLeaf(this,GotHit2);
        gotKnockDown_GunFuNodeLeaf = new GotKnockDown_GunFuGotHitNodeLeaf(this,KnockDown);

        gotHumandShielded_GunFuNodeLeaf = new HumandShield_GotInteract_NodeLeaf(this, animator);

        startSelector.AddChildNode(enemtDeadState);
        startSelector.AddChildNode(fallDown_EnemyState_NodeLeaf);
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

    public void ChangeStateNode(EnemyStateLeafNode enemyStateLeafNode)
    {
        if(curStateLeaf != null)
            curStateLeaf.Exit();

        curStateLeaf = enemyStateLeafNode;
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
    public Vector3 moveInputVelocity_WorldCommand { get; set; }
    public Vector3 moveInputVelocity_LocalCommand { get; set; }
    public Vector3 lookRotationCommand { get; set; }

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

    public bool isSprintCommand { get; set; }


    #endregion

    #region InitilizedPainState
    public bool _isPainTrigger { get ; set ; }
    public bool _isInPain { get 
        {
            if(_painPart == IPainState.PainPart.None)
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

    public HumandShield_GotInteract_NodeLeaf _humandShield_GotInteract_NodeLeaf { get => gotHumandShielded_GunFuNodeLeaf; set => gotHumandShielded_GunFuNodeLeaf = value; }

    [SerializeField] GunFu_GotHit_ScriptableObject GotHit1;
    [SerializeField] GunFu_GotHit_ScriptableObject GotHit2;
    [SerializeField] GunFu_GotHit_ScriptableObject KnockDown;
    public void TakeGunFuAttacked(GunFuHitNodeLeaf gunFu_NodeLeaf, IGunFuAble attacker)
    {
        switch (gunFu_NodeLeaf)
        {
            case Hit1GunFuNode hit1GunFuNode:
                {
                    gotHit1_GunFuHitNodeLeaf.gunFuAble = attacker;
                    ChangeStateNode(gotHit1_GunFuHitNodeLeaf);

                }
                break;

            case Hit2GunFuNode hit2GunFuNode:
                {
                    gotHit2_GunFuHitNodeLeaf.gunFuAble = attacker;
                    ChangeStateNode(gotHit2_GunFuHitNodeLeaf);

                }
                break;

            case KnockDown_GunFuNode knockDownGunFuNode: 
                {
                    gotKnockDown_GunFuNodeLeaf.gunFuAble = attacker;
                    ChangeStateNode(gotKnockDown_GunFuNodeLeaf);

                }
                break;
        }
        attackedPos = attacker._gunFuUserTransform.position;
    }
    public void TakeGunFuAttacked(GunFu_Interaction_NodeLeaf gunFu_Interaction_NodeLeaf, IGunFuAble gunFuAble)
    {
        switch (gunFu_Interaction_NodeLeaf)
        {
            case HumanShield_GunFuInteraction_NodeLeaf humandShield_GunFuNode:
                {
                    if(humandShield_GunFuNode.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.InteractionPhase.Enter)
                    ChangeStateNode(gotHumandShielded_GunFuNodeLeaf);
                }
                break;
        }
        attackedPos = gunFuAble._gunFuUserTransform.position;
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
