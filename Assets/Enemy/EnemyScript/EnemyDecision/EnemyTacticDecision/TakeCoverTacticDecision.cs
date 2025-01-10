using UnityEngine;

public class TakeCoverTacticDecision : TacticDecision
{
    private CoverPoint coverPoint;
    private float cost_DrainRate;
    private float exitTacticCost;

    private float coverTiming;
    private float peekTiming;
    public TakeCoverTacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision) : base(enemy, enemyTacticDecision)
    {
    }

    public override void Enter()
    {
        Debug.Log("TakeCoverTactic Enter");

        coverTiming = 0f;
        peekTiming = Random.Range(4, 7);

        if (enemy.findingCover.FindCoverInRaduis(7,out CoverPoint coverPoint))
            this.coverPoint = coverPoint;
        else
        {
            enemyTacticDecision.ChangeTactic(enemyTacticDecision.holdingTacticDecision);
            return;
        }

       
        cost_DrainRate = Random.Range(1.5f,3f);
        exitTacticCost = Random.Range(65f, 86f);
        Debug.Log("cost_DrainRate =" + cost_DrainRate);

    }

    public override void Exit()
    {
        Debug.Log("TakeCoverTactic Exit");

        this.coverPoint = null;
        enemyCommand.GetOffCover();
    }

    public override void FixedUpdate()
    {
        Debug.Log("TakeCoverTactic FixedUpdate");
    }

    public override void Update()
    {
        Debug.Log("TakeCoverTactic Update");

        enemy.cost += cost_DrainRate * Time.deltaTime;
        if (enemy.cost > exitTacticCost)
            enemyTacticDecision.ChangeTactic(enemyTacticDecision.encouterTacticDecision);

        if (enemy.isInCover == false)
        {
            if (enemy.combatOffensiveInstinct.offensiveIntensity < 30)
                enemyCommand.MoveToTakeCover(this.coverPoint, 1);
            else
                enemyCommand.SprintToCover(this.coverPoint);

            return;
        }

        coverTiming += Time.deltaTime;

        if(coverTiming < peekTiming) //TakeCover
        {
            enemyCommand.LowReady();
            return;
        }

        switch (enemy.combatOffensiveInstinct.myCombatPhase)//TakeAim
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 4);
                    enemyCommand.NormalFiringPattern.Performing();
                }
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 4);
                    enemyCommand.NormalFiringPattern.Performing();
                }
                break;
            default:
                {
                    enemyCommand.AimDownSight();
                }
                break;
        }

        if (Vector3.Dot(coverPoint.coverDir.normalized,
            (enemy.targetKnewPos - enemy.transform.position).normalized) <= 0)
            enemy.cost += 100 * Time.deltaTime;

        if (coverTiming > 10)
            coverTiming = 0;
    }
}
