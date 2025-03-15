using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class SearchingTacticDecision : TacticDecision,IObserverEnemy
{
    public Vector3 anchorPos;
    public Vector3 searchingPos;
    public float waitTime;
    public SearchingTacticDecision(Enemy enemy, EnemyTacticDecision enemyTacticDecision) : base(enemy, enemyTacticDecision)
    {
        anchorPos = enemy.transform.position;
        enemy.AddObserver(this);
    }

    public override void Enter()
    {
        searchingPos = RandomPosInNavmesh();
        waitTime = 3;
    }
    public override void Exit()
    {
        enemyCommand.Freez();

    }

    public override void FixedUpdate()
    {
       
    }

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
       
    }

    public override void Update()
    {
        switch (enemyTacticDecision.curCombatPhase)
        {
            case EnemyTacticDecision.CombatPhase.Chill: 
                {
                    if (enemyCommand.MoveToPosition(searchingPos, 1, true))
                    {
                        enemyCommand.Freez();

                        if (waitTime > 0)
                            waitTime -= Time.deltaTime;

                        if (waitTime <= 0)
                        {
                            searchingPos = RandomPosInNavmesh();
                            waitTime = 3;
                        }

                    }
                    enemyCommand.LowReady();
                }
                break;
            case EnemyTacticDecision.CombatPhase.Aware: 
                {
                    enemyCommand.MoveToPosition(enemy.targetKnewPos, 1, true);
                    enemyCommand.LowReady();
                }
                break;
            case EnemyTacticDecision.CombatPhase.Alert: 
                {
                    enemyTacticDecision.ChangeTactic(enemyTacticDecision.encouterTacticDecision);
                }
                break;
        }

       
    }

    private Vector3 RandomPosInNavmesh()
    {
        Vector3 position = anchorPos;
        position += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * Random.Range(3, 6);
        NavMeshHit hit;
        float maxDistance = 100f;

        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
            return hit.position;
        else
            return anchorPos;
        
    }
}
