using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor,IObserverEnemy
{
    [SerializeField] protected List<Enemy> enemies;

    private void Awake()
    {
        if(this.enemies != null && this.enemies.Count > 0)
        {
            for(int i = 0;i < this.enemies.Count; i++)
            {
                this.enemies[i].AddObserver(this);
            }
        }
    }
    public void Notify<T>(Enemy enemy, T node)
    {
        base.NotifyObserver<T>(node);
    }
    public void AddEnemy(Enemy enemy)
    {
        if(this.enemies == null)
            this.enemies = new List<Enemy>();

        enemy.AddObserver(this);
        this.enemies.Add(enemy);

    }

    protected override void OnDrawGizmos()
    {
        if (base.isEnableGizmos
            && this.enemies != null 
            && this.enemies.Count > 0)
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < this.enemies.Count; i++) 
            {
                Gizmos.DrawLine(transform.position, enemies[i].transform.position);
            }
        }
        base.OnDrawGizmos();
    }

   
}
