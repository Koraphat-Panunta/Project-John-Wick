using UnityEngine;

public class EncouterTacticDecision : TacticDecision
{
    private float backToSerchTiming = 2;
    private float exitTacticCost;
    private float cost_DrainRate;
    public CurvePath curvePath;

    public EncouterTacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision) : base(enemy, enemyTacticDecision)
    {
        cost_DrainRate = Random.Range(9, 15);
        exitTacticCost = Random.Range(29, 42);
        curvePath = new CurvePath();
    }

    public override void Enter()
    {
        curvePath.GenaratePath(enemy.targetKnewPos,enemy.transform.position);
        enemyCommand.Freez();

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        enemy.cost -= cost_DrainRate * Time.deltaTime;
        if (enemy.cost < exitTacticCost/*&&enemy.cost > Vector3.Distance(enemy.transform.position,enemy.Target.transform.position)*2*/)
        {
            if (enemy.findingCover.FindCoverInRaduis(7, out CoverPoint coverPoint))
            {
                enemyTacticDecision.ChangeTactic(enemyTacticDecision.takeCoverTacticDecision);
            }
            else
            {
                enemyTacticDecision.ChangeTactic(enemyTacticDecision.holdingTacticDecision);
            }
            return;
        }

        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        switch (combatPhase)
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommand.NormalFiringPattern.Performing();
                    enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);

                }
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommand.NormalFiringPattern.Performing();

                }
                break;
            case CombatOffensiveInstinct.CombatPhase.SemiAlert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                }
                break;
        }



        curvePath.AutoRegenaratePath(enemy.targetKnewPos, enemy.transform.position, 2);

        if (curvePath._curvePoint.Count > 0)
        if (enemyCommand.MoveToPosition(curvePath._curvePoint.Peek(), 1))
        {
            enemyCommand.Freez();
            curvePath._curvePoint.Dequeue();
        }



    }
}
