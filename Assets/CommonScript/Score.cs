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
        Debug.Log("NotifyEvent :" + enemyEvent);
        if (enemyEvent == SubjectEnemy.EnemyEvent.GetShoot_Head)
        {
            score += 100;
            Debug.Log("Current Score = " + score);
        }
        if (enemyEvent == SubjectEnemy.EnemyEvent.Dead)
        {
            score += 50;
            Debug.Log("Current Score = " + score);
            enemy.RemoveObserver(this);
        }
        
    }
    public void Reset()
    {
        score = 0;
    }
}
