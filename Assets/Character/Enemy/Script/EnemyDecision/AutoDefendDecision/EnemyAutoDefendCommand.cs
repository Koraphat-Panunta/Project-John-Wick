public class EnemyAutoDefendCommand  
{
    private readonly NodeComponentManager _nodeComponentManager;
    public readonly DodgingDecisionNodeLeaf _dodgingDecision;
    public readonly GuardingDecisionNodeLeaf _guardingDecision;

    public EnemyAutoDefendCommand(EnemyCommandAPI api)
    {
        var enemy = api._enemy;
        _dodgingDecision  = new DodgingDecisionNodeLeaf(
            enemy
            , api
            , enemy.enemyStatsScripableObject.minDodgeCoolDownTime
            , enemy.enemyStatsScripableObject.maxDodgeCoolDownTime
            , enemy.enemyStatsScripableObject.minDodgeHitRate
            , enemy.enemyStatsScripableObject.maxDodgeHitRate
            );
        _guardingDecision = new GuardingDecisionNodeLeaf(
            enemy
            , api
            , enemy.enemyStatsScripableObject.minGuardRate
            , enemy.enemyStatsScripableObject.maxGuardRate
            , enemy.enemyStatsScripableObject.minCounterRate
            , enemy.enemyStatsScripableObject.maxCounterRate
            );
        _nodeComponentManager = new NodeComponentManager();
        //_nodeComponentManager.AddNode(_dodgingDecision);
        _nodeComponentManager.AddNode(_guardingDecision);
 
    }

    public void UpdateDefendActionBlackBoard() => _nodeComponentManager.Update();

   
}
