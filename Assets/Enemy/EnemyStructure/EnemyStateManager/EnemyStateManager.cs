using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyState _currentState;
    public EnemyMove _move { get; private set; }
    public EnemyIdle _idle { get; private set; }
    public EnemyPainState _painState { get; private set; }
    public EnemyDead enemyDead { get; private set; }
    public Enemy enemy;
    // Start is called before the first frame update
    
    private void Start()
    {
        _move = new EnemyMove();
        _idle = new EnemyIdle();
        _painState = new EnemyPainState();
        enemyDead = new EnemyDead();
        _currentState = _idle;
        _currentState.StateEnter(this);
    }
    public void FixedUpdate()
    {
        _currentState.StateFixedUpdate(this);
    }
    public void Update()
    {
        _currentState.StateUpdate(this);
    }
    public void ChangeState(EnemyState nextState)
    {
        if (_currentState != nextState)
        {
            _currentState.StateExit(this);
            _currentState = nextState;
            _currentState.StateEnter(this);
        }
    }
    public void ChangeState(EnemyPainState painState,IEnemyHitReaction hitReaction)
    {
        if(_currentState != painState)
        {
            _currentState.StateExit(this);
        }
        _currentState = painState;
        painState.StateEnter(this,hitReaction);
    }
}
