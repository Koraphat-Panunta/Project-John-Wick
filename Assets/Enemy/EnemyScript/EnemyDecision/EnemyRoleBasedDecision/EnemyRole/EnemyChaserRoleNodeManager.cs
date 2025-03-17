using NUnit.Framework.Constraints;
using UnityEngine;

public class EnemyChaserRoleNodeManager : EnemyActionNodeManager
{
    public override EnemyActionNodeLeaf curNodeLeaf { get; set; }
    public override EnemyActionSelectorNode startNodeSelector { get; set; }

    public EnemyChaserRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision, float minTimeUpdateYingYang, float maxTimeUpdateYingYang) 
        : base(enemy, enemyCommandAPI, enemyDecision, minTimeUpdateYingYang, maxTimeUpdateYingYang)
    {
    }

    public GuardingEnemyActionNodeLeaf guardingEnemyActionNodeLeaf { get;private set; }
    public FindTargetInTargetZoneEnemyActionNodeLeaf findTargetInTargetZoneEnemyActionNodeLeaf { get; private set; }
    private EnemyActionSelectorNode enemyAlertActionSelectorNode { get; set; }
    public MoveToTheZoneEnemyActionNodeLeaf moveToCombatZone { get; set; }
    public TakeCoverEnemyActionNodeLeaf takeCoverEnemyActionNodeLeaf { get; set; }
    public InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf { get; private set; }
    public ApprouchingTargetEnemyActionNodeLeaf approuchingTargetEnemyActionNodeLeaf { get; private set; }
    public override float yingYangCalculate { get ;protected set ; }

    public override void InitailizedNode()
    {
        startNodeSelector = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);

        guardingEnemyActionNodeLeaf = new GuardingEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill,this);
        findTargetInTargetZoneEnemyActionNodeLeaf = new FindTargetInTargetZoneEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            ()=> curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware,this,targetZone);

        enemyAlertActionSelectorNode = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);
        moveToCombatZone = new MoveToTheZoneEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => moveToCombatZone.assignZone.IsPositionInTheZone(enemy.transform.position) == false, this, targetZone);
        approuchingTargetEnemyActionNodeLeaf = new ApprouchingTargetEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=> enegyWithIn >= yingYang,this);
        takeCoverEnemyActionNodeLeaf = new TakeCoverEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => 
            {
                if(Vector3.Distance(enemy.targetKnewPos,enemy.transform.position) < 5.5f)
                    return false;

                if(enemy.coverPoint != null && takeCoverAble)
                    return true;

                if(enemyCommandAPI.FindCoverAndBook(6, out CoverPoint coverPoint)
            && takeCoverAble)
                {
                    return true;
                }
                return false;
            }
            , this);
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>true,this);

        startNodeSelector.AddtoChildNode(guardingEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(findTargetInTargetZoneEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(enemyAlertActionSelectorNode);

        enemyAlertActionSelectorNode.AddtoChildNode(moveToCombatZone);
        enemyAlertActionSelectorNode.AddtoChildNode(approuchingTargetEnemyActionNodeLeaf);
        enemyAlertActionSelectorNode.AddtoChildNode(takeCoverEnemyActionNodeLeaf);
        enemyAlertActionSelectorNode.AddtoChildNode(insistEnemyActionNodeLeaf);
        
        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
        curNodeLeaf.Enter();

    }
    public override void UpdateNode()
    {
        base.UpdateNode();

        if (curNodeLeaf is ApprouchingTargetEnemyActionNodeLeaf)
            yingYangCalculate -= Time.deltaTime * 2f;
        else if (curNodeLeaf is InsistEnemyActionNodeLeaf
            || curNodeLeaf is TakeCoverEnemyActionNodeLeaf)
            yingYangCalculate += Time.deltaTime * 7f;

        yingYangCalculate = Mathf.Clamp(yingYangCalculate,0, 100);
    }
}
