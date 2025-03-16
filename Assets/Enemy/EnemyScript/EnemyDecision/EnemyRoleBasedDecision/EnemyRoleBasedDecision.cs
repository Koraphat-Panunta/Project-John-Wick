using UnityEngine;

public class EnemyRoleBasedDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    public EnemyActionNodeManager enemyActionNodeManager { get ;private set ; }
    public EnemyChaserRoleNodeManager chaserRoleNodeManager { get ;private set ; }
    public ZoneDefine targetZoneDefine { get ; set ; }

    public enum StartRole
    {
        Chaser,
        Overwatch
    }
    public StartRole startRole;

    protected override void Awake()
    {
        base.Awake();

        chaserRoleNodeManager = new EnemyChaserRoleNodeManager(enemy,enemyCommand,this);
        chaserRoleNodeManager.InitailizedNode();

        switch (startRole)
        {
            case StartRole.Chaser:enemyActionNodeManager = chaserRoleNodeManager; 
                break;
        }

        targetZoneDefine = new ZoneDefine(Vector3.zero,8.5f);
        enemy.NotifyCommunicate += OnNotifyGetCommunicate;
        enemyCommand = GetComponent<EnemyCommandAPI>();
        curCombatPhase = CombatPhase.Chill;
        pressure = 0;
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        enemyActionNodeManager.UpdateNode();
        base.Update();
    }
    protected override void FixedUpdate()
    {
        enemyActionNodeManager.FixedUpdateNode();
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if (curCombatPhase == CombatPhase.Alert)
            return;

        curCombatPhase = CombatPhase.Aware;
        targetZoneDefine.SetZone(noiseMaker.position);

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        curCombatPhase = CombatPhase.Alert;
        targetZoneDefine.SetZone(target.transform.position);
    }

    private void OnNotifyGetCommunicate(Communicator communicator)
    {
        if (communicator is EnemyCommunicator enemyCommunicator == false)
            return;
        switch (enemyCommunicator.enemyCommunicateMassage)
        {
            case EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition:
                {
                    if (curCombatPhase == CombatPhase.Alert)
                        return;
                    curCombatPhase = CombatPhase.Aware;
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
        Gizmos.DrawWireSphere(targetZoneDefine.zonePosition, targetZoneDefine.raduis);

        //DrawGuardingZone
        if (chaserRoleNodeManager.curNodeLeaf == chaserRoleNodeManager.guardingEnemyActionNodeLeaf)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(chaserRoleNodeManager.guardingEnemyActionNodeLeaf.guardingZone.zonePosition
                , chaserRoleNodeManager.guardingEnemyActionNodeLeaf.guardingZone.raduis);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(chaserRoleNodeManager.guardingEnemyActionNodeLeaf.destinate, 0.2f);
        }
    }

}

