using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAction : MonoBehaviour
{
    public Animator m_Animator;
    public Enemy _enemy;
    public EnemyStateManager _enemyStateManager;
    public GameObject Target;
    public EnemyPath e_nemyPath;
    void Start()
    {
        e_nemyPath = new EnemyPath(_enemy.agent);
        _enemyStateManager = new EnemyStateManager(this);
    }
    void Update()
    {
        e_nemyPath.UpdateTargetPos(Target.transform.position, gameObject.transform.position);
        _enemyStateManager.Update(this);
    }
    private void FixedUpdate()
    {
        _enemyStateManager.FixedUpdate(this);
    }
}
