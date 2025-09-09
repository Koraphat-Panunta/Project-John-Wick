using NUnit.Framework.Constraints;
using UnityEngine;

public class EnemyChaserRoleNodeManager : EnemyActionNodeManager
{
    public override EnemyActionNodeLeaf curNodeLeaf { get; set; }
    public override EnemyActionSelectorNode startNodeSelector { get; set; }
    public float combatIntensity;
    private float approuchCoolDown;

    public EnemyChaserRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision, float minTimeUpdateYingYang, float maxTimeUpdateYingYang) 
        : base(enemy, enemyCommandAPI, enemyDecision, minTimeUpdateYingYang, maxTimeUpdateYingYang)
    {
    }

    public GuardingEnemyActionNodeLeaf guardingEnemyActionNodeLeaf { get;private set; }
    public FindTargetInTargetZoneEnemyActionNodeLeaf findTargetInTargetZoneEnemyActionNodeLeaf { get; private set; }
    private EnemyActionSelectorNode enemyAlertActionSelectorNode { get; set; }
    public DisarmTargetWeaponEnemyActionNodeLeaf disarmTargetWeaponEnemyActionNodeLeaf { get; private set; }
    public ApprouchingTargetEnemyActionNodeLeaf approuchingTargetEnemyActionNodeLeaf { get; private set; }
    public InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf { get; private set; }



    public override void InitailizedNode()
    {
        startNodeSelector = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);

        guardingEnemyActionNodeLeaf = new GuardingEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill,this);
        findTargetInTargetZoneEnemyActionNodeLeaf = new FindTargetInTargetZoneEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            ()=> curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware,this,targetZone);

        enemyAlertActionSelectorNode = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);
  
        approuchingTargetEnemyActionNodeLeaf = new ApprouchingTargetEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            ()=>
            {
                if (this.approuchCoolDown <= 0)
                {
                    this.approuchCoolDown = Random.Range(1,2);
                    return true;
                }
                return false;
            }
            ,this);
       
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            ()=>true
            ,this);

        disarmTargetWeaponEnemyActionNodeLeaf = new DisarmTargetWeaponEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => enemy._currentWeapon == null && curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert
            ,this);

        startNodeSelector.AddtoChildNode(guardingEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(findTargetInTargetZoneEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(enemyAlertActionSelectorNode);

        enemyAlertActionSelectorNode.AddtoChildNode(disarmTargetWeaponEnemyActionNodeLeaf);
        enemyAlertActionSelectorNode.AddtoChildNode(approuchingTargetEnemyActionNodeLeaf);
        enemyAlertActionSelectorNode.AddtoChildNode(insistEnemyActionNodeLeaf);
        
        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
        curNodeLeaf.Enter();

    }
    public override void UpdateNode()
    {
        base.UpdateNode();

    }
    public override void Enter()
    {
        base.targetZone.SerRaduise(10);
        base.Enter();
    }
    private void CoolDownUpdate()
    {
        if(approuchCoolDown > 0 && (this as INodeManager).TryGetCurNodeLeaf<ApprouchingTargetEnemyActionNodeLeaf>() == false)
            approuchCoolDown -= Time.deltaTime;
    }
}
