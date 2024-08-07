using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticManager 
{
    public EnemyTactic _currentTactic;
    public Flanking _flanking { get; private set; }
    public EnemyBrain _enemyBrain { get; private set; }
    public Enemy _enemy;
    // Start is called before the first frame update
    public TacticManager(EnemyBrain enemyBrain)
    {
        _enemyBrain = enemyBrain;
        _flanking = new Flanking();
    }

    public void FixedUpdate(EnemyBrain enemyBrain)
    {
        
        _currentTactic.TacticFixedUpdate(enemyBrain);
    }
    public void Update(EnemyBrain enemyBrain)
    {
        _currentTactic.TacticUpdate(enemyBrain);
    }
    public void ChangeTactic(EnemyTactic nextTac)
    {
        if (_currentTactic != null)
        {
            _currentTactic.TacticExit(this);
        }
        _currentTactic = nextTac;
        _currentTactic.TacticEnter(this);
    }
}
