
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy : SubjectEnemy
    , IMotionDriven
    , IHeardingAble
    , IPainStateAble
    , IFriendlyFirePreventing
    , ICommunicateAble
    
{


    public FieldOfView enemyFieldOfView;
    public override MovementCompoent _movementCompoent { get ; set ; }
    public EnemyGetShootDirection enemyGetShootDirection;
    public INodeManager stateManagerNode;
    public EnemyStateManagerNode enemyStateManagerNode => this.stateManagerNode as EnemyStateManagerNode;
    private EnemyCommunicator enemyCommunicator;

    public AIAgent agent;

    public Vector3 forceSave;

    public float myHP;

    
    [SerializeField] public bool isImortal;
    [SerializeField] public bool isNotFallAble;
    public Transform rayCastPos;

    public LayerMask selfLayerMask;

    public override Stance stance 
    {
        get 
        {
            if(this.stateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>()
                ||(this.stateManagerNode.TryGetCurNodeLeaf<GetUpStateNodeLeaf>(out GetUpStateNodeLeaf getUpStateNodeLeaf)
                && getUpStateNodeLeaf.isStandingComplete == false))
                return Stance.prone;

            return Stance.stand;
        }
    }
    public Stance stanceCommand = Stance.stand;

    public override void Initialized()
    {


        enemyFieldOfView = new FieldOfView(120, 225, rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        _isGotAttackedAble = true;
        InitializedBodyPart();
        MotionControlInitailized();
        friendlyFirePreventingBehavior = new FriendlyFirePreventingBehavior(this);
        _movementCompoent = new EnemyMovement(this, transform, this, this.characterController);
        enemyCommunicator = new EnemyCommunicator();

        InitailizedGunFuComponent();

        stateManagerNode = new EnemyStateManagerNode(this);
        InitailizedFindingTarget();
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
        stateManagerNode.UpdateNode();
        _weaponManuverManager.UpdateNode();
        _movementCompoent.UpdateNode();


    }
    private void LateUpdate()
    {
        this.posture = this._posture;
        BlackBoardUpdate();
        BlackBoardBufferUpdate();

    }

    private void FixedUpdate()
    {
        stateManagerNode.FixedUpdateNode();
        _weaponManuverManager.FixedUpdateNode();
        _movementCompoent.FixedUpdateNode();
    }

    public void TakeDamage(float Damage)
    {
        if(this.isImortal)
            SetHP(Mathf.Clamp(HP - Damage, 1, maxHp));
        else
        SetHP(Mathf.Clamp(HP - Damage, 0, maxHp));
        
    }
    private float gotHitWithStandHP = 20;
    public void TakeDamage(IDamageVisitor damageVisitor)
    {
        switch (damageVisitor)
        {
            case GunFuHitDownNodeLeaf gunFuHitDownNodeLeaf:
                {
                    if(gunFuHitDownNodeLeaf.gunFuHitDownPhase == GunFuHitDownNodeLeaf.GunFuHitDownPhase.Attack)
                    {
                        if (this.GetHP() > this.gotHitWithStandHP)
                        {
                            this.TakeDamage(Mathf.Clamp(gunFuHitDownNodeLeaf._hPDamage, 0 , this.GetHP() - this.gotHitWithStandHP));
                        }
                        else
                            this.TakeDamage(gunFuHitDownNodeLeaf._hPDamage);

                        this._posture = Mathf.Clamp(this._posture - gunFuHitDownNodeLeaf._postureDamageVisitor, 1, this._maxPosture);
                    }
                    if(gunFuHitDownNodeLeaf.gunFuHitDownPhase == GunFuHitDownNodeLeaf.GunFuHitDownPhase.PullUp)
                    {
                        this._posture = Mathf.Clamp(this.maxPosture, 0, this._maxPosture);
                    }
                    return;
                }
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                    {
                        this.enemyStateManagerNode.gotGunFuHitNodeLeaf.SetPainTime(gunFuHitNodeLeaf.stuntingTime);

                        if (this.GetHP() > this.gotHitWithStandHP)
                        {
                            this.TakeDamage(Mathf.Clamp(gunFuHitNodeLeaf._hPDamage, 0 , this.GetHP() - this.gotHitWithStandHP));
                        }
                        else
                            this.TakeDamage(gunFuHitNodeLeaf._hPDamage);

                        if (gunFuHitNodeLeaf._stateName == GunFuManaverStateName.Hit3.ToString())
                        {
                            this._posture = Mathf.Clamp(this._posture - gunFuHitNodeLeaf._postureDamageVisitor, 0, this._maxPosture);
                        }
                        else
                        {
                            this._posture = Mathf.Clamp(this._posture - gunFuHitNodeLeaf._postureDamageVisitor, 1, this._maxPosture);
                        }

                    }
                    return;
                }
        }

        if (damageVisitor is IHPDamageVisitor hPDamageVisitor)
            this.TakeDamage(hPDamageVisitor._hPDamage);

        if (damageVisitor is IPostureDamageVisitor postureDamageVisitor)
            this.TakePostureDamaged(postureDamageVisitor._postureDamageVisitor);


    }
    public void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        throw new NotImplementedException();
    }
    private void BlackBoardUpdate()
    {
        this.isSpottingTaget = this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.isSpottingTarget;
        this.moveInputVelocity_LocalCommand = TransformWorldToLocalVector(this.moveInputVelocity_WorldCommand, this.transform.forward);
        if (this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.isSpottingTarget && _isInPain == false)
            this.enemyGetShootDirection.SetTrackingRate(this.enemyGetShootDirection.trackingTargetRate + (Time.deltaTime * this.enemyGetShootDirection.trackingTargetAccelerate));
        else
            this.enemyGetShootDirection.SetTrackingRate(this.enemyGetShootDirection.trackingTargetRate - (Time.deltaTime * this.enemyGetShootDirection.trackingTargetDecelerate));

        this.curTrackRate = this.enemyGetShootDirection.trackingTargetRate;
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

    public Vector3 targetKnowPos => this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.targetKnewPos;

    public Action<GameObject> NotifyEnemySpottingTarget;
    public Transform target => this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.target;
    public void InitailizedFindingTarget()
    {
        
        this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.OnSpottingTarget += EnemySpotingTarget;

    }
    private void EnemySpotingTarget(GameObject target)
    {

        if (isDead)
            return;

        this.enemyCommunicator.SendCommunicate(transform.position, 10, selfLayerMask, EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition,this.targetKnowPos);
        if (NotifyEnemySpottingTarget != null)
            NotifyEnemySpottingTarget.Invoke(target);
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
            this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.SetTargetKnowPos(i_enemyAITargeted.selfEnemyAIBeenTargeted.transform.position);
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
                            this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.SetTargetKnowPos(targetSendedPosition);
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

    

    [Range(0, 100)]
    public float dodgeImpluseForce;
    [Range(0, 100)]
    public float dodgeInAirStopForce;
    [Range(0, 100)]
    public float dodgeOnGroundStopForce;

    public bool isSprintCommand { get; set; }
    public bool _triggerDodge { get; set; }


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
            if(stateManagerNode == null)
                return false;

            if(stateManagerNode.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
                return true;    

            if(stateManagerNode.TryGetCurNodeLeaf<IGotGunFuAttackNode>()
                || stateManagerNode.TryGetCurNodeLeaf<IGotGunFuExecuteNodeLeaf>())
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
        this._posture = this._maxPosture;
        base.HP = 100;
        base.maxHp = 100;

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
        try
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.targetKnowPos, 0.14f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.15f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward);
        }
        catch { }

        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(_transform.position, _transform.forward);
    }

   
}
