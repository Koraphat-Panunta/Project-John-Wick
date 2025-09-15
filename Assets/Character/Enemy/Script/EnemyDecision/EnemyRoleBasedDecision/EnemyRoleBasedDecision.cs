using System;
using UnityEngine;
using static SubjectEnemy;

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
    public IEnemyActionNodeManagerImplementDecision.CombatPhase CombatPhase;//Let Editor See

    public bool _takeCoverAble { get ; set ; }
    public EnemyDecision _enemyDecision { get => this; set { } }
    [SerializeField] bool TakeCoverAble;
    private float takeCoverAbleDelay { get; set ; }


    [SerializeField] private float elapesLostSightTime;
    [SerializeField] private float lostSightTime;
    

    public enum Role
    {
        Chaser,
        Overwatch
    }
    public Role startRole;

    public override void Initialized()
    {
        enemy.AddObserver(this);
        _targetZone = new ZoneDefine(Vector3.zero, 10f);

        chaserRoleNodeManager = new EnemyChaserRoleNodeManager(enemy, enemyCommand, this, 2.5f, 5f);
        chaserRoleNodeManager.InitailizedNode();

        overwatchRoleNodeManager = new EnemyOverwatchRoleNodeManager(enemy, enemyCommand, this, 5, 8.5f);
        overwatchRoleNodeManager.InitailizedNode();

        switch (startRole)
        {
            case Role.Chaser:
                enemyActionNodeManager = chaserRoleNodeManager;
                break;
            case Role.Overwatch:
                enemyActionNodeManager = overwatchRoleNodeManager;
                break;
        }

        enemy.NotifyCommunicate += OnNotifyGetCommunicate;
        enemyCommand = GetComponent<EnemyCommandAPI>();
        _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill;

        lostSightTime = 8;
        _takeCoverAble = true;
        base.Initialized();
    }
   
    protected override void Update()
    {
        enemyActionNodeManager.UpdateNode();
        base.Update();

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
        UpdateDebugRole();
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if (_curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert)
            return;



        if (noiseMaker is Bullet bullet
            && bullet.weapon.userWeapon._userWeapon.gameObject.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted i_NPCTargetAble))
        {
            if (_curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill)
                _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Suspect;
            else
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
    [SerializeField] private bool isEnableDrawGizmosDebug;
    private void OnDrawGizmos()
    {
        //DrawTargetZoneDefine
        //if (Application.isPlayer ==false)
        //    return;
        if(isEnableDrawGizmosDebug == false)
            return;


        if (enemyActionNodeManager == chaserRoleNodeManager && chaserRoleNodeManager.curNodeLeaf == chaserRoleNodeManager.approuchingTargetEnemyActionNodeLeaf)
        {
            if(chaserRoleNodeManager.approuchingTargetEnemyActionNodeLeaf.curvePath._curvePoint.Count > 0)
            {
                Gizmos.color = Color.red * 0.5f;
                for(int i = 0;i< chaserRoleNodeManager.approuchingTargetEnemyActionNodeLeaf.curvePath._markPoint.Count; i++)
                {
                    Gizmos.DrawSphere(chaserRoleNodeManager.approuchingTargetEnemyActionNodeLeaf.curvePath._markPoint[i],0.2f);
                    if (i > 0)
                        Gizmos.DrawLine(chaserRoleNodeManager.approuchingTargetEnemyActionNodeLeaf.curvePath._markPoint[i - 1]
                            , chaserRoleNodeManager.approuchingTargetEnemyActionNodeLeaf.curvePath._markPoint[i]);
                }
            }
        }
        if(overwatchRoleNodeManager.curNodeLeaf == overwatchRoleNodeManager.swarpCombatPositionActionNodeLeaf)
        {
            Gizmos.color = Color.blue * 0.5f;
            Gizmos.DrawSphere(overwatchRoleNodeManager.swarpCombatPositionActionNodeLeaf.swarpPosition, .25f);
            Gizmos.DrawLine(enemy.transform.position, overwatchRoleNodeManager.swarpCombatPositionActionNodeLeaf.swarpPosition);
           
        }

    }

    

    public void ChangeRole(EnemyActionNodeManager roleEnemy)
    {
        if (enemyActionNodeManager != null)
            enemyActionNodeManager.Exit();
        enemyActionNodeManager = roleEnemy;
        enemyActionNodeManager.Enter();
    }

    public void Notify<T>(Enemy enemy, T node) 
    {
        if (node is EnemyEvent enemyEvent 
            && enemyEvent == SubjectEnemy.EnemyEvent.GotBulletHit)
        {
            _takeCoverAble = false;
            takeCoverAbleDelay = 5;
        }

        if (node is EnemyStateLeafNode enemyStateLeafNode)
            switch (enemyStateLeafNode)
            {
                case IGotGunFuAttackNode:
                case EnemyDeadStateNode:
                    {
                        _takeCoverAble = false;
                        takeCoverAbleDelay = 5;
                        break;
                    }
            }
    }

    #region DebugRole
    private void UpdateDebugRole()
    {
        this.curRole = this.enemyActionNodeManager.ToString();
        this.curAction = this.enemyActionNodeManager.curNodeLeaf.ToString();
        this.approuchCoolDown = chaserRoleNodeManager.approuchCoolDown;
    }

  

    [SerializeField] private string curRole;
    [SerializeField] private string curAction;
    [SerializeField] private float approuchCoolDown;
    #endregion
}

