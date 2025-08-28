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
        if (enemyEvent == SubjectEnemy.EnemyEvent.OnEnable)
            isAlreadyDrop = false;
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
     

       if (node is EnemyDeadStateNode deadState && deadState.curstate == EnemyStateLeafNode.Curstate.Enter && isAlreadyDrop == false)
        {

            AmmoGetAbleObject.amoutAmmoAdd = 6;
            base.DropObject(AmmoGetAbleObject);

            HpGetAbleObject.amoutOfHpAdd = 20;
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
