using UnityEngine;
using static SubjectEnemy;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient, IObserverEnemy, IInitializedAble
{
    [SerializeField] protected Enemy enemy;
    bool isAlreadyDrop;
    bool isBeenExecute;


    [SerializeField] protected AmmoGetAbleObject AmmoGetAbleObject;
    [SerializeField] protected HpGetAbleObject HpGetAbleObject;


    public void Initialized()
    {
        enemy.AddObserver(this);
    }
    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (node is SubjectEnemy.EnemyEvent enemyEvent
            && enemyEvent == SubjectEnemy.EnemyEvent.OnEnable)
        {
            isBeenExecute = false;
            isAlreadyDrop = false;

        }

        if(node is IGotGunFuExecuteNodeLeaf)
        {
            this.isBeenExecute = true;
        }
     

        if (node is EnemyDeadStateNode deadState && deadState.curstate == EnemyStateLeafNode.Curstate.Enter && isAlreadyDrop == false)
        {
            if (this.isBeenExecute)
            {
                base.DropObject(AmmoGetAbleObject);
                base.DropObject(AmmoGetAbleObject);
                base.DropObject(AmmoGetAbleObject);

                base.DropObject(HpGetAbleObject);
                base.DropObject(HpGetAbleObject);
            }

            isAlreadyDrop = true;
            return;
        }

    }
  
   
}
