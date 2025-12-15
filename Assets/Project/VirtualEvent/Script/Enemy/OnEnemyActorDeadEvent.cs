using UnityEngine;

public class OnEnemyActorDeadEvent : VirtualEventNode, IObserverActor
{
    [SerializeField] private EnemyActor enemyActor;
    private void Awake()
    {
        enemyActor.AddActorObserver(this);
    }
    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if(actor is EnemyActor
            && var is EnemyDeadStateNode enenmyDead
            && enenmyDead.curstate == EnemyStateLeafNode.Curstate.Enter)
        {
            Execute();
        }
    }

    protected override void OnDrawGizmos()
    {
        if (base.isEnableGizmos
            && enemyActor != null)
        {
            onDrawGizmosTriggerEvent.DrawSphere(enemyActor.transform.position, this.transform.position, color);
        }
        base.OnDrawGizmos();
    }
}
