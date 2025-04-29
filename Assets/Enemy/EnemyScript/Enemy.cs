using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser, IMotionDriven,
     IFindingTarget, ICoverUseable,
    IHeardingAble, IPatrolComponent,
    IPainStateAble, IFallDownGetUpAble,
    IGunFuGotAttackedAble, IFriendlyFirePreventing,
    IThrowAbleObjectVisitable, ICommunicateAble
    , IBulletDamageAble,IGunFuAble
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

    public EnemyGetShootDirection enemyGetShootDirection;
    public EnemyMovement enemyMovement;
    public EnemyStateManagerNode enemyStateManagerNode;
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
        enemyFieldOfView = new FieldOfView(120, 225, rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        enemyMovement = new EnemyMovement(agent, this);

        _isGotAttackedAble = true;

        MotionControlInitailized();
        Initialized_IWeaponAdvanceUser();
        InitailizedFindingTarget();
        InitailizedCoverUsable();
        InitailizedGunFuComponent();
        friendlyFirePreventingBehavior = new FriendlyFirePreventingBehavior(this);

        enemyCommunicator = new EnemyCommunicator(this);

        posture = 100;

        base.HP = 100;
        base.maxHp = 100;
        enemyStateManagerNode = new EnemyStateManagerNode(this);
        if(startWeapon == null)
        startWeapon = Instantiate(startWeapon);
        startWeapon.AttatchWeaponTo(this);

    }


    void Update()
    {
        myHP = base.HP;
        findingTargetComponent.FindTarget(out GameObject target);
        //combatOffensiveInstinct.UpdateSening();
        enemyStateManagerNode.UpdateNode();
        weaponManuverManager.UpdateNode();
        enemyMovement.MovementUpdate();

    }
    private void LateUpdate()
    {
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
        if (isImortal == false)
            HP -= Damage;

        if (base.HP <= 0 && isImortal == false)
        {
            NotifyObserver(this, EnemyEvent.Dead);
        }
    }
    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        if (damageVisitor is Bullet bullet)
        {
            TakeDamage(bullet.hpDamage);
            bullet.weapon.userWeapon.weaponAfterAction.HitDamageAble(this);
        }


    }
    public void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        throw new NotImplementedException();
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
        isPickingUpWeaponCommand = false;
        isPullTriggerCommand = false;
        _triggerGunFu = false;

    }


    #region Initailized WeaponAdvanceUser
    [SerializeField] private Transform weaponMainSocket;
    [SerializeField] private Transform primaryWeaponHoster;
    [SerializeField] private Transform secondaryWeaponHoster;

    public bool isSwitchWeaponCommand { get; set; }
    public bool isPullTriggerCommand { get; set; }
    public bool isAimingCommand { get; set; }
    public bool isReloadCommand { get; set; }
    public bool isPickingUpWeaponCommand { get; set; }
    public bool isDropWeaponCommand { get; set; }
    public Animator weaponUserAnimator { get; set; }
    public Weapon _currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }
    public Vector3 shootingPos {
        get { return enemyGetShootDirection.GetShootingPos(); }
        set { }
    }
    public Vector3 pointingPos { get => enemyGetShootDirection.GetPointingPos();
        set { } }

    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public Character userWeapon => this;

    [SerializeField] AnimatorOverrideController AnimatorOverrideController;
    public AnimatorOverrideController _animatorOverride { get; set; }
    public WeaponManuverManager weaponManuverManager { get; set; }
    public FindingWeaponBehavior findingWeaponBehavior { get; set; }

    public void Initialized_IWeaponAdvanceUser()
    {
        weaponUserAnimator = animator;
        currentWeaponSocket = weaponMainSocket;
        weaponBelt = new WeaponBelt(primaryWeaponHoster, secondaryWeaponHoster, new AmmoProuch(1000, 1000, 1000, 1000
            , 1000, 1000, 1000, 1000));
        weaponAfterAction = new WeaponAfterActionEnemy(this);
        weaponCommand = new WeaponCommand(this);
        weaponManuverManager = new EnemyWeaponManuver(this, this);
        findingWeaponBehavior = new FindingWeaponBehavior(this);
        _animatorOverride = this.AnimatorOverrideController;
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
            && bullet.weapon.userWeapon.userWeapon.gameObject.TryGetComponent<I_NPCTargetAble>(out I_NPCTargetAble i_NPCTargetAble))
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

    #region IBulletDamageAble
    public IBulletDamageAble bulletDamageAbleBodyPartBehavior { get; set; }
    public Action<IDamageVisitor> NotifyGotAttack;
    #endregion
    #region ImplementGunFuAble
    public bool _triggerGunFu { get; set ; }
    public bool _triggerExecuteGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float triggerGunFuBufferTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Transform _gunFuUserTransform { get => transform; set { } }
    public Transform _targetAdjustTranform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Vector3 _gunFuAimDir { get => transform.forward; set { } }
    public GunFuDetectTarget gunFuDetectTarget { get ; set ; }
    public LayerMask _layerTarget { get => targetMask; set { } }
    public IGunFuGotAttackedAble attackedAbleGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IGunFuGotAttackedAble executedAbleGunFu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IGunFuNode curGunFuNode { get 
        {
            if (enemyStateManagerNode.curNodeLeaf is IGunFuNode gunFuNode)
                return gunFuNode;
            return null;

        } set { } }
    public StackGague gunFuExecuteStackGauge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [SerializeField] public EnemySpinKickScriptable EnemySpinKickScriptable;
    public void InitailizedGunFuComponent()
    {
        _gunFuUserTransform = transform;
    }

    public void UpdateDetectingTarget()
    {
       throw new NotImplementedException();
    }
    #endregion
    #region ImplementGunFuGotHitAble
    public bool _triggerHitedGunFu { get; set; }
    public Vector3 attackedPos { get; set; }
    public Transform _gunFuAttackedAble { get { return transform; } set { } }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get => enemyStateManagerNode.curNodeLeaf; set { } }
    public IMovementCompoent _movementCompoent { get => this.enemyMovement; set { } }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => this; set { } }
    public IDamageAble _damageAble { get => this; set { } }
    bool IGunFuGotAttackedAble._isDead { get => this.isDead; set { } }
    public bool _isGotAttackedAble { get
        {
            if (curNodeLeaf is KnockDown_GunFuNode)
                return false;
            if (curNodeLeaf is HumandThrow_GotInteract_NodeLeaf)
                return false;
            if (curNodeLeaf is FallDown_EnemyState_NodeLeaf fallNode)
            {
                return false;
            }
            if(curNodeLeaf is EnemySpinKickGunFuNodeLeaf)
                return false;

            return true;
        } set { } }

    public bool _isGotExecutedAble { 
        get 
        {
            if(curNodeLeaf is FallDown_EnemyState_NodeLeaf)
                return true;
            return false;
        } set { } }

    [SerializeField] public GunFu_GotHit_ScriptableObject GotHit1;
    [SerializeField] public GunFu_GotHit_ScriptableObject GotHit2;
    [SerializeField] public GunFu_GotHit_ScriptableObject KnockDown;
    [SerializeField] public GotRestrictScriptableObject gotRestrictScriptableObject;
    [SerializeField] public WeaponGotDisarmedScriptableObject primary_WeaponGotDisarmedScriptableObject;
    [SerializeField] public WeaponGotDisarmedScriptableObject secondary_WeaponGotDisarmedScriptableObject;

    [SerializeField] public AnimationClip layUpExecutedAnim;
    [SerializeField] public AnimationClip layDownExecutedAnim;
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

    private void OnValidate()
    {
        gunFuDetectTarget = GetComponent<GunFuDetectTarget>();
    }
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
