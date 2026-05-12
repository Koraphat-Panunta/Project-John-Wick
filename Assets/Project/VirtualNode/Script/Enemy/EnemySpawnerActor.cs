using UnityEngine;

public class EnemySpawnerActor : Actor
{
    [SerializeField] private EnemySpawnerData spawnerData;
    [SerializeField] private EnemyPoolManager enemyPoolManager;
    [SerializeField] private EnemySpawnerPoint enemySpawnerPoint;

    protected OnDrawGizmosTriggerEvent drawGizmosTriggerEvent = new OnDrawGizmosTriggerEvent();

    public void SpawnEnemyUnityEvent()
    {
        Enemy spawnedEnemy = enemySpawnerPoint.SpawnEnemy(enemyPoolManager, spawnerData, null);

        if (returnEnemyActor != null)
            returnEnemyActor.AddEnemy(spawnedEnemy);
    }

    [SerializeField] protected EnemyActor returnEnemyActor;


    protected override void OnDrawGizmos()
    {
        if (this.returnEnemyActor != null
            && this.isEnableGizmos)
        {

            this.drawGizmosTriggerEvent.isDrawEnable = true;
            this.drawGizmosTriggerEvent.DrawLine(this.transform.position, this.returnEnemyActor.transform.position, color);
            base.DrawName(Vector3.Lerp(this.transform.position, this.returnEnemyActor.transform.position, .3f), "returnEnemyActor");
        }

        //if (enemyDirector != null
        //    && this.isEnableGizmos)
        //{
        //    Gizmos.color = enemyDirector.color * .25f;
        //    Gizmos.DrawLine(this.transform.position, this.enemyDirector.transform.position);
        //}

        base.OnDrawGizmos();
    }
}
