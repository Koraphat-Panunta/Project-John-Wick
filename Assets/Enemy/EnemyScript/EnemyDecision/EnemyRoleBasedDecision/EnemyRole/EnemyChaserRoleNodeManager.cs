using NUnit.Framework.Constraints;
using UnityEngine;

public class EnemyChaserRoleNodeManager : EnemyActionNodeManager
{

    public override EnemyActionNodeLeaf curNodeLeaf { get; set; }
    public override EnemyActionSelectorNode startNodeSelector { get; set; }
    public EnemyRoleBasedDecision enemyRoleBasedDecision { get; set; }

    public EnemyChaserRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI,EnemyRoleBasedDecision enemyRoleBasedDecision) : base(enemy, enemyCommandAPI,enemyRoleBasedDecision)
    {
        this.enemyRoleBasedDecision = enemyRoleBasedDecision;
    }
  
    public GuardingEnemyActionNodeLeaf guardingEnemyActionNodeLeaf { get;private set; }

    private EnemyActionSelectorNode enemyAlertActionSelectorNode { get; set; }
    public InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf { get; private set; }
    public ApprouchingTargetEnemyActionNodeLeaf approuchingTargetEnemyActionNodeLeaf { get; private set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);

        guardingEnemyActionNodeLeaf = new GuardingEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>curCombatPhase == EnemyDecision.CombatPhase.Chill,this);

        enemyAlertActionSelectorNode = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);

        approuchingTargetEnemyActionNodeLeaf = new ApprouchingTargetEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=> pressure < 60,this);
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>true,this);

        startNodeSelector.AddtoChildNode(guardingEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(enemyAlertActionSelectorNode);

        enemyAlertActionSelectorNode.AddtoChildNode(approuchingTargetEnemyActionNodeLeaf);
        enemyAlertActionSelectorNode.AddtoChildNode(insistEnemyActionNodeLeaf);
        
        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
        curNodeLeaf.Enter();

    }
}
