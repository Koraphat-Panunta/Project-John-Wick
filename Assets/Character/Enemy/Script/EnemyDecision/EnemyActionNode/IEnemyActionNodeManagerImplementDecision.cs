using UnityEngine;

public interface IEnemyActionNodeManagerImplementDecision 
{
    public enum CombatPhase
    {
        Chill,
        Aware,
        Alert
    }
    public CombatPhase _curCombatPhase { get; set; }

    public float _yingYang { get; set;} //0 : Ying represent calm , 100 : Yang represent aggressively
    public ZoneDefine _targetZone { get; set; }
    public bool _takeCoverAble { get; set; }


}
