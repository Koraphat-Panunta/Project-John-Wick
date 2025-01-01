using UnityEngine;

public interface IEnemyGOAP
{
    public EnemyControllerAPI _enemyController { get; set; }
    public Enemy _enemy { get; set; }
    public EnemyGoalLeaf curGoal { get; set; }
    public EnemyGoalSelector startSelecotr { get; set; }
    public void GOAP_Update();
    public void GOAP_FixedUpdate();
    public void InitailizedGOAP();
}
