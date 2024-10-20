
using UnityEngine;
using UnityEngine.AI;


public class TakeCoverTactic : IEnemyTactic
{
    public Enemy enemy;
    public EnemyFindingCover enemyFindingCover;
    private IEnemyFiringPattern enemyFiringPattern;
    private NavMeshAgent agent;
    private bool isInCover;
    private CoverPositionEnemy coverPositionEnemy;
    private float costRate;
    private float exitStateCost = 86;
    public TakeCoverTactic(Enemy enemy)
    {
        this.enemy = enemy;
        enemyFindingCover = new EnemyFindingCover();
        enemyFiringPattern = new NormalFiringPattern(enemy);
        agent = enemy.agent;
        agent.speed = 0;
        agent.acceleration = 0;
        isInCover = false;
        coverPositionEnemy = null;
        costRate = Random.Range(3f, 5f);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TakeCover);
    }
    public void Manufacturing()
    {
        enemy.enemyLookForPlayer.Recived();
        if (enemy.enemyLookForPlayer.IsSeeingPlayer)
        {
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
        }
        if (coverPositionEnemy == null)
        {
            if (enemyFindingCover.FindingCover(enemy))
            {
                this.coverPositionEnemy = enemyFindingCover.coverPositionEnemy;
            }
            else
            {
                enemy.currentTactic = new HoldingTactic(enemy);
            }
        }
        else if(coverPositionEnemy != null)
        {
            if (isInCover == false)
            {
                if (MoveToCover(enemyFindingCover.coverPositionEnemy.coverPos, agent))
                {
                    isInCover = true;
                }
            }
            else if (isInCover == true)
            {
                CoverUsingPattern();
                if (CheckingCoverInterupt())
                {
                    enemy.currentTactic = new HoldingTactic(enemy);
                }
            }
        }
        enemy.cost += costRate * Time.deltaTime;
        if (enemy.cost >= exitStateCost)
        {
            enemy.currentTactic = new FlankingTactic(enemy);
        }
    }
    private void PeekAndShoot(Vector3 peekPos,NavMeshAgent agent) 
    {
        NavMeshHit hit;
        float maxDistance = 1;
        //if (NavMesh.SamplePosition(peekPos, out hit, maxDistance, NavMesh.AllAreas))
        //{
        //   agent.SetDestination(hit.position);  
        //}
        agent.SetDestination(peekPos);
        enemy.enemyAgentMovementOverride.OverrideAgentInFrame(3, 3);
        enemy.enemyWeaponCommand.Aiming();
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._move);
        enemyFiringPattern.Performing();
        new RotateObjectToward().RotateTowards(enemy.Target, enemy.gameObject, 6);
    }
    private void BackToCover(Vector3 CoverPos, NavMeshAgent agent) 
    {
        NavMeshHit hit;
        float maxDistance = 1;

        //if (NavMesh.SamplePosition(CoverPos, out hit, maxDistance, NavMesh.AllAreas))
        //{
        //    agent.SetDestination(hit.position);
        //}
        agent.SetDestination(CoverPos);
        enemy.enemyAgentMovementOverride.OverrideAgentInFrame(3, 3);
        enemy.enemyWeaponCommand.LowReady();
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._move);
    }
    private bool MoveToCover(Vector3 CoverPos, NavMeshAgent agent)
    {
        agent.SetDestination(CoverPos);
        if (Vector3.Distance(enemy.transform.position, new Vector3(CoverPos.x,enemy.transform.position.y,CoverPos.z)) < 1f)
        {
            return true;
        }
        else
        {
            enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._sprint);
            enemy.enemyWeaponCommand.LowReady();
            return false;
        }
    }

    float timimgCoverPattern = 0;
    bool IsPeeking;
    private void CoverUsingPattern()
    {
        if (IsPeeking == true)
        {
            PeekAndShoot(coverPositionEnemy.aimPos, agent);
        }
        else if (IsPeeking == false)
        {
           BackToCover(coverPositionEnemy.coverPos, agent);
        }
        timimgCoverPattern -= Time.deltaTime;
        if(timimgCoverPattern <= 0)
        {
            timimgCoverPattern = Random.Range(2f, 4.5f);
            if(IsPeeking == true)
            {
                IsPeeking = false;
            }
            else if(IsPeeking == false)
            {
                IsPeeking= true;
            }
        }
    }
    private bool CheckingCoverInterupt()
    {
        Collider[] other = Physics.OverlapSphere(enemy.transform.position, 0.5f);
        foreach(Collider collider in other)
        {
            Enemy enemyNearBy;
            if(collider.TryGetComponent<ChestBodyPart>(out ChestBodyPart e))
            {
                enemyNearBy = e.enemy;
                if (enemyNearBy != enemy)
                {
                    return true;
                }
            }
           
        }
        return false;
    }
}
