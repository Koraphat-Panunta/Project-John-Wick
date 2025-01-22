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

        enemy.cost += cost_DrainRate * Time.deltaTime;
        if (enemy.cost > exitTacticCost)
        {
            enemyTacticDecision.ChangeTactic(enemyTacticDecision.encouterTacticDecision);
            return;
        }

        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
        switch (combatPhase)
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert:
                {
                    enemyCommand.Freez();
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommand.NormalFiringPattern.Performing();
                    enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);

                }
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert:
                {
                    enemyCommand.Freez();
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommand.NormalFiringPattern.Performing();

                }
                break;
            case CombatOffensiveInstinct.CombatPhase.SemiAlert:
                {
                    enemyCommand.Freez();
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);

                }
                break;
            default: 
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos,5);
                    enemyCommand.MoveToPosition(enemy.targetKnewPos, 1);
                }
                break;
        }
        
    }
}
