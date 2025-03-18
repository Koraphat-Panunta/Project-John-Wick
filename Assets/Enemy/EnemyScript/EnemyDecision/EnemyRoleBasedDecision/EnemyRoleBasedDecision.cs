using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyRoleBasedDecision : EnemyDecision,IEnemyActionNodeManagerImplementDecision,IObserverEnemy
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    public EnemyActionNodeManager enemyActionNodeManager { get ;private set ; }
    public EnemyChaserRoleNodeManager chaserRoleNodeManager { get ;private set ; }
    public EnemyOverwatchRoleNodeManager overwatchRoleNodeManager { get ;private set ; }
    public ZoneDefine _targetZone { get ; set ; }

    [Range(0, 100)]
    [SerializeField]private float raduisTargetZone;
    public IEnemyActionNodeManagerImplementDecision.CombatPhase _curCombatPhase { get ; set ; }
    public IEnemyActionNodeManagerImplementDecision.CombatPhase CombatPhase;

    [Range(0,100)]
    [SerializeField]private float YingYang;
    public float _yingYang { get => YingYang; set => YingYang = value; }
    public bool _takeCoverAble { get ; set ; }
    [SerializeField] bool TakeCoverAble;
    private float takeCoverAbleDelay { get; set ; }

    [SerializeField] private float enegyWithIn;
    [SerializeField] private float YingYangCalulate;

    [SerializeField] private float elapesLostSightTime;
    [SerializeField] private float lostSightTime;
    

    public enum Role
    {
        Chaser,
        Overwatch
    }
    public Role startRole;

    protected override void Awake()
    {
        base.Awake();
        enemy.AddObserver(this);
        _targetZone = new ZoneDefine(Vector3.zero, 8.5f);

        chaserRoleNodeManager = new EnemyChaserRoleNodeManager(enemy,enemyCommand,this,5,8.5f);
        chaserRoleNodeManager.InitailizedNode();

        overwatchRoleNodeManager = new EnemyOverwatchRoleNodeManager(enemy, enemyCommand, this, 5, 8.5f);
        overwatchRoleNodeManager.InitailizedNode();

        switch (startRole)
        {
            case Role.Chaser:enemyActionNodeManager = chaserRoleNodeManager; 
                break;
            case Role.Overwatch:enemyActionNodeManager= overwatchRoleNodeManager; 
                break;
        }

        enemy.NotifyCommunicate += OnNotifyGetCommunicate;
        enemyCommand = GetComponent<EnemyCommandAPI>();
        _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill;

        lostSightTime = YingYang / 10;
        _takeCoverAble = true;

    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        enemyActionNodeManager.UpdateNode();
        base.Update();
        enegyWithIn = enemyActionNodeManager.enegyWithIn;
        YingYangCalulate = enemyActionNodeManager.yingYangCalculate;

        if (_curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert)
        {
            this.elapesLostSightTime += Time.deltaTime;
            if (this.elapesLostSightTime >= this.lostSightTime)
                _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware;
        }

        this.CombatPhase = _curCombatPhase;
        this.TakeCoverAble = _takeCoverAble;
        if(_takeCoverAble == false)
        {
            takeCoverAbleDelay -= Time.deltaTime;
            if(takeCoverAbleDelay <=0)
                _takeCoverAble = true;
        }
    }
    protected override void FixedUpdate()
    {
        enemyActionNodeManager.FixedUpdateNode();
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if (_curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert)
            return;
        if (noiseMaker is Bullet bullet
            && bullet.weapon.userWeapon.userWeapon.gameObject.TryGetComponent<I_NPCTargetAble>(out I_NPCTargetAble i_NPCTargetAble))
        {
            _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware;
            _targetZone.SetZone(noiseMaker.position, raduisTargetZone);
        }
    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert;
        _targetZone.SetZone(target.transform.position, raduisTargetZone);

        elapesLostSightTime = 0;
    }

    private void OnNotifyGetCommunicate(Communicator communicator)
    {
        if (communicator is EnemyCommunicator enemyCommunicator == false)
            return;
        switch (enemyCommunicator.enemyCommunicateMassage)
        {
            case EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition:
                {
                    if (_curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert)
                        return;
                    _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware;
                    _targetZone.SetZone(enemy.targetKnewPos, raduisTargetZone);
                }
                break;
        }
    }
    private void OnDrawGizmos()
    {
        //DrawTargetZoneDefine
        if(Application.isEditor)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_targetZone.zonePosition, _targetZone.raduis);

        //DrawGuardingZone
        if (chaserRoleNodeManager.curNodeLeaf == chaserRoleNodeManager.guardingEnemyActionNodeLeaf)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(chaserRoleNodeManager.guardingEnemyActionNodeLeaf.guardingZone.zonePosition
                , chaserRoleNodeManager.guardingEnemyActionNodeLeaf.guardingZone.raduis);
        }

    }

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if(enemyEvent == SubjectEnemy.EnemyEvent.GotHit
            ||enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotHit
            || enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotInteract
            || enemyEvent == SubjectEnemy.EnemyEvent.Dead)
        {
            _takeCoverAble = false;
            takeCoverAbleDelay = 5;
        }
    }

    public void ChangeRole(EnemyActionNodeManager roleEnemy)
    {
        enemyActionNodeManager = roleEnemy; 
    }
   


    
}

