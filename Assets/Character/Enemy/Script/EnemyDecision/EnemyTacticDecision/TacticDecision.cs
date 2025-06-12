using UnityEngine;

public abstract class TacticDecision 
{
    protected Enemy enemy;
    protected EnemyTacticDecision enemyTacticDecision;
    protected EnemyCommandAPI enemyCommand;
    public TacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision)
    {
        this.enemy = enemy;
        this.enemyTacticDecision = enemyTacticDecision;
        enemyCommand = this.enemyTacticDecision.enemyCommand;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract void FixedUpdate();
}
