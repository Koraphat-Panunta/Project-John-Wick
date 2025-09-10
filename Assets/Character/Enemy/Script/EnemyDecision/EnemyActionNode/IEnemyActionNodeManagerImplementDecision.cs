using UnityEngine;

public interface IEnemyActionNodeManagerImplementDecision 
{
    public enum CombatPhase
    {
        Chill,
        Suspect,
        Aware,
        Alert
    }
    public CombatPhase _curCombatPhase { get; set; }
    public ZoneDefine _targetZone { get; set; }
    public bool _takeCoverAble { get; set; }
    public EnemyDecision _enemyDecision { get; set; }


}
