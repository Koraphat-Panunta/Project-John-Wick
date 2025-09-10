using NUnit.Framework.Constraints;
using UnityEngine;

public class EnemyChaserRoleNodeManager : EnemyActionNodeManager,IObserverEnemyDecision
{
    public override EnemyActionNodeLeaf curNodeLeaf { get; set; }
    public override EnemyActionSelectorNode startNodeSelector { get; set; }
    public float combatIntensity;
    public float approuchCoolDown;

    public EnemyChaserRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision, float minTimeUpdateYingYang, float maxTimeUpdateYingYang) 
        : base(enemy, enemyCommandAPI, enemyDecision, minTimeUpdateYingYang, maxTimeUpdateYingYang)
    {
        (enemyDecision as EnemyDecision).AddEnemyDecisionObserver(this);
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

        guardingEnemyActionNodeLeaf = new GuardingEnemyActionNodeLeaf(enemy
            ,enemyCommandAPI
            ,()=>curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill,this.enemyDecision._enemyDecision);
        findTargetInTargetZoneEnemyActionNodeLeaf = new FindTargetInTargetZoneEnemyActionNodeLeaf(enemy
            ,enemyCommandAPI,
            ()=> curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Suspect
            ,this.enemyDecision._enemyDecision
            ,this.enemyDecision,targetZone);

        enemyAlertActionSelectorNode = new EnemyActionSelectorNode(enemy,enemyCommandAPI,()=>true);
  
        approuchingTargetEnemyActionNodeLeaf = new ApprouchingTargetEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            ()=>
            {
                Debug.Log("approuchCoolDown = " + approuchCoolDown);
                if (this.approuchCoolDown <= 0)
                {
                    return true;
                }
                return false;
            }
            ,this.enemyDecision._enemyDecision,this.enemyDecision);
       
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,
            ()=>true
            ,this.enemyDecision._enemyDecision
            ,this.enemyDecision);

        disarmTargetWeaponEnemyActionNodeLeaf = new DisarmTargetWeaponEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => enemy._currentWeapon == null && curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert
            ,this.enemyDecision._enemyDecision, this.enemyDecision);

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
        CoolDownUpdate();
    }
    public override void Enter()
    {
        base.targetZone.SerRaduise(10);
        base.Enter();
    }
    private void CoolDownUpdate()
    {
        if(approuchCoolDown > 0 && curNodeLeaf != this.approuchingTargetEnemyActionNodeLeaf)
            approuchCoolDown -= Time.deltaTime;

      
    }

    public void OnNotifyEnemyDecision<T>(EnemyDecision enemyDecision, T var)
    {
       if(var is ApprouchingTargetEnemyActionNodeLeaf approuchingTargetEnemyActionNodeLeaf 
            && approuchingTargetEnemyActionNodeLeaf.curPhase == EnemyActionNodeLeaf.EnemyActionPhase.Enter)
            approuchCoolDown = Random.Range(1.5f, 2.7f);
    }
}
