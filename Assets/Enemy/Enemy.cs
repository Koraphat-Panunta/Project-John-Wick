using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using static Reload;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser,IMotionDriven,ICombatOffensiveInstinct,IFindingTarget
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public MultiRotationConstraint rotationConstraint;
    //public GameObject Target;
    public EnemyStateManager enemyStateManager;
    [SerializeField] public EnemyPath enemyPath;

    public LayerMask targetMask;
    public IEnemyTactic currentTactic;
    public FieldOfView enemyFieldOfView;
    public EnemyHearingSensing enemyHearingSensing;
    public EnemyGetShootDirection enemyGetShootDirection;
    public EnemyComunicate enemyComunicate;
    public float cost;
    public float pressure;

    public IEnemyHitReaction enemyHitReaction;
    public EnemyMiniFlinch enemyMiniFlinch;

    [SerializeField] private bool isImortal;
    public Transform rayCastPos;
    public bool isIncombat;

    public float offensiveIntenstiby;

   

    void Start()
    {
        //Target = new GameObject();
        enemyStateManager = new EnemyStateManager(this);    
        enemyPath = new EnemyPath(agent);
        enemyFieldOfView = new FieldOfView(120, 225,this.gameObject.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);
        enemyHearingSensing = new EnemyHearingSensing(this);
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
        new WeaponFactorySTI9mm().CreateWeapon(this);
        cost = Random.Range(36, 40);
        pressure = 100;
        this.isIncombat = false;
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
        Debug.Log("target know position = " + targetKnewPos);
    }

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

    #region GoapAI
    private EnemyGoalSelector startSelector { get; set; }
    private EnemyGoalLeaf curGoal { get; set; }
    private EncouterGoal encouterGoal { get; set; }
    private TakeCoverGoal takeCoverGoal { get; set; }
    private HoldingGoal holdingGoal { get; set; }
    private PatrolingGoal patrolingGoal { get; set; }

    private void InitailizedGoap()
    {
        startSelector = new EnemyGoalSelector(this,()=>true);

        encouterGoal = new EncouterGoal(this);
        takeCoverGoal = new TakeCoverGoal(this);
        holdingGoal = new HoldingGoal(this);
        patrolingGoal = new PatrolingGoal(this);

        startSelector.Transition(out EnemyGoalLeaf enemyGoalLeaf);
        curGoal = enemyGoalLeaf;

    }
    private void GoapUpdate()
    {
        if (curGoal.IsReset()){

            curGoal.Exit();
            curGoal = null;
            startSelector.Transition(out EnemyGoalLeaf enemyGoalLeaf);
            //Debug.Log("Out PlayerNode = " + enemyGoalLeaf);
            curGoal = enemyGoalLeaf;
            curGoal.Enter();
        }

        if (curGoal != null)
            curGoal.Update();
    }
    private void GoapFixedUpdate() 
    {
        if(curGoal != null)
            curGoal.FixedUpdate();
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
        combatOffensiveInstinct = new CombatOffensiveInstinct(fieldOfView,this,base.My_environment);
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
}
