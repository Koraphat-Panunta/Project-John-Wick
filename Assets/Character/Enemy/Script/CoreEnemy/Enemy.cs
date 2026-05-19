
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

    public LocalServiceLocator localServiceLocator;
    public FieldOfView enemyFieldOfView;
    public override MovementCompoent _movementCompoent { get ; set ; }
    public EnemyGetShootDirection enemyGetShootDirection;
    public INodeManager stateManagerNode;
    public EnemyStateManagerNode enemyStateManagerNode => this.stateManagerNode as EnemyStateManagerNode;
    private EnemyCommunicator enemyCommunicator;

    public AIAgent agent;

    public Vector3 forceSave;

    
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

    public override Gauge _hpGauge { get ; protected set ; }

    public override void Initialized()
    {
        Debug.Log("enemy Initialized" + this.gameObject);
        this._hpGauge = new Gauge(this.enemyStatsScripableObject.maxHp,this.enemyStatsScripableObject.maxHp);
        this.postureGauge = new Gauge(this.enemyStatsScripableObject.maxPosture,this.enemyStatsScripableObject.maxPosture);
        this.reactionTime = new Gauge(this.enemyStatsScripableObject.reactionTime, this.enemyStatsScripableObject.reactionTime);
        InitializeGuardSystem();

        enemyFieldOfView = new FieldOfView(120, 225, rayCastPos.transform);
        enemyGetShootDirection = new EnemyGetShootDirection(this);

        _isGotAttackedAble = true;
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
        stateManagerNode.UpdateNode();
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
        stateManagerNode.FixedUpdateNode();
        _weaponManuverManager.FixedUpdateNode();
        _movementCompoent.FixedUpdateNode();
    }

    public void TakeDamage(float Damage)
    {
        if(this.isImortal)
            SetHP(Mathf.Clamp(this.GetHP() - Damage, 1, this.GetMaxHp()));
        else
        SetHP(Mathf.Clamp(this.GetHP() - Damage, 0, this.GetMaxHp()));
        
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

                        this._posture = Mathf.Clamp(this._posture - gunFuHitDownNodeLeaf._postureDamageVisitor, 1, this._maxPosture);
                    }
                    if(gunFuHitDownNodeLeaf.gunFuHitDownPhase == GunFuHitDownNodeLeaf.GunFuHitDownPhase.PullUp)
                    {
                        this._posture = Mathf.Clamp(this._maxPosture, 0, this._maxPosture);
                    }

                    gunFuHitDownNodeLeaf.OnNotifyFeedBackVisitor(this);

                    return;
                }
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                    {
                        Vector3 hitDir = gunFuHitNodeLeaf.gunFuAble._character._movementCompoent.curPosition - this._movementCompoent.curPosition;
                        hitDir = new Vector3(hitDir.x, this._movementCompoent.curPosition.y , hitDir.z).normalized;

                        if(this.guardGauge._gauge > 0 
                            && this.isGuardModeEnabled
                            && Vector3.Dot(hitDir,this.transform.forward) > 0) // Block
                        {
                            this.guardGauge.AddGauge(-gunFuHitNodeLeaf._hPDamage);
                            this._triggerBlock = true;
                            gunFuHitNodeLeaf.OnNotifyFeedBackVisitor(this);
                            return;
                        }

                        this.enemyStateManagerNode.gotGunFuHitNodeLeaf.SetPainTime(gunFuHitNodeLeaf.stuntingTime);

                        if (gunFuHitNodeLeaf._stateName == GunFuManaverStateName.Hit3.ToString())
                        {
                            this._posture = Mathf.Clamp(this._posture - gunFuHitNodeLeaf._postureDamageVisitor, 0, this._maxPosture);
                            this.TakeDamage(gunFuHitNodeLeaf._hPDamage);
                        }
                        else
                        {
                            if (this.GetHP() > this.gotHitWithStandHP)
                            {
                                this.TakeDamage(Mathf.Clamp(gunFuHitNodeLeaf._hPDamage, 0, this.GetHP() - this.gotHitWithStandHP));
                            }

                            if (this._posture > 0)
                            {
                                this._posture = Mathf.Clamp(this._posture - gunFuHitNodeLeaf._postureDamageVisitor, 1, this._maxPosture);
                            }
                        }

                       

                        gunFuHitNodeLeaf.OnNotifyFeedBackVisitor(this);

                    }
                    return;
                }
            case ExecuteMethod gunFuMethod:
                {
                    this.TakeDamage(this.GetHP());
                    break;
                }
        }

        if (damageVisitor is IHPDamageVisitor hPDamageVisitor)
            this.TakeDamage(hPDamageVisitor._hPDamage);

        if (damageVisitor is IPostureDamageVisitor postureDamageVisitor)
            this.TakePostureDamaged(postureDamageVisitor._postureDamageVisitor);

        damageVisitor.OnNotifyFeedBackVisitor(this);


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
        _triggerAttack = false;
        _triggerDodge = false;
        isSprintCommand = false;
        isTriggerMeleeWeaponAttack = false;
        _triggerEvade = false;
        _triggerBlock = false;
        moveInputVelocity_WorldCommand = Vector3.zero;

    }
  

    #region InitializedMotionControl


    public List<GameObject> bones { get; set; }
    public GameObject hips { get; set; }
    Animator IMotionDriven.animator { get => animator; set => animator = value; }
    public MotionControlManager motionControlManager { get; set; }
    public void MotionControlInitailized()
    {
        FullBodyCharacterPart fullBodyCharacterPart = this.localServiceLocator.Get<FullBodyCharacterPart>();

        hips = fullBodyCharacterPart.hipBodyPart.gameObject;
        bones = new List<GameObject>();
        bones.Add(fullBodyCharacterPart.headBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.spline_0BodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.hipBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.upperLegRightBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.lowerLegRightBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.upperLegLeftBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.lowerLegLeftBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.armRightBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.foreArmRightBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.armLeftBodyPart.gameObject);
        bones.Add(fullBodyCharacterPart.foreArmLeftBodyPart.gameObject);

        motionControlManager = new MotionControlManager(bones, hips, animator);
    } 
    #endregion

    #region InitailizedFindingTarget

    public Vector3 targetKnowPos => this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.targetKnewPos;

    public Action<GameObject> NotifyEnemySpottingTarget;
    public Transform target 
    { 
        get => this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.target ;
        set => this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.target = value; 
    }
    public void SetTargetKnowPos(Vector3 targetKnowPos)
    {
        this.enemyStateManagerNode.findAndTrackTargetNodeLeaf.SetTargetKnowPos(targetKnowPos);
    }
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

        this.NotifyObserver<FindiAndTrackingTargetNodeLeaf>(this, this.enemyStateManagerNode.findAndTrackTargetNodeLeaf);
    }

    #endregion

    #region InitailizedHearingComponent

    public Action<INoiseMakingAble> NotifyGotHearing { get; set; }
    public void GotHearding(INoiseMakingAble noiseMakingAble)
    {
        if (isDead)
            return;

        if (noiseMakingAble is Bullet bullet
            && bullet.weapon.userWeapon._character.gameObject.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted i_enemyAITargeted))
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

            if(stateManagerNode.TryGetCurNodeLeaf<IGotParriedNode>())
                return true;

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
        try
        {
            this._posture = this._maxPosture;
            this.SetHP(this.GetMaxHp());
            ResetGuardSystem();
            enemyGetShootDirection.HardSetPointingPos(transform.position + transform.forward + Vector3.up);
        }
        catch
        {
            
        }

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
