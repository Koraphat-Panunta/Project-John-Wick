using UnityEngine;

public class HoldingTacticDecision : TacticDecision
{
    private float cost_DrainRate;
    private float exitTacticCost;
    public HoldingTacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision) : base(enemy, enemyTacticDecision)
    {
    }

    public override void Enter()
    {

        cost_DrainRate = Random.Range(8f, 15f);
        exitTacticCost = Random.Range(65f, 90f);

    }

    public override void Exit()
    {
       

    }

    public override void FixedUpdate()
    {


    }

    public override void Update()
    {

        enemyTacticDecision.cost += cost_DrainRate * Time.deltaTime;
        if (enemyTacticDecision.cost > exitTacticCost)
        {
            enemyTacticDecision.ChangeTactic(enemyTacticDecision.encouterTacticDecision);
            return;
        }

        switch (enemyTacticDecision.curCombatPhase)
        {
            case EnemyTacticDecision.CombatPhase.Alert:
                {
                    enemyCommand.FreezPosition();
                    enemyCommand.AimDownSight(this.enemy.targetKnowPos);
                    enemyCommand.NormalFiringPattern.Performing();

                }
                break;
            case EnemyTacticDecision.CombatPhase.Aware:
                {
                    enemyCommand.FreezPosition();
                    enemyCommand.AimDownSight(this.enemy.targetKnowPos);

                }
                break;
            default: 
                {
                    enemyCommand.AimDownSight(this.enemy.targetKnowPos);
                    enemyCommand.MoveToPosition(this.enemy.targetKnowPos, 1);
                }
                break;
        }
        
    }
}
