using UnityEngine;

public interface IHoldingGoal 
{
    public IEnemyGOAP _enemyGOAP { get; }
    public IFindingTarget _findingTarget { get; }
    public HoldingGoal _holdingGoal { get; set; }

}
