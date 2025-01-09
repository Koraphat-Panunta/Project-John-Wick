
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SerchingTactic : IEnemyTactic
{
    private Enemy enemy;
    private EnemyStateManager enemyStateManager;
    //private RotateObjectToward enemyRot;
    private NavMeshAgent agent;

    public SerchingTactic(Enemy enemy)
    {
        this.enemy = enemy;
        //this.enemyStateManager = _enemy.enemyStateManager;
        //this.enemyRot = new RotateObjectToward();
        this.agent = enemy.agent;
        agent.speed = 0;
        agent.acceleration = 0;
        agent.ResetPath();
        agent.destination = RandomPosInNavmesh();
        //_enemy.StartCoroutine(PerformedAction());
        //_enemy.isInCombat = false;
    }
    public void Manufacturing()
    {
        enemy.weaponCommand.LowReady();
        if (agent.hasPath == false)
        {
            agent.destination = RandomPosInNavmesh();
            //_enemy.StartCoroutine(PerformedAction());
        }
        else if (agent.hasPath && (Vector3.Distance(agent.destination, enemy.transform.position) < agent.radius + 0.5f))
        {
            agent.ResetPath();
            agent.destination = RandomPosInNavmesh();
            //_enemy.StartCoroutine(PerformedAction());
        }
        if (agent.hasPath && (Vector3.Distance(agent.destination, enemy.transform.position) >= agent.radius + 0.5f))
        {
            //agent.ResetPath();
            //agent.destination = RandomPosInNavmesh();
            //_enemy.StartCoroutine(PerformedAction());
            Vector3 dir = (agent.steeringTarget - enemy.transform.position).normalized;
            enemy.enemyController.Move(dir, 1);
        }
        if (enemy.combatOffensiveInstinct.myCombatPhase != CombatOffensiveInstinct.CombatPhase.Chill)
        {
            enemy.currentTactic = new FlankingTactic(enemy);
        }

        //enemyRot.RotateToward((agent.destination-_enemy.transform.position).normalized,_enemy.gameObject,6);
        if (enemy.findingTargetComponent.FindTarget(out GameObject target) == true)
        {
            //_enemy.StopCoroutine(PerformedAction());
            enemy.enemyComunicate.SendNotify(EnemyComunicate.NotifyType.SendTargetLocation, 18f);
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
            return hit.position;
        }
        else
        {
            return enemy.transform.position;
        }
    }
    //IEnumerator PerformedAction()
    //{
    //    //_enemy.enemyStateManager.ChangeState(_enemy.enemyStateManager._idle);
    //    _enemy.enemyController.Freez();
    //    yield return new WaitForSeconds(2);
    //    //_enemy.enemyStateManager.ChangeState(_enemy.enemyStateManager._move);
    //    Vector3 dir = _enemy.agent.steeringTarget - _enemy.transform.position;
    //    _enemy.enemyController.Move(dir,6);
    //}

   
}
