using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    private Enemy enemy;
    public Animator animator;

    public float _moveVelocityForward;
    public float _moveVelocitySideward;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        NavMeshAgent agent = enemy.agent;
        _moveVelocityForward = Vector3.Dot(enemy.transform.forward,agent.velocity.normalized) * agent.velocity.magnitude;
        _moveVelocitySideward = Vector3.Dot(agent.transform.right,agent.velocity.normalized) * agent.velocity.magnitude;
    }
    private void FixedUpdate()
    {
        
    }
}
