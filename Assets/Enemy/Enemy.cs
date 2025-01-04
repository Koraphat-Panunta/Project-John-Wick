using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser,IMotionDriven,ICombatOffensiveInstinct,IFindingTarget,ICoverUseable,IHearingComponent,IMovementCompoent
{
    [Range(0,100)]
    public float intelligent;
    [Range(0, 100)]
    public float strength;

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public MultiRotationConstraint rotationConstraint;
    //public GameObject Target;
    public EnemyStateManager enemyStateManager;
    [SerializeField] public EnemyPath enemyPath;

    public LayerMask targetMask;
    public IEnemyTactic currentTactic;
    public FieldOfView enemyFieldOfView;
    //public HearingSensing enemyHearingSensing;
    public EnemyGetShootDirection enemyGetShootDirection;
    public EnemyComunicate enemyComunicate;

    public readonly float maxCost = 100;
    public readonly float lowestCost = 0;
    public float cost;
    public float pressure;



    public IEnemyHitReaction enemyHitReaction;
    public EnemyMiniFlinch enemyMiniFlinch;

    [SerializeField] private bool isImortal;
    public Transform rayCastPos;
    //public bool isIncombat;

    public float offensiveIntenstiby;

   

    void Start()
    {
        //Target = new GameObject();
        enemyStateManager = new EnemyStateManager(this);    
        enemyPath = new EnemyPath(agent);
        enemyFieldOfView = new FieldOfView(120, 225,this.gameObject.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);
        //enemyHearingSensing = new HearingSensing(this);
        enemyComunicate = new EnemyComunicate(this);
        enemyMiniFlinch = new EnemyMiniFlinch(this);

        enemyStateManager._currentState = enemyStateManager._idle;
        enemyStateManager._currentState.StateEnter(enemyStateManager);

        MotionControlInitailized();
        //InitailizedGoap();

        currentTactic = new SerchingTactic(this);
        Initialized_IWeaponAdvanceUser();
        InitailizedCombatOffensiveInstinct();
        InitailizedFindingTarget();
        InitailizedCoverUsable();
        InitailizedHearingComponent();
        new WeaponFactorySTI9mm().CreateWeapon(this);
        cost = Random.Range(36, 40);
        pressure = 100;
        //base.isDead = false;

        base.HP = 100;
    }

    void Update()
    {
        //GoapUpdate();
        combatOffensiveInstinct.UpdateSening();
        offensiveIntenstiby = combatOffensiveInstinct.offensiveIntensity;
        enemyStateManager.Update();
    }
    private void FixedUpdate()
    {
        //GoapFixedUpdate();
        enemyStateManager.FixedUpdate();
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
            enemyStateManager.ChangeState(enemyStateManager.enemyDead);
        }
    }
    public IEnumerator RecoveryReloadLayerWeight(Weapon weapon)
    {
        float RecoveryWeight = 10;
        while (animator.GetLayerWeight(2) > 0)
        {
            //enemyWeaponCommand.ammoProuch.prochReload.Performed(weapon);
            new AmmoProchReload(weaponBelt.ammoProuch).Performed(weapon);
            animator.SetLayerWeight(2, animator.GetLayerWeight(2) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
    public IEnumerator RecoveryFiringLayerWeight()
    {
        float RecoveryWeight = 10;
        while (animator.GetLayerWeight(3) > 0)
        {
            animator.SetLayerWeight(3, animator.GetLayerWeight(3) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        //if (Target != null)
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        //}
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetKnewPos, 0.5f);
    }

    private void BlackBoardBufferUpdate()
    {
        isReload = false;
        isSwapShoulder = false;
        isSwitchWeapon = false;
    }

    #region Initailized State Node



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
            currentTactic = new FlankingTactic(this);
        }
    }

    #endregion 

    #region InitailizedMovementComponent
    public GameObject userMovement { get; set; }
    public Vector2 moveInputVelocity_World { get ; set; }
    public Vector2 moveInputVelocity_Local { get ; set ; }
    public Quaternion rotating { get; set; }
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
}
