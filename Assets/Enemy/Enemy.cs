using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using static Reload;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser,IMotionDriven
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public MultiRotationConstraint rotationConstraint;
    public GameObject Target;
    public EnemyStateManager enemyStateManager;
    [SerializeField] public EnemyPath enemyPath;

    public LayerMask targetMask;
    public IEnemyTactic currentTactic;
    public FieldOfView enemyFieldOfView;
    public EnemyLookForPlayer enemyLookForPlayer;
    public EnemyHearingSensing enemyHearingSensing;
    public EnemyGetShootDirection enemyGetShootDirection;
    public EnemyComunicate enemyComunicate;
    public float cost;
    public float pressure;

    public IEnemyHitReaction enemyHitReaction;
    public EnemyAgentMovementOverride enemyAgentMovementOverride;

    public EnemyMiniFlinch enemyMiniFlinch;

    [SerializeField] private bool isImortal;
    public Transform rayCastPos;
    public bool isIncombat;

   

    void Start()
    {
        Target = new GameObject();
        enemyStateManager = new EnemyStateManager(this);    
        //enemyWeaponCommand = new EnemyWeaponCommand(this);
        enemyPath = new EnemyPath(agent);
        
        enemyFieldOfView = new FieldOfView(120, 225,this.gameObject.transform);
        enemyLookForPlayer = new EnemyLookForPlayer(this,targetMask);
        enemyGetShootDirection = new EnemyGetShootDirection(this);
        enemyHearingSensing = new EnemyHearingSensing(this);
        enemyComunicate = new EnemyComunicate(this);
        enemyAgentMovementOverride = new EnemyAgentMovementOverride(agent);
        enemyMiniFlinch = new EnemyMiniFlinch(this);

        enemyStateManager._currentState = enemyStateManager._idle;
        enemyStateManager._currentState.StateEnter(enemyStateManager);

        MotionControlInitailized();

        currentTactic = new SerchingTactic(this);
        Initialized_IWeaponAdvanceUser();
        new WeaponFactorySTI9mm().CreateWeapon(this);
        cost = Random.Range(36, 40);
        pressure = 100;
        this.isIncombat = false;
        //base.isDead = false;

        base.HP = 100;
    }

    void Update()
    {
        enemyStateManager.Update();
    }
    private void FixedUpdate()
    {
        enemyStateManager.FixedUpdate();
    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
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
        if (Target != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        }
    }

    [SerializeField]private Transform weaponMainSocket;
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
    public void Initialized_IWeaponAdvanceUser()
    {
        weaponUserAnimator = animator;
        currentWeaponSocket = weaponMainSocket;
        weaponBelt = new WeaponBelt(primaryWeaponHoster, secondaryWeaponHoster, new AmmoProuch(90, 90, 360, 360));
        weaponAfterAction = new WeaponAfterActionEnemy(this);
        weaponCommand = new WeaponCommand(this);
    }

    

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
}
