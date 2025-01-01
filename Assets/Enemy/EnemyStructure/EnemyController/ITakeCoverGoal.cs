using UnityEngine;

public interface ITakeCoverGoal 
{
    public IEnemyGOAP _enemyGOAP { get; }
    public ICoverUseable _coverUseable { get; }
    public TakeCoverGoal _takeCoverGoal { get; set; }
}
