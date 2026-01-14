using UnityEngine;
using static SubjectEnemy;

[RequireComponent(typeof(Enemy))]
public class EnemyDropAbleObject : DropAbleObjectClient, IObserverEnemy, IInitializedAble
{
    [SerializeField] protected Enemy enemy;
    bool isAlreadyDrop;
    int ammoDropNumber;
 

    [SerializeField] protected AmmoGetAbleObject AmmoGetAbleObject;
    [SerializeField] protected HpGetAbleObject HpGetAbleObject;


    public void Initialized()
    {
        enemy.AddObserver(this);
    }
    public void Notify<T>(Enemy enemy, T node)
    {
        if (node is SubjectEnemy.EnemyEvent enemyEvent
            && enemyEvent == SubjectEnemy.EnemyEvent.OnEnable)
        {
            isAlreadyDrop = false;
            ammoDropNumber = 3;
        }

        if (node is GotGunFuHitNodeLeaf gotHit
            && gotHit.curstate == EnemyStateLeafNode.Curstate.Enter
            &&
            (
            gotHit.gotHitstateName == GotGunFuManuverStateName.GotHit1_Attack.ToString()||
            gotHit.gotHitstateName == GotGunFuManuverStateName.GotHit2_Attack.ToString()||    
            gotHit.gotHitstateName == GotGunFuManuverStateName.GotHit3.ToString()
            )
            && ammoDropNumber >0)
        {
            ammoDropNumber--;
            AmmoGetAbleObject.amoutAmmoAdd = 5;
            base.DropObject(AmmoGetAbleObject);
        }

        if (node is EnemyDeadStateNode deadState && deadState.curstate == EnemyStateLeafNode.Curstate.Enter && isAlreadyDrop == false)
        {

            HpGetAbleObject.amoutOfHpAdd = 20;
            base.DropObject(HpGetAbleObject);
            isAlreadyDrop = true;
            return;
        }

    }
  
   
}
