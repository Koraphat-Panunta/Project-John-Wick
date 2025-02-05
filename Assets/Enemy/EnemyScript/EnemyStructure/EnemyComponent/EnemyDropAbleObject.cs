using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient,IObserverEnemy
{
    protected Enemy enemy;

    void IObserverEnemy.Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if (enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotHit)
        {
            Debug.Log("EnemyDropAble Notify");
            base.DropObject(base.HpGetAbleObject);
            base.DropObject(base.AmmoGetAbleObject);
        }
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.AddObserver(this);
    }
    
}
