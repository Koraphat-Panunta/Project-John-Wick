using UnityEngine;

public class EnemyOverwatchRoleNodeManager : EnemyActionNodeManager
{
    public ZoneDefine overWatchZone;
    private const float overWatchZoneRaduis = 5;
    public EnemyOverwatchRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision, float minTimeUpdateYingYang, float maxTimeUpdateYingYang) : base(enemy, enemyCommandAPI, enemyDecision, minTimeUpdateYingYang, maxTimeUpdateYingYang)
    {
        overWatchZone = new ZoneDefine(enemy.transform.position, overWatchZoneRaduis);
        yingYangCalculate = enemyDecision._yingYang;
    }

    public override float yingYangCalculate { get; protected set ; }
    public override EnemyActionNodeLeaf curNodeLeaf { get ; set ; }
    public override EnemyActionSelectorNode startNodeSelector { get ; set ; }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }

    public GuardingEnemyActionNodeLeaf guardingEnemyActionNodeLeaf { get ; set ; }

    public EnemyActionSelectorNode overwatchEnemyActionSelector { get; set; }

    public MoveToTheZoneEnemyActionNodeLeaf moveToOverwatchZoneEnemyActionNodeLeaf { get; set; }
    public TakeCoverEnemyActionNodeLeaf takeCoverEnemyActionNodeLeaf { get; set; }
    public InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf { get; set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new EnemyActionSelectorNode(enemy, enemyCommandAPI, () => true);

        guardingEnemyActionNodeLeaf = new GuardingEnemyActionNodeLeaf(enemy, enemyCommandAPI, () => curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill, this);

        overwatchEnemyActionSelector = new EnemyActionSelectorNode(enemy, enemyCommandAPI, () => curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware
        || curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert);
        moveToOverwatchZoneEnemyActionNodeLeaf = new MoveToTheZoneEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => moveToOverwatchZoneEnemyActionNodeLeaf.assignZone.IsPositionInTheZone(enemy.transform.position) == false
            , this, overWatchZone);
        takeCoverEnemyActionNodeLeaf = new TakeCoverEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            () => 
            {
                if(takeCoverAble == false)
                    return false;

                if (Vector3.Distance(enemy.targetKnewPos, overWatchZone.zonePosition) < overWatchZone.raduis)
                    return false;

                if (enemy.coverPoint != null)
                    return true;

                if(enemyCommandAPI.FindCoverAndBook(overWatchZoneRaduis,enemy.targetKnewPos,out CoverPoint coverPoint))
                {
                    overWatchZone.SetZone(coverPoint.coverPos.position);
                    return true;
                }
                    

                return false;
            },this);
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>true,this);

        startNodeSelector.AddtoChildNode(guardingEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(overwatchEnemyActionSelector);

        overwatchEnemyActionSelector.AddtoChildNode(moveToOverwatchZoneEnemyActionNodeLeaf);
        overwatchEnemyActionSelector.AddtoChildNode(takeCoverEnemyActionNodeLeaf);
        overwatchEnemyActionSelector.AddtoChildNode(insistEnemyActionNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
        curNodeLeaf.Enter();
    }
}
