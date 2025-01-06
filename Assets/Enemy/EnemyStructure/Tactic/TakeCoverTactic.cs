
using UnityEngine;
using UnityEngine.AI;


public class TakeCoverTactic : IEnemyTactic
{
    public Enemy enemy;
    public EnemyFindingCover enemyFindingCover;
    private IEnemyFiringPattern enemyFiringPattern;
    private NavMeshAgent agent;
    private bool isInCover;
    private CoverPoint coverPositionEnemy;
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
        costRate = Random.Range(1f, 2f);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TakeCover);
        //Debug.Log(enemy + " EnterTakeCover");
        //enemy.isInCombat = true;
    }
    public void Manufacturing()
    {
        
        if (enemy.findingTargetComponent.FindTarget(out GameObject target))
        {
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
        }
        if (coverPositionEnemy == null)
        {
            //Debug.Log(enemy + " EnterTakeCover");
            if (enemy.findingCover.FindCoverInRaduis(15,out CoverPoint coverPoint))
            {
                this.coverPositionEnemy = coverPoint;
                //Debug.Log("FindCoverComplete");
            }
            else
            {
                enemy.currentTactic = new HoldingTactic(enemy);
                //Debug.Log("FindCoverFaild");
            }
        }
        else if(coverPositionEnemy != null)
        {
            if (enemy.isInCover == false)
            {
                if (MoveToCover(enemy.coverPos, agent))
                {
                    isInCover = true;
                }
            }
            else if (isInCover == true)
            {
                CoverUsingPattern();
                //if (CheckingCoverInterupt())
                //{
                //    enemy.currentTactic = new HoldingTactic(enemy);
                //}
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
        //NavMeshHit hit;
        //float maxDistance = 1;
        //if (NavMesh.SamplePosition(peekPos, out hit, maxDistance, NavMesh.AllAreas))
        //{
        //    agent.SetDestination(hit.position);
        //}

        //agent.SetDestination(peekPos);
        enemy.enemyController.AimDownSight();
        //enemy.weaponCommand.AimDownSight();
        //enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._moveWithAgent);
        //enemyFiringPattern.Performing();

        enemy.enemyController.Rotate((enemy.targetKnewPos - enemy.transform.position).normalized, 6);
        //new RotateObjectToward().RotateTowardsObjectPos(enemy.targetKnewPos, enemy.gameObject, 6);
        if (enemy.findingTargetComponent.FindTarget(out GameObject target) == false)
        {
            enemy.cost += costRate * Time.deltaTime;
        }
    }
    private void BackToCover(Vector3 CoverPos, NavMeshAgent agent) 
    {
        //NavMeshHit hit;
        //float maxDistance = 1;

        //if (NavMesh.SamplePosition(CoverPos, out hit, maxDistance, NavMesh.AllAreas))
        //{
        //    agent.SetDestination(hit.position);
        //}
        //agent.SetDestination(CoverPos);
        enemy.enemyController.LowReady();
        enemy.enemyController.Rotate((enemy.coverPos - enemy.transform.position).normalized, 6);
        //enemy.weaponCommand.LowReady();
        //enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._moveWithAgent);

    }
    bool isSetMovePos = false;
    private bool MoveToCover(Vector3 CoverPos, NavMeshAgent agent)
    {
        if (isSetMovePos == false)
        {
            float maxDistance = 1.8f;
            if (NavMesh.SamplePosition(CoverPos, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                isSetMovePos = true;
            }
            else 
            {
                enemy.currentTactic = new HoldingTactic(enemy);
            }
        }
        if (Vector3.Distance(enemy.transform.position, new Vector3(CoverPos.x,enemy.transform.position.y,CoverPos.z)) < 1.6f)
        {
            //enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._idle);
            enemy.enemyController.Freez();
            enemy.enemyController.TakeCover();
            isSetMovePos = false;
            return true;
        }
        else
        {
            //enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._sprint);
            enemy.enemyController.Sprint();
            enemy.enemyController.LowReady();
            //enemy.weaponCommand.LowReady();
            enemy.cost +=   costRate * Time.deltaTime;
            return false;
        }
    }

    float timimgCoverPattern = 0;
    bool IsPeeking;
    private void CoverUsingPattern()
    {
        if (enemy.findingTargetComponent.FindTarget(out GameObject target) == true)
        {
            new RotateObjectToward().RotateTowardsObjectPos(enemy.targetKnewPos, enemy.gameObject, 6);
            enemy.enemyController.AimDownSight();
            enemyFiringPattern.Performing();
        }
        else
        {
            if (IsPeeking == true)
            {
                PeekAndShoot(enemy.peekPos, agent);
            }
            else if (IsPeeking == false)
            {
                BackToCover(enemy.coverPos, agent);
            }
            timimgCoverPattern -= Time.deltaTime;
        }
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
    //private bool CheckingCoverInterupt()
    //{
    //    Collider[] other = Physics.OverlapSphere(enemy.transform.position, 0.5f);
    //    foreach(Collider collider in other)
    //    {
    //        Enemy enemyNearBy;
    //        if(collider.TryGetComponent<ChestBodyPart>(out ChestBodyPart e))
    //        {
    //            enemyNearBy = e.enemy;
    //            if (enemyNearBy != enemy)
    //            {
    //                return true;
    //            }
    //        }
           
    //    }
    //    return false;
    //}
}
