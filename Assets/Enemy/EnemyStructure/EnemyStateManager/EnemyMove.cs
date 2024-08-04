using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : EnemyState
{
    public EnemyPath enemyPath;
    public EnemyMove()
    {
        enemyPath = new EnemyPath();
    }
    public override void StateEnter(EnemyStateManager enemyState)
    {
        if (enemyState.EnemyAction._enemy.agent.hasPath == false)
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
        enemyPath.UpdateTargetPos(enemyState.EnemyAction.Target.transform.position);
        //if (Vector3.Distance(enemyPath.targetAnchor, enemyPath.targetPos) > 10)
        //{
        //    enemyPath.RegenaratePath(enemyState.EnemyAction.Target.transform.position, enemyState.EnemyAction._enemy.gameObject.transform.position,agent);
        //}
        if (agent.hasPath)
        {
            Vector3 dir = agent.steeringTarget - MyEnemy.transform.position;
            Vector3 animDir = MyEnemy.transform.InverseTransformDirection(dir);
            //float dot = Vector3.Dot(MyEnemy.transform.position, dir);
            //bool isFacingDir;
            //if (dot > 0.65f)
            //{
            //    isFacingDir = true;
            //}
            //else
            //{
            //    isFacingDir = false;
            //}

            animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
            animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);

            MyEnemy.transform.rotation = Quaternion.RotateTowards(MyEnemy.transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
            //If Enemy reach destination of NavmeshAgent
            CheckReachDestination(MyEnemy,agent,animator,enemyState);
        }
        else
        {
            enemyState.ChangeState(enemyState._idle);
        }
        //else
        //{
        //    animator.SetFloat("Vertical", Mathf.Lerp(animator.GetFloat("Vertical"), 0, 2 * Time.deltaTime));
        //    animator.SetFloat("Horizontal", Mathf.Lerp(animator.GetFloat("Horizontal"), 0, 2 * Time.deltaTime));
        //}    
    }
    private void CheckReachDestination(GameObject MyEnemy, NavMeshAgent agent, Animator animator, EnemyStateManager enemyState)
    {
        if (Vector3.Distance(MyEnemy.transform.position, agent.destination) <= agent.radius)
        {
            enemyPath.SetNavDestinationNext(agent);
            //if (agent.SetDestination(enemyPath._markPoint[0])) { enemyPath._markPoint.RemoveAt(0);}
            //if (enemyPath._markPoint.Count > 0)
            //{
            //    agent.ResetPath();
            //    agent.SetDestination(enemyPath._markPoint[0]);
            //}
            //else if (agent.SetDestination(enemyState.EnemyAction.Target.transform.position) == false)
            //{
            //    agent.ResetPath();
            //    agent.SetDestination(enemyState.EnemyAction.Target.transform.position);
            //}
            //else
            //{
            //    agent.ResetPath();
            //    enemyState.ChangeState(enemyState._idle);
            //}
        }
    }
}
