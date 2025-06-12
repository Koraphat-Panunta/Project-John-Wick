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

        coverTiming = 0f;
        peekTiming = Random.Range(1,2.5f);

        if (enemy.findingCover.FindCoverInRaduisInGunFight(7,out CoverPoint coverPoint))
            this.coverPoint = coverPoint;
        else
        {
            enemyTacticDecision.ChangeTactic(enemyTacticDecision.holdingTacticDecision);
            return;
        }

       
        cost_DrainRate = Random.Range(1.5f,3f);
        exitTacticCost = Random.Range(65f, 86f);

    }

    public override void Exit()
    {
        this.coverPoint = null;
        enemyCommand.GetOffCover();
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {

        enemyTacticDecision.cost += cost_DrainRate * Time.deltaTime;
        if (enemy._isPainTrigger || enemy._triggerHitedGunFu)
            enemyTacticDecision.cost += 30;
        if (enemyTacticDecision.cost > exitTacticCost)
        {
            enemyTacticDecision.ChangeTactic(enemyTacticDecision.encouterTacticDecision);
            return ;
        }

        if (enemy.isInCover == false)
        {
            if (enemyTacticDecision.pressure < 30)
                enemyCommand.MoveToTakeCover(this.coverPoint, 1);
            else
                enemyCommand.SprintToCover(this.coverPoint);
            return;
        }
        else
            PerformCoverManuver();// isInCover == true
        

       
    }
    private void PerformCoverManuver()
    {
        coverTiming += Time.deltaTime;

        if (coverTiming < peekTiming) //TakeCover
        {
            enemyCommand.LowReady();
            return;
        }

        switch (enemyTacticDecision.curCombatPhase)//TakeAim
        {
            case EnemyTacticDecision.CombatPhase.Alert:
                {
                    enemyCommand.AimDownSight(enemy.targetKnewPos, 4);
                    enemyCommand.NormalFiringPattern.Performing();
                }
                break;
            case EnemyTacticDecision.CombatPhase.Aware:
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
            enemyTacticDecision.cost += 100 * Time.deltaTime;

        if (coverTiming > 5)
            coverTiming = 0;
    }
}
