using System;
using UnityEngine;

public class EnemySpawnerActor : Actor
{
    [SerializeField] private EnemyObjectManager enemyObjectManager;
    [SerializeField] private EnemyDirector enemyDirector;
    [SerializeField] private WeaponObjectManager weaponObjectManager;

    [SerializeField] private EnemySpawnerPoint enemySpawnerPoint;

    protected OnDrawGizmosTriggerEvent drawGizmosTriggerEvent = new OnDrawGizmosTriggerEvent();
    public void SpawnEnemyUnityEvent()
    {

        Debug.Log("EnemySpawnerActor SpawnEnemyUnityEvent");

        Enemy spawnedEnemy;


            
        if (enemyDirector == null && weaponObjectManager == null)
            spawnedEnemy = enemySpawnerPoint.SpawnEnemy(enemyObjectManager);
        else if (enemyDirector == null)
            spawnedEnemy = enemySpawnerPoint.SpawnEnemy(enemyObjectManager, weaponObjectManager);
        else
            spawnedEnemy = enemySpawnerPoint.SpawnEnemy(enemyObjectManager,enemyDirector,weaponObjectManager);

        if(returnEnemyActor != null)
        this.returnEnemyActor.AddEnemy(spawnedEnemy);
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

        //if(enemyDirector != null
        //    && this.isEnableGizmos) 
        //{
        //    Gizmos.color = enemyDirector.color * .5f;
        //    Gizmos.DrawLine(this.transform.position, this.enemyDirector.transform.position);
        //}

        base.OnDrawGizmos();
    }
}
