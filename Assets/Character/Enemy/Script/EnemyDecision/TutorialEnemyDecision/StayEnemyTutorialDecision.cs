using UnityEngine;

public class StayEnemyTutorialDecision : EnemyDecision, IEnemyActionNodeManagerImplementDecision
{
    public IEnemyActionNodeManagerImplementDecision.CombatPhase _curCombatPhase { get => this.combatPhase; set => this.combatPhase = value; }
    private IEnemyActionNodeManagerImplementDecision.CombatPhase combatPhase;
    public ZoneDefine _targetZone { get; set; }
    public bool _takeCoverAble { get; set; }
    public EnemyDecision _enemyDecision { get => this; set { } }

    public float lostSightTime;
    public override void Initialized()
    {
        _takeCoverAble = false;
        _targetZone = new ZoneDefine(this.transform.position,2.5f);
        _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill;

        base.Initialized();
    }
    protected override void Update()
    {
        if(lostSightTime < 0)
        {
            if(_curCombatPhase > IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware)
                _curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware;
        }
        else
            this.lostSightTime -= Time.deltaTime;   

        switch (_curCombatPhase)
        {
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Chill:
                {
                    enemyCommand.LowReady();
                    enemyCommand.FreezPosition();
                    break;
                }
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Suspect: 
                {
                    enemyCommand.RotateToPosition(enemy.targetKnewPos,1);
                    enemyCommand.FreezPosition();
                    break;
                }
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Aware: 
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos);
                    enemyCommand.FreezPosition();
                    break;
                }
            case IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert: 
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos);
                    enemyCommand.NormalFiringPattern.Performing();
                    enemyCommand.FreezPosition();
                    break;
                }
        }
        
        base.Update();
    }
    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if(this._curCombatPhase < IEnemyActionNodeManagerImplementDecision.CombatPhase.Suspect)
            this._curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Suspect;

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        if(this._curCombatPhase < IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert)
            this._curCombatPhase = IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert;

        lostSightTime = 3;
    }
}
