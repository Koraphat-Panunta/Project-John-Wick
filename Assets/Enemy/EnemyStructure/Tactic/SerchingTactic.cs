
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SerchingTactic : IEnemyTactic
{
    private Enemy enemy;
    private EnemyStateManager enemyStateManager;
    private RotateObjectToward enemyRot;
    private EnemyWeaponCommand enemyWeaponCommand;
    private NavMeshAgent agent;

    public SerchingTactic(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemyStateManager = enemy.enemyStateManager;
        this.enemyRot = new RotateObjectToward();
        this.enemyWeaponCommand = enemy.enemyWeaponCommand;
        this.agent = enemy.agent;
        agent.ResetPath();
        agent.destination = RandomPosInNavmesh();
        enemy.StartCoroutine(PerformedAction());
    }
    public void Manufacturing()
    {
        enemyWeaponCommand.LowReady();
        if(agent.hasPath == false )
        {
            Debug.Log("Agent has path false");
            agent.destination = RandomPosInNavmesh();
            enemy.StartCoroutine(PerformedAction());
        }
        else if(agent.hasPath && (Vector3.Distance(agent.destination,enemy.transform.position)<agent.radius+0.5f))
        {
            Debug.Log("Agent des < distance");
            agent.ResetPath();
            agent.destination = RandomPosInNavmesh();
            enemy.StartCoroutine(PerformedAction());
        }
        enemy.enemyLookForPlayer.Recived();
        enemyRot.RotateTowards((agent.destination-enemy.transform.position).normalized,enemy.gameObject,6);
        if(enemy.enemyLookForPlayer.IsSeeingPlayer == true)
        {
            enemy.StopCoroutine(PerformedAction());
            enemy.currentTactic = new FlankingTactic(enemy);
        }
    }
    private Vector3 RandomPosInNavmesh()
    {
        Vector3 position = enemy.transform.position;
        position += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))*Random.Range(5,15);
        NavMeshHit hit;
        float maxDistance = 100f;
        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
        {
            Debug.Log("Return Hit Pos");
            return hit.position;
        }
        else
        {
            Debug.Log("Return cur Pos");
            return enemy.transform.position;
        }
    }
    IEnumerator PerformedAction()
    {
        Debug.Log("Performed called");
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._idle);
        yield return new WaitForSeconds(2);
        enemy.enemyStateManager.ChangeState(enemy.enemyStateManager._move);
    }

   
}
