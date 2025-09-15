using UnityEngine;

public class EnemyOverwatchRoleNodeManager : EnemyActionNodeManager,IObserverEnemyDecision
{

    public float swarpPositionTimer;
    private readonly float MIN_RANDOM_SWARP_TIME = 5;
    private readonly float MAX_RANDOM_SWARP_TIME = 8;
    public EnemyOverwatchRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision, float minTimeUpdateYingYang, float maxTimeUpdateYingYang) 
        : base(enemy, enemyCommandAPI, enemyDecision, minTimeUpdateYingYang, maxTimeUpdateYingYang)
    {
        enemyDecision._enemyDecision.AddEnemyDecisionObserver(this);
    }
    public override EnemyActionNodeLeaf curNodeLeaf { get ; set ; }
    public override EnemyActionSelectorNode startNodeSelector { get ; set ; }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        if (swarpPositionTimer > 0)
            swarpPositionTimer -= Time.deltaTime;
        base.UpdateNode();
    }
   
    public InsistEnemyActionNodeLeaf guardingEnemyActionNodeLeaf { get ; set ; }
    public FindTargetInTargetZoneEnemyActionNodeLeaf findTargetInTargetZoneEnemyActionNodeLeaf { get; private set; }
    public EnemyActionSelectorNode overwatchAwareAlertActionSelector { get; set; }

    public DisarmTargetWeaponEnemyActionNodeLeaf disarmTargetWeaponEnemyActionNodeLeaf { get; private set; }
    public SwarpCombatPositionActionNodeLeaf swarpCombatPositionActionNodeLeaf { get; private set; }
    public InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf { get; set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new EnemyActionSelectorNode(enemy, enemyCommandAPI, () => true);

        guardingEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy, enemyCommandAPI, 
            () => curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill
            , this.enemyDecision._enemyDecision
            ,this.enemyDecision);
        findTargetInTargetZoneEnemyActionNodeLeaf = new FindTargetInTargetZoneEnemyActionNodeLeaf(enemy,enemyCommandAPI
            ,()=> curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Suspect 
            ,this.enemyDecision._enemyDecision,this.enemyDecision
            ,targetZone);
        overwatchAwareAlertActionSelector = new EnemyActionSelectorNode(enemy, enemyCommandAPI
            , () => curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware
        || curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert);
        disarmTargetWeaponEnemyActionNodeLeaf = new DisarmTargetWeaponEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => enemy._currentWeapon == null && curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert
            , this.enemyDecision._enemyDecision
            , this.enemyDecision);
        swarpCombatPositionActionNodeLeaf = new SwarpCombatPositionActionNodeLeaf(enemy,enemyCommandAPI
            ,()=> swarpPositionTimer <= 0
            ,this.enemyDecision,this.enemyDecision._enemyDecision);
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>true,this.enemyDecision._enemyDecision, this.enemyDecision);

        startNodeSelector.AddtoChildNode(guardingEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(findTargetInTargetZoneEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(overwatchAwareAlertActionSelector);

        overwatchAwareAlertActionSelector.AddtoChildNode(disarmTargetWeaponEnemyActionNodeLeaf);
        overwatchAwareAlertActionSelector.AddtoChildNode(swarpCombatPositionActionNodeLeaf);
        overwatchAwareAlertActionSelector.AddtoChildNode(insistEnemyActionNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
        curNodeLeaf.Enter();
    }
  
    public override void Enter()
    {
        base.targetZone.SerRaduise(6);

        base.Enter();
    }

    public void OnNotifyEnemyDecision<T>(EnemyDecision enemyDecision, T var)
    {
        if (var is SwarpCombatPositionActionNodeLeaf swarpCombatPositionActionNodeLeaf && swarpCombatPositionActionNodeLeaf.curPhase == EnemyActionNodeLeaf.EnemyActionPhase.Exit)
            this.swarpPositionTimer = Random.Range(this.MIN_RANDOM_SWARP_TIME, this.MAX_RANDOM_SWARP_TIME);
    }
}
