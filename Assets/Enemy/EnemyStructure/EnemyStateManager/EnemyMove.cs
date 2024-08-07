using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : EnemyState
{
    EnemyPath enemyPath;
    public EnemyMove()
    {

    }
    public override void StateEnter(EnemyStateManager enemyState)
    {
        Debug.Log("Move Enter");
        if (enemyPath == null)
        {
            enemyPath = enemyState.EnemyAction.e_nemyPath;
        }
        if (enemyPath._markPoint.Count<=0)
        {
            enemyPath.GenaratePath(enemyState.EnemyAction.Target.transform.position,enemyState.EnemyAction._enemy.gameObject.transform.position);
            enemyPath.SetNavDestinationNext(enemyState.EnemyAction._enemy.agent);
        }
    }
    public override void StateExit(EnemyStateManager enemyState)
    {
        
    }
    public override void StateFixedUpdate(EnemyStateManager enemyState)
    {
       
    }
    public override void StateUpdate(EnemyStateManager enemyState)
    {
        Animator animator = enemyState.EnemyAction._enemy.animator;
        NavMeshAgent agent = enemyState.EnemyAction._enemy.agent;
        GameObject MyEnemy = enemyState.EnemyAction._enemy.gameObject;
        EnemyPath enemyPath = enemyState.EnemyAction.e_nemyPath;
        if (enemyPath._markPoint.Count > 0)
        {
            if (agent.hasPath == false && agent.destination != null)//Reapeat Set agent destination
            {
                enemyPath.SetNavDestinationNext(agent);
            }
            enemyPath.UpdateTargetPos(enemyState.EnemyAction.Target.transform.position);
            if (agent.hasPath)//Move Enemy
            {
                Vector3 dir = agent.steeringTarget - MyEnemy.transform.position;
                Vector3 animDir = MyEnemy.transform.InverseTransformDirection(dir);
                animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
                animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);
                MyEnemy.transform.rotation = Quaternion.RotateTowards(MyEnemy.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
                //If Enemy reach destination of NavmeshAgent
                CheckReachDestination(MyEnemy, agent, animator, enemyState);

            }
            if (Vector3.Distance(enemyPath.targetPos, enemyPath.targetAnchor) > 5)
            {
                enemyPath.RegenaratePath(enemyState.EnemyAction.Target.transform.position,MyEnemy.transform.position,agent);
            }
        }
        else if (enemyPath._markPoint.Count <= 0)
        {
            Debug.Log("Change to idle form move");
            enemyState.ChangeState(enemyState._idle);
        }
        EnemyDebuger.curPos = agent.destination;
    }
    private void CheckReachDestination(GameObject MyEnemy, NavMeshAgent agent, Animator animator, EnemyStateManager enemyState)
    {
        if (Vector3.Distance(MyEnemy.transform.position, agent.destination) <= agent.radius)
        {
            Debug.Log("Reach Destination");
            enemyPath.SetNavDestinationNext(agent);
        }
    }
}
