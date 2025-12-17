using UnityEngine;

public class OnEnemyWaveEndEvent : VirtualEventNode,IObserverActor
{
    [SerializeField] protected EnemyWaveManager enemyWaveManager;

    private void Awake()
    {
        enemyWaveManager.AddActorObserver(this);
    }
    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if(actor is EnemyWaveManager 
            && var is EnemyWaveManager.EnemyWaveEvent enemyWaveEvent
            && enemyWaveEvent == EnemyWaveManager.EnemyWaveEvent.OnWaveEnd)
        {
            Execute();
        }
    }

    protected override void OnDrawGizmos()
    {
        if(isEnableGizmos
            && this.enemyWaveManager != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(enemyWaveManager.transform.position, this.transform.position, color);
        }

        base.OnDrawGizmos();
    }
}
