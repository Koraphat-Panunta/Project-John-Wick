using UnityEngine;

public class EnemyOverwatchRoleNodeManager : EnemyActionNodeManager
{
    public ZoneDefine overWatchZone;
    private const float overWatchZoneRaduis = 1;
    public EnemyOverwatchRoleNodeManager(Enemy enemy, EnemyCommandAPI enemyCommandAPI, IEnemyActionNodeManagerImplementDecision enemyDecision, float minTimeUpdateYingYang, float maxTimeUpdateYingYang) 
        : base(enemy, enemyCommandAPI, enemyDecision, minTimeUpdateYingYang, maxTimeUpdateYingYang)
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
        this.UpdateYingYang();
        base.UpdateNode();
    }
    private void UpdateYingYang()
    {
        if(enegyWithIn > yingYang)
        {
            yingYangCalculate = Mathf.Clamp(Random.Range(0,1),0.5f,0.75f)*yingYang;
            enegyWithIn = yingYangCalculate;
            this.AssignOverwatchOverwatchZone();
        }
        yingYangCalculate += Time.deltaTime * 4.5f;

        yingYangCalculate = Mathf.Clamp(yingYangCalculate, 0, 100);
    }

    public InsistEnemyActionNodeLeaf guardingEnemyActionNodeLeaf { get ; set ; }
    public FindTargetInTargetZoneEnemyActionNodeLeaf findTargetInTargetZoneEnemyActionNodeLeaf { get; private set; }
    public EnemyActionSelectorNode overwatchAwareAlertActionSelector { get; set; }

    public DisarmTargetWeaponEnemyActionNodeLeaf disarmTargetWeaponEnemyActionNodeLeaf { get; private set; }
    public MoveToTheZoneEnemyActionNodeLeaf moveToOverwatchZoneEnemyActionNodeLeaf { get; set; }
    public TakeCoverEnemyActionNodeLeaf takeCoverEnemyActionNodeLeaf { get; set; }
    public InsistEnemyActionNodeLeaf insistEnemyActionNodeLeaf { get; set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new EnemyActionSelectorNode(enemy, enemyCommandAPI, () => true);

        guardingEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy, enemyCommandAPI, 
            () => curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill
            , this);
        findTargetInTargetZoneEnemyActionNodeLeaf = new FindTargetInTargetZoneEnemyActionNodeLeaf(enemy,enemyCommandAPI
            ,()=> curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware
            ,this
            ,targetZone);
        overwatchAwareAlertActionSelector = new EnemyActionSelectorNode(enemy, enemyCommandAPI
            , () => curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware
        || curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert);
        disarmTargetWeaponEnemyActionNodeLeaf = new DisarmTargetWeaponEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => enemy._currentWeapon == null && curCombatPhase == IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert
            , this);
        moveToOverwatchZoneEnemyActionNodeLeaf = new MoveToTheZoneEnemyActionNodeLeaf(enemy, enemyCommandAPI,
            () => moveToOverwatchZoneEnemyActionNodeLeaf.assignZone.IsPositionInTheZone(enemy.transform.position) == false
            , this, overWatchZone);
        insistEnemyActionNodeLeaf = new InsistEnemyActionNodeLeaf(enemy,enemyCommandAPI,()=>true,this);

        startNodeSelector.AddtoChildNode(guardingEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(findTargetInTargetZoneEnemyActionNodeLeaf);
        startNodeSelector.AddtoChildNode(overwatchAwareAlertActionSelector);

        overwatchAwareAlertActionSelector.AddtoChildNode(disarmTargetWeaponEnemyActionNodeLeaf);
        overwatchAwareAlertActionSelector.AddtoChildNode(moveToOverwatchZoneEnemyActionNodeLeaf);
        overwatchAwareAlertActionSelector.AddtoChildNode(insistEnemyActionNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf as EnemyActionNodeLeaf;
        curNodeLeaf.Enter();
    }
    private float randomMaxDegreesOverwatchZone = 30;

    private float randomMaxRangeOverwatchZone = 13;
    private float randomMinRangeOverwatchZone = 6f;
    private void AssignOverwatchOverwatchZone()
    {
        float rotateOverwatch = Random.Range(- this.randomMaxDegreesOverwatchZone, this.randomMaxDegreesOverwatchZone);
        float raduisOverwatch = Random.Range(this.randomMinRangeOverwatchZone, this.randomMaxRangeOverwatchZone);

        Vector3 targetToEnemyDir = enemy.targetKnewPos - enemy.transform.position;
        targetToEnemyDir.y = 0;
        targetToEnemyDir.Normalize();



        Vector3 assignPos = enemy.transform.position + ((Quaternion.AngleAxis(rotateOverwatch, Vector3.up) * targetToEnemyDir)*raduisOverwatch);
        if(Physics.Raycast(enemy.targetKnewPos
            ,(assignPos - enemy.targetKnewPos).normalized
            ,out RaycastHit hit
            , (assignPos - enemy.targetKnewPos).magnitude
            ,LayerMask.GetMask("Default")
            ,QueryTriggerInteraction.Ignore) 
            && Vector3.Distance(hit.point,enemy.targetKnewPos) >= randomMinRangeOverwatchZone)
        {
            assignPos = hit.point;
        }
       


        overWatchZone.SetZone(assignPos);
    }
    public override void Enter()
    {
        base.targetZone.SerRaduise(15);
        base.Enter();
    }
}
