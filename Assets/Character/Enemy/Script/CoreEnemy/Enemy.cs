using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public partial class Enemy : SubjectEnemy
    , IWeaponAdvanceUser, IMotionDriven,
     IFindingTarget, ICoverUseable,
    IHeardingAble, IPatrolComponent,
    IPainStateAble, IFallDownGetUpAble,
     IFriendlyFirePreventing,
    IThrowAbleObjectVisitable, ICommunicateAble
    , IBulletDamageAble
{
    [Range(0, 100)]
    public float intelligent;
    [Range(0, 100)]
    public float strength;

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public MultiRotationConstraint rotationConstraint;

    public LayerMask targetMask;
    public LayerMask targetSpoterMask;
    public FieldOfView enemyFieldOfView;
    public override MovementCompoent _movementCompoent { get ; set ; }
    public EnemyGetShootDirection enemyGetShootDirection;
    public INodeManager enemyStateManagerNode;
    private EnemyCommunicator enemyCommunicator;
    [SerializeField] Weapon startWeapon;


    public Vector3 forceSave;

    public float myHP;
    private float posture;

    
    [SerializeField] public bool isImortal;
    public Transform rayCastPos;

    public LayerMask selfLayerMask;

    protected override void Awake()
    {
        base.Awake();

        targetMask.value = LayerMask.GetMask("Player");

        posture = 100;
        base.HP = 100;
        base.maxHp = 100;

        enemyFieldOfView = new FieldOfView(120, 225, rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        _isGotAttackedAble = true;
        MotionControlInitailized();
        enemyStateManagerNode = new EnemyStateManagerNode(this);
        Initialized_IWeaponAdvanceUser();
        InitailizedFindingTarget();
        InitailizedCoverUsable();
        InitailizedGunFuComponent();
        friendlyFirePreventingBehavior = new FriendlyFirePreventingBehavior(this);
        _movementCompoent = new EnemyMovement(this,transform,this,agent);
        enemyCommunicator = new EnemyCommunicator(this);
    }
    protected override void Start()
    {
        if (startWeapon != null)
            new WeaponAttachingBehavior().Attach(startWeapon, MainHandSocket);
        base.Start();
    }

    void Update()
    {
        myHP = base.HP;
        findingTargetComponent.FindTarget(out GameObject target);
        enemyStateManagerNode.UpdateNode();
        _weaponManuverManager.UpdateNode();
        _movementCompoent.UpdateNode();

    }
    private void LateUpdate()
    {
        BlackBoardUpdate();
        BlackBoardBufferUpdate();
    }
    private void FixedUpdate()
    {
        enemyStateManagerNode.FixedUpdateNode();
        _weaponManuverManager.FixedUpdateNode();
        _movementCompoent.FixedUpdateNode();
    }

    public void TakeDamage(float Damage)
    {
        if (isImortal == false)
            HP -= Damage;
        
    }
    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        if (NotifyGotAttack != null)
            NotifyGotAttack.Invoke(damageVisitor);

        if (damageVisitor is Bullet bullet)
        {
            TakeDamage(bullet.hpDamage);
            bullet.weapon.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction
                <IBulletDamageAble>(WeaponAfterAction.WeaponAfterActionSending.HitConfirm,this);
        }
    }
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        throw new NotImplementedException();
    }
    private void BlackBoardUpdate()
    {
        moveInputVelocity_LocalCommand = TransformWorldToLocalVector(moveInputVelocity_WorldCommand, transform.forward);
    }
    public void BlackBoardBufferUpdate()
    {
        _isHolsterWeaponCommand = false;
        _isDrawPrimaryWeaponCommand = false;
        _isDrawSecondaryWeaponCommand = false;
        _isDropWeaponCommand = false;
        _isAimingCommand = false;
        _isReloadCommand = false;
        _isPainTrigger = false;
        _triggerHitedGunFu = false;
        _tiggerThrowAbleObjectHit = false;
        _isPickingUpWeaponCommand = false;
        _isPullTriggerCommand = false;
        _triggerGunFu = false;

    }


    #region Initailized WeaponAdvanceUser
    [SerializeField] private MainHandSocket MainHandSocket;
    [SerializeField] private SecondHandSocket SecondHandSocket;
    [SerializeField] private PrimaryWeaponSocket PrimaryWeaponSocket;
    [SerializeField] private SecondaryWeaponSocket SecondaryWeaponSocket;

    public bool _isPullTriggerCommand { get; set; }
    public bool _isAimingCommand { get; set; }
    public bool _isReloadCommand { get; set; }
    public bool _isDropWeaponCommand { get; set; }
    public bool _isPickingUpWeaponCommand { get; set; }
    public bool _isHolsterWeaponCommand { get; set; }
    public bool _isDrawPrimaryWeaponCommand { get; set; }
    public bool _isDrawSecondaryWeaponCommand { get; set; }

    public MainHandSocket _mainHandSocket { get => this.MainHandSocket; set => this.MainHandSocket = value; }
    public SecondHandSocket _secondHandSocket { get => this.SecondHandSocket; set => this.SecondHandSocket = value; }

    public Animator _weaponUserAnimator { get; set; }
    public Weapon _currentWeapon { get; set; }

    public Vector3 _shootingPos {
        get { return enemyGetShootDirection.GetShootingPos(); }
        set { }
    }
    public Vector3 _pointingPos { get => enemyGetShootDirection.GetPointingPos();
        set { } }

    public WeaponBelt _weaponBelt { get; set; }
    public WeaponAfterAction _weaponAfterAction { get; set; }
    public Character _userWeapon => this;

    [SerializeField] AnimatorOverrideController AnimatorOverrideController;
    public AnimatorOverrideController _animatorWeaponAdvanceUserOverride { get; set; }
    public WeaponManuverManager _weaponManuverManager { get; set; }
    public FindingWeaponBehavior _findingWeaponBehavior { get; set; }

    public void Initialized_IWeaponAdvanceUser()
    {
        _weaponUserAnimator = animator;
        _weaponBelt = new WeaponBelt(PrimaryWeaponSocket, SecondaryWeaponSocket, new AmmoProuch(1000, 1000, 1000, 1000
            , 1000, 1000, 1000, 1000));
        _weaponAfterAction = new WeaponAfterActionEnemy(this);
        _findingWeaponBehavior = new FindingWeaponBehavior(this);
        _weaponManuverManager = new EnemyWeaponManuver(this, this);
        _animatorWeaponAdvanceUserOverride = this.AnimatorOverrideController;
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

    public List<GameObject> bones { get; set; }
    public GameObject hips { get; set; }
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
        bones.Add(right_lowerLeg);
        bones.Add(left_upperLeg);
        bones.Add(left_lowerLeg);
        bones.Add(right_upperArm);
        bones.Add(right_lowerArm);
        bones.Add(left_upperArm);
        bones.Add(left_lowerArm);

        motionControlManager = new MotionControlManager(bones, hips, animator);
    }
    #endregion

    #region InitailizedFindingTarget

    public FindingTarget findingTargetComponent { get; set; }
    public Vector3 targetKnewPos;
    public Action<GameObject> NotifyEnemySpottingTarget;
    public void InitailizedFindingTarget()
    {
        findingTargetComponent = new FindingTarget(targetSpoterMask, enemyFieldOfView);
        findingTargetComponent.OnSpottingTarget += EnemySpotingTarget;

    }
    private void EnemySpotingTarget(GameObject target)
    {
        if (isDead)
            return;

        targetKnewPos = target.transform.position;
        enemyCommunicator.SendCommunicate(transform.position, 10, selfLayerMask, EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition);
        if (NotifyEnemySpottingTarget != null)
            NotifyEnemySpottingTarget.Invoke(target);
    }

    #endregion

    #region InitailizedCoverUsable
    public Vector3 peekPos { get; set; }
    public Vector3 coverPos { get; set; }
    public CoverPoint coverPoint { get; set; }
    public Character userCover { get; set; }
    public FindingCover findingCover { get; set; }
    public bool isInCover { get; set; }

    public void InitailizedCoverUsable()
    {
        userCover = this;
        findingCover = new EnemyFindCover(this, this, this);
    }


    #endregion

    #region InitailizedHearingComponent

    public Action<INoiseMakingAble> NotifyGotHearing { get; set; }
    public void GotHearding(INoiseMakingAble noiseMakingAble)
    {
        if (isDead)
            return;

        if (noiseMakingAble is Bullet bullet
            && bullet.weapon.userWeapon._userWeapon.gameObject.TryGetComponent<I_NPCTargetAble>(out I_NPCTargetAble i_NPCTargetAble))
        {
            targetKnewPos = i_NPCTargetAble.selfNPCTarget.transform.position;
        }

        NotifyObserver(this, EnemyEvent.HeardingGunShoot);
        if (NotifyGotHearing != null)
            NotifyGotHearing(noiseMakingAble);
    }

    #endregion
    #region ImplementCommunicateAble

    public Action<Communicator> NotifyCommunicate { get; set; }
    public GameObject communicateAble => gameObject;
    public void GetCommunicate<TypeCommunicator>(TypeCommunicator typeCommunicator) where TypeCommunicator : Communicator
    {

        if (isDead)
            return;


        if (typeCommunicator is EnemyCommunicator enemyCommunicator)
        {

            switch (enemyCommunicator.enemyCommunicateMassage)
            {
                case EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition:
                    {

                        targetKnewPos = enemyCommunicator.enemy.targetKnewPos;
                    }
                    break;
            }
        }

        if (NotifyCommunicate != null)
            NotifyCommunicate.Invoke(typeCommunicator);
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

    [Range(0, 10)]
    public float breakAccelerate;
    [Range(0, 10)]
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
    public bool _isPainTrigger { get; set; }
    public bool _isInPain { get
        {
            if(curNodeLeaf is EnemyPainStateNodeLeaf)
                return true;    
            return false;
        } set { } }
    public float _posture { get => posture; set => posture = value; }
    [Range(0, 100)]
    [SerializeField] private float postureLight;
    public float _postureLight { get => postureLight; set => postureLight = value; }
    [Range(0, 100)]
    [SerializeField] private float postureMedium;
    public float _postureMedium { get => postureMedium; set => postureMedium = value; }
    [Range(0, 100)]
    [SerializeField] private float postureHeavy;
    public float _postureHeavy { get => postureHeavy; set => postureHeavy = value; }

    public IPainStateAble.PainPart _painPart { get; set; }

    [SerializeField] private PainStateDurationScriptableObject painDurScrp;
    public PainStateDurationScriptableObject _painDurScrp { get => painDurScrp; }
    public void InitializedPainState()
    {
        _painPart = IPainStateAble.PainPart.None;
    }


    #endregion

    #region InitailizedPatrol
    public GameObject userPatrol { get; set; }
    public List<PatrolPoint> patrolPoints { get => this.PatrolPoints; set => PatrolPoints = value; }
    public int Index { get; set; }


    [SerializeField] private List<PatrolPoint> PatrolPoints = new List<PatrolPoint>();
    public void InitailizedPatrolComponent()
    {
        this.userPatrol = gameObject;
        Index = 0;
    }

    #endregion

    #region IBulletDamageAble
    public IBulletDamageAble bulletDamageAbleBodyPartBehavior { get; set; }
    public Action<IDamageVisitor> NotifyGotAttack;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetKnewPos, 0.14f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.15f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

   
}
