using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager 
{
    public EnemyState _currentState;
    public EnemyMove _move { get; private set; }
    public EnemyIdle _idle { get; private set; }    
    public EnemyAction EnemyAction { get; private set; }
    // Start is called before the first frame update
    public EnemyStateManager(EnemyAction enemyAction)
    {
        EnemyAction = enemyAction;
        _move = new EnemyMove();
        _idle = new EnemyIdle();
        _currentState = _idle;
        _currentState.StateEnter(this);
    }

    public void FixedUpdate(EnemyAction enemyAction)
    {
        _currentState.StateFixedUpdate(this);
    }
    public void Update(EnemyAction enemyAction)
    {
        _currentState.StateUpdate(this);
    }
    public void ChangeState(EnemyState nextState)
    {
        if (_currentState != null)
        {
            _currentState.StateExit(this);
        }
        _currentState = nextState;
        _currentState.StateEnter(this);
    }
}
