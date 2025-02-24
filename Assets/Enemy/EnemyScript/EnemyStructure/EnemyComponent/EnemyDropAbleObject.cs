using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient,IObserverEnemy
{
    protected Enemy enemy;
    bool isAlreadyHitted;
    bool isAlreadyKilled;

    void IObserverEnemy.Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if (enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotHit)
        {
            if(isAlreadyHitted)
                return;

            if (enemy.curNodeLeaf is GotKnockDown_GunFuGotHitNodeLeaf)
            {
                base.DropObject(base.HpGetAbleObject);
                base.DropObject(base.AmmoGetAbleObject);
                base.DropObject(base.AmmoGetAbleObject);
                base.DropObject(base.AmmoGetAbleObject);
                isAlreadyHitted = true;
            }
        }

        if(enemyEvent == SubjectEnemy.EnemyEvent.Dead)
        {
            if(isAlreadyKilled)
                return;

            base.DropObject(base.HpGetAbleObject);
            base.DropObject(base.AmmoGetAbleObject);
            isAlreadyKilled = true;
        }
    }
    private void Awake()
    {
        isAlreadyKilled = false;
        isAlreadyHitted = false;
    }
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.AddObserver(this);
    }
    
}
