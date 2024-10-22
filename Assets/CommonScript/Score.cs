using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : IObserverEnemy
{
    public float score;
    public Score()
    {
        score = 0;
    }
    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if (enemyEvent == SubjectEnemy.EnemyEvent.GetShoot_Head)
        {
            score += 100;
        }
        if (enemyEvent == SubjectEnemy.EnemyEvent.Dead)
        {
            score += 50;
            enemy.RemoveObserver(this);
        }
        
    }
    public void Reset()
    {
        score = 0;
    }
}
