using UnityEngine;

public class StayEnemyTutorialDecision : EnemyDecision
{
    public CombatPhase _curCombatPhase { get => this.combatPhase; set => this.combatPhase = value; }
    private CombatPhase combatPhase;
    public ZoneDefine _targetZone { get; set; }
    public bool _takeCoverAble { get; set; }
    public EnemyDecision _enemyDecision { get => this; set { } }

    public float lostSightTime;
    public override void Initialized()
    {
        _takeCoverAble = false;
        _targetZone = new ZoneDefine(this.transform.position,2.5f);
        _curCombatPhase = CombatPhase.Chill;

        base.Initialized();
    }
    protected override void Update()
    {
        if(lostSightTime < 0)
        {
            if(_curCombatPhase > CombatPhase.Aware)
                _curCombatPhase = CombatPhase.Aware;
        }
        else
            this.lostSightTime -= Time.deltaTime;   

        switch (_curCombatPhase)
        {
            case CombatPhase.Chill:
                {
                    enemyCommand.LowReady();
                    enemyCommand.FreezPosition();
                    break;
                }
            case CombatPhase.Suspect: 
                {
                    enemyCommand.RotateToPosition(this.enemy.targetKnowPos,1);
                    enemyCommand.FreezPosition();
                    break;
                }
            case CombatPhase.Aware: 
                {
                    enemyCommand.AimDownSight(this.enemy.targetKnowPos);
                    enemyCommand.FreezPosition();
                    break;
                }
            case CombatPhase.Alert: 
                {
                    enemyCommand.AimDownSight(this.enemy.targetKnowPos);
                    enemyCommand.NormalFiringPattern.Performing();
                    enemyCommand.FreezPosition();
                    break;
                }
        }
        
        base.Update();
    }
    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if(this._curCombatPhase < CombatPhase.Suspect)
            this._curCombatPhase = CombatPhase.Suspect;

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        if(this._curCombatPhase < CombatPhase.Alert)
            this._curCombatPhase = CombatPhase.Alert;

        lostSightTime = 3;
    }
}
