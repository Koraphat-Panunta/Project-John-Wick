using UnityEngine;

public class EncouterTacticDecision : TacticDecision
{
    private float backToSerchTiming = 2;
    private float cost_DrainRate;
    public CurvePath curvePath;

    public EncouterTacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision) : base(enemy, enemyTacticDecision)
    {
        cost_DrainRate = Random.Range(9, 15);
        curvePath = new CurvePath();
    }

    public override void Enter()
    {
        curvePath.GenaratePath(enemy.targetKnewPos,enemy.transform.position);
        enemyCommand.Freez();

        Debug.Log("Encouter Enter");

    }

    public override void Exit()
    {
        Debug.Log("Encouter Exit");
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (enemy.cost < 34/*&&enemy.cost > Vector3.Distance(enemy.transform.position,enemy.Target.transform.position)*2*/)
        {
            //enemy.currentTactic = new TakeCoverTactic(enemy);
            //return;
        }

        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        switch (combatPhase)
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommand.NormalFiringPattern.Performing();
                    enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
                    enemy.cost -= cost_DrainRate * Time.deltaTime;
                }
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);
                    enemyCommand.NormalFiringPattern.Performing();
                    enemy.cost -= cost_DrainRate * Time.deltaTime;
                }
                break;
            case CombatOffensiveInstinct.CombatPhase.SemiAlert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 5);

                    enemy.cost -= cost_DrainRate * Time.deltaTime;
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

        Debug.Log("Encouter Update");

    }
}
