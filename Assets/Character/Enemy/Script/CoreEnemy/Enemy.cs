
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy : SubjectEnemy
    , IMotionDriven
    , IFindingTarget
    , ICoverUseable
    , IHeardingAble
    , IPainStateAble
    , IFriendlyFirePreventing
    , ICommunicateAble
    
{


    [SerializeField] public NavMeshAgent agent;

    public LayerMask targetMask;
    public LayerMask targetSpoterMask;
    public FieldOfView enemyFieldOfView;
    public override MovementCompoent _movementCompoent { get ; set ; }
    public EnemyGetShootDirection enemyGetShootDirection;
    public INodeManager enemyStateManagerNode;
    private EnemyCommunicator enemyCommunicator;


    public Vector3 forceSave;

    public float myHP;

    
    [SerializeField] public bool isImortal;
    public Transform rayCastPos;

    public LayerMask selfLayerMask;
  
    public override void Initialized()
    {
        targetMask.value = LayerMask.GetMask("Player");


        enemyFieldOfView = new FieldOfView(120, 225, rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        _isGotAttackedAble = true;
        InitializedBodyPart();
        MotionControlInitailized();
        friendlyFirePreventingBehavior = new FriendlyFirePreventingBehavior(this);
        _movementCompoent = new EnemyMovement(this, transform, this, agent);
        enemyCommunicator = new EnemyCommunicator();
        InitailizedFindingTarget();
        InitailizedCoverUsable();
        InitailizedGunFuComponent();

        enemyStateManagerNode = new EnemyStateManagerNode(this);
        Initialized_IWeaponAdvanceUser();

        this.SetDefaultAttribute();

        AddObserver(this);

        base.Initialized();
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

        switch (damageVisitor)
        {
            case Bullet bullet:
                {
                    TakeDamage(bullet.GetHpDamage);
                    bullet.weapon.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction
                        <IBulletDamageAble>(WeaponAfterAction.WeaponAfterActionSending.HitConfirm, this);
                    NotifyObserver(this, EnemyEvent.GotBulletHit);
                    break;
                }
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                    {

                        if (this.staggerGauge > 0)
                            this.staggerGauge -= gunFuHitNodeLeaf.staggerHitDamage;
                    }
                    break;
                }
        }



    }
    public void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        throw new NotImplementedException();
    }
    private void BlackBoardUpdate()
    {
        isSpottingTaget = this.findingTargetComponent.isSpottingTarget;
        moveInputVelocity_LocalCommand = TransformWorldToLocalVector(moveInputVelocity_WorldCommand, transform.forward);
        if (this.findingTargetComponent.isSpottingTarget && _isInPain == false)
            enemyGetShootDirection.SetTrackingRate(enemyGetShootDirection.trackingTargetRate + (Time.deltaTime * enemyGetShootDirection.trackingTargetAccelerate));
        else
            enemyGetShootDirection.SetTrackingRate(enemyGetShootDirection.trackingTargetRate - (Time.deltaTime * enemyGetShootDirection.trackingTargetDecelerate));

        curTrackRate = enemyGetShootDirection.trackingTargetRate;
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
        _triggerDodge = false;
        isSprintCommand = false;
        moveInputVelocity_WorldCommand = Vector3.zero;

    }
    private float findingTargetTimeInterval = .25f;
    private float findingTargetTimer;
    private void FindingTargetUpdate()
    {
        if(isDead)
            return;

        if(this.target != null
            && (Physics.Raycast(rayCastPos.position
                , (this.target.transform.position - rayCastPos.position).normalized
                , Vector3.Distance(rayCastPos.position, this.target.transform.position)
                , LayerMask.GetMask("Default")) == false)) 
        {
          
            this.targetKnewPos = this.target.transform.position;
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
    #region InitialziedBodyPart
    [SerializeField] public HeadBodyPart head;
    [SerializeField] public ChestBodyPart spline;
    [SerializeField] public ChestBodyPart hip;
    [SerializeField] public LegRightBodyPart right_upper_Leg;
    [SerializeField] public LegRightBodyPart right_lower_Leg;
    [SerializeField] public LegLeftBodyPart left_upper_Leg;
    [SerializeField] public LegLeftBodyPart left_lower_Leg;
    [SerializeField] public ArmRightBodyPart right_upper_Arm;
    [SerializeField] public ArmRightBodyPart right_lower_Arm;
    [SerializeField] public ArmLeftBodyPart left_upper_Arm;
    [SerializeField] public ArmLeftBodyPart left_lower_Arm;

    public void InitializedBodyPart()
    {
        //Head
        head.Initialized();
        //Chest
        spline.Initialized();
        hip.Initialized();
        //Legs
        right_upper_Leg.Initialized();
        right_lower_Leg.Initialized();
        left_upper_Leg.Initialized();
        left_lower_Leg.Initialized();
        //Arm
        right_lower_Arm.Initialized();
        right_upper_Arm.Initialized();
        left_lower_Arm.Initialized();
        left_upper_Arm.Initialized();
    }
    #endregion
    #region InitializedMotionControl


    public List<GameObject> bones { get; set; }
    public GameObject hips { get; set; }
    Animator IMotionDriven.animator { get => animator; set => animator = value; }
    public MotionControlManager motionControlManager { get; set; }
    public void MotionControlInitailized()
    {
        hips = this.hip.gameObject;
        bones = new List<GameObject>();
        bones.Add(head.gameObject);
        bones.Add(spline.gameObject);
        bones.Add(hip.gameObject);
        bones.Add(right_upper_Leg.gameObject);
        bones.Add(right_lower_Leg.gameObject);
        bones.Add(left_upper_Leg.gameObject);
        bones.Add(left_lower_Leg.gameObject);
        bones.Add(right_upper_Arm.gameObject);
        bones.Add(right_lower_Arm.gameObject);
        bones.Add(left_upper_Arm.gameObject);
        bones.Add(left_lower_Arm.gameObject);

        motionControlManager = new MotionControlManager(bones, hips, animator);
    }
    #endregion

    #region InitailizedFindingTarget

    public FindingTarget findingTargetComponent { get; set; }
    public Vector3 targetKnewPos;
    public GameObject target { get; set; }
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
        enemyCommunicator.SendCommunicate(transform.position, 10, selfLayerMask, EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition,targetKnewPos);
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
    public void GetCommunicate<TypeCommunicator,T>(TypeCommunicator typeCommunicator,T var) where TypeCommunicator : Communicator
    {

        if (isDead)
            return;


        if (typeCommunicator is EnemyCommunicator enemyCommunicator)
        {
            switch (enemyCommunicator.enemyCommunicateMassage)
            {
                case EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition:
                    {
                        if (var is Vector3 targetSendedPosition)
                            targetKnewPos = targetSendedPosition;
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

    [Range(0, 100)]
    public float CrouchMoveAccelerate;
    [Range(0, 100)]
    public float CrouchMoveMaxSpeed;
    [Range(0, 100)]
    public float CrouchMoveRotateSpeed;

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

    [Range(0, 100)]
    public float dodgeImpluseForce;
    [Range(0, 100)]
    public float dodgeInAirStopForce;
    [Range(0, 100)]
    public float dodgeOnGroundStopForce;

    public bool isSprintCommand { get; set; }
    public bool _triggerDodge { get; set; }

    public Stance enemyStance = Stance.stand;


    #endregion

    #region InitilizedPainState
    [Range(0, 10)]
    public float miniPainStateDuration;
    [Range(0, 10)]
    public float mediumPainStateDuration;
    [Range(0, 10)]
    public float heavyPainStateDuration;
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

    #endregion

    public Action<IDamageVisitor> NotifyGotAttack;

   
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
        this._posture = 100;
        base.HP = 100;
        base.maxHp = 100;
        staggerGauge = maxStaggerGauge;

        targetKnewPos = transform.position + transform.forward + Vector3.up;

        enemyGetShootDirection.HardSetPointingPos(transform.position + transform.forward +Vector3.up);
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);

        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(_transform.position, _transform.forward);
    }

   
}
