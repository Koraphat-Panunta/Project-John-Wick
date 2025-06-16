using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient,IObserverEnemy
{
    protected Enemy enemy;
    bool isAlreadyExecuted;

    void IObserverEnemy.Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
        if(node is EnemyDeadStateNode)
        {
            if (isAlreadyExecuted)
                return;

            if (enemy.curNodeLeaf is GotExecuteOnGround_GotInteract_NodeLeaf gotExecuteOnGround)
            {
                StackGague gunFuExecuteStackGauge = enemy.gunFuAbleAttacker.gunFuExecuteStackGauge;
                if (gunFuExecuteStackGauge.amount >= gunFuExecuteStackGauge.max)
                {
                    isAlreadyExecuted = true;
                    DropObject(AmmoGetAbleObject);
                    DropObject(AmmoGetAbleObject);
                    DropObject(AmmoGetAbleObject);
                }
            }
        }
    }
    private void Awake()
    {
        isAlreadyExecuted = false;
        enemy = GetComponent<Enemy>();
        enemy.AddObserver(this);
    }

}
