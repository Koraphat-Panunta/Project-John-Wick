using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public partial class Enemy : SubjectEnemy
    , IMotionDriven,
     IFindingTarget, ICoverUseable,
    IHeardingAble, IPatrolComponent,
    IPainStateAble, 
     IFriendlyFirePreventing,
     ICommunicateAble
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

        this.SetDefaultAttribute();

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
        base.Start();
    }

    [SerializeField] private float _staggerGauge;
    [SerializeField] private bool isGround;
    void Update()
    {
        this.isGround = _movementCompoent.IsGround(out Vector3 groundPos);
        this._staggerGauge = this.staggerGauge;
        myHP = base.HP;
        this.FindingTargetUpdate();
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
        if (isImortal)
            return;

        SetHP(Mathf.Clamp(HP - Damage, 0, maxHp));
        
    }
    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        if (NotifyGotAttack != null)
            NotifyGotAttack.Invoke(damageVisitor);

        if (damageVisitor is Bullet bullet)
        {
            TakeDamage(bullet._hpDamage);
            bullet.weapon.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction
                <IBulletDamageAble>(WeaponAfterAction.WeaponAfterActionSending.HitConfirm,this);
            NotifyObserver(this, EnemyEvent.GotBulletHit);
        }
        if(damageVisitor is GunFuHitNodeLeaf gunFuHitNodeLeaf)
        {
            if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
            {

                if (this.staggerGauge > 0)
                    this.staggerGauge -= gunFuHitNodeLeaf.staggerHitDamage;
            }
        }
    }
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        throw new NotImplementedException();
    }
    private void BlackBoardUpdate()
    {
        moveInputVelocity_LocalCommand = TransformWorldToLocalVector(moveInputVelocity_WorldCommand, transform.forward);
        if (this.findingTargetComponent.isSpottingTarget)
            enemyGetShootDirection.SetTrackingRate(enemyGetShootDirection.trackingTargetRate + (Time.deltaTime * enemyGetShootDirection.trackingTargetAccelerate));
        else
            enemyGetShootDirection.SetTrackingRate(enemyGetShootDirection.trackingTargetRate - (Time.deltaTime * enemyGetShootDirection.trackingTargetDecelerate));
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
        _isPickingUpWeaponCommand = false;
        _isPullTriggerCommand = false;
        _triggerGunFu = false;
        moveInputVelocity_WorldCommand = Vector3.zero;

    }
    private float findingTargetTimeInterval = .25f;
    private float findingTargetTimer;
    private void FindingTargetUpdate()
    {
        if(this.target != null
            && (Physics.Raycast(rayCastPos.position
                , (this.target.transform.position - rayCastPos.position).normalized
                , Vector3.Distance(rayCastPos.position, this.target.transform.position)
                , LayerMask.GetMask("Default")) == false)) 
        {
            enemyGetShootDirection.SetTrackingRate(enemyGetShootDirection.trackingTargetRate + Time.deltaTime * enemyGetShootDirection.trackingTargetAccelerate);
            this.targetKnewPos = this.target.transform.position;
        }
        else
        {
            enemyGetShootDirection.SetTrackingRate(enemyGetShootDirection.trackingTargetRate - Time.deltaTime * enemyGetShootDirection.trackingTargetDecelerate);
        }
        

        findingTargetTimer += Time.deltaTime;

        if(findingTargetTimer < findingTargetTimeInterval)
            return;

        if(findingTargetComponent.FindTarget(out GameObject target))
            this.target = target;
        else
            this.target = null;
       
        findingTargetTimer = 0;
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
    private GameObject target;
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
            && bullet.weapon.userWeapon._userWeapon.gameObject.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted i_enemyAITargeted))
        {
            targetKnewPos = i_enemyAITargeted.selfEnemyAIBeenTargeted.transform.position;
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
    public float hitedForceStop;

    [Range(0, 100)]
    public float painStateForceStop;

    public bool isSprintCommand { get; set; }


    #endregion

    #region InitilizedPainState
    public bool _isPainTrigger { get; set; }
    public bool _isInPain { get
        {
            if(enemyStateManagerNode == null)
                return false;

            if(enemyStateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;    

            if(enemyStateManagerNode.TryGetCurNodeLeaf<IGotGunFuAttackNode>()
                || enemyStateManagerNode.TryGetCurNodeLeaf<IGotGunFuExecuteNodeLeaf>())
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

    private void SetDefaultAttribute()
    {
        posture = 100;
        base.HP = 100;
        base.maxHp = 100;
        staggerGauge = maxStaggerGauge;
    }
    private void OnEnable()
    {
        this.SetDefaultAttribute();
        NotifyObserver(this, SubjectEnemy.EnemyEvent.OnEnable);
    }
    private void OnDisable()
    {
        NotifyObserver(this, SubjectEnemy.EnemyEvent.OnDisable);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetKnewPos, 0.14f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.15f);

        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(_transform.position, _transform.forward);
    }

   
}
