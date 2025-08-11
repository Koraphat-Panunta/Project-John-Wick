using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient,IObserverEnemy
{
    protected Enemy enemy;
    bool isAlreadyExecuted;
    bool isBeenExecute;

    [SerializeField] protected AmmoGetAbleObject AmmoGetAbleObject;

    void IObserverEnemy.Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
       if(node is IGotGunFuExecuteNodeLeaf gotGunFuExecuteNodeLeaf)
            isBeenExecute = true;

       if(node is EnemyDeadStateNode && isBeenExecute && isAlreadyExecuted == false)
        {
            AmmoGetAbleObject.amoutAmmoAdd = Random.Range(7, 10);
            base.DropObject(AmmoGetAbleObject);
            isAlreadyExecuted = true;
        }
    }
    private void Awake()
    {
        isAlreadyExecuted = false;
        enemy = GetComponent<Enemy>();
        enemy.AddObserver(this);
    }

}
