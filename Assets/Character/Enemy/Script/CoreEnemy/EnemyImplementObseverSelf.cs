using UnityEngine;

public partial class Enemy : IObserverEnemy
{
    public void Notify(Enemy enemy, EnemyEvent enemyEvent)
    {
        
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
        
    }
}
