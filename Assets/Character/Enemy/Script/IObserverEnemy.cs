using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverEnemy 
{
    public void Notify(Enemy enemy,SubjectEnemy.EnemyEvent enemyEvent);
    public void Notify<T>(Enemy enemy, T node) where T : INode;
}
