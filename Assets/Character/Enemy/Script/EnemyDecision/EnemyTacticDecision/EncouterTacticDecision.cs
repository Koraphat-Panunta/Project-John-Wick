using UnityEngine;

public class EncouterTacticDecision : TacticDecision
{
    private float backToSerchTiming = 2;
    private float exitTacticCost;
    private float cost_DrainRate;
    public EnemyMoveCurvePath curvePath;

    public EncouterTacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision) : base(enemy, enemyTacticDecision)
    {
        cost_DrainRate = Random.Range(9, 15);
        exitTacticCost = Random.Range(29, 42);
        curvePath = new EnemyMoveCurvePath(5,10 );
    }

    public override void Enter()
    {
        curvePath.GenaratePath(enemy.targetKnewPos,enemy.transform.position);
        enemyCommand.FreezPosition();

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        enemyTacticDecision.cost -= cost_DrainRate * Time.deltaTime;
        if (enemyTacticDecision.cost < exitTacticCost/*&&enemyBody.cost > Vector3.Distance(enemyBody._transform.position,enemyBody.Target._transform.position)*2*/)
        {
            if (enemy.findingCover.FindCoverInRaduisInGunFight(7, out CoverPoint coverPoint))
            {
                enemyTacticDecision.ChangeTactic(enemyTacticDecision.takeCoverTacticDecision);
            }
            else
            {
                enemyTacticDecision.ChangeTactic(enemyTacticDecision.holdingTacticDecision);
            }
            return;
        }

        switch (enemyTacticDecision.curCombatPhase)
        {
            case EnemyTacticDecision.CombatPhase.Alert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos);
                    enemyCommand.NormalFiringPattern.Performing();

                }
                break;
            case EnemyTacticDecision.CombatPhase.Aware:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos);
                }
                break;
        }

        curvePath.AutoRegenaratePath(enemy.targetKnewPos, enemy.transform.position, 2);

        if (curvePath._curvePoint.Count > 0)
        if (enemyCommand.MoveToPosition(curvePath._curvePoint.Peek(), 1))
        {
            enemyCommand.FreezPosition();
            curvePath._curvePoint.Dequeue();
        }



    }
}
