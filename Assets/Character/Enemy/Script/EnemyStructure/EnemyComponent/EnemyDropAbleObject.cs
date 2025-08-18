using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient,IObserverEnemy
{
    protected Enemy enemy;
    bool isAlreadyDrop;

    [SerializeField] protected AmmoGetAbleObject AmmoGetAbleObject;
    [SerializeField] protected HpGetAbleObject HpGetAbleObject;

    void IObserverEnemy.Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
     


       if(node is EnemyDeadStateNode && isAlreadyDrop == false)
        {
            AmmoGetAbleObject.amoutAmmoAdd = Random.Range(5, 10);
            HpGetAbleObject.amoutOfHpAdd = 20;
            base.DropObject(AmmoGetAbleObject);
            base.DropObject(HpGetAbleObject);
            isAlreadyDrop = true;
            return;
        }

    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy.AddObserver(this);
    }

}
