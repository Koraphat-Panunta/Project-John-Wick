using System;
using UnityEngine;

public class EnemyUnityEventSpawn : MonoBehaviour
{
    [SerializeField] private EnemyObjectManager enemyObjectManager;
    [SerializeField] private EnemyDirector enemyDirector;
    [SerializeField] private WeaponObjectManager weaponObjectManager;

    [SerializeField] private EnemySpawnerPoint enemySpawnerPoint;
    public void SpawnEnemyUnityEvent()
    {
        if (enemyDirector == null && weaponObjectManager == null)
            enemySpawnerPoint.SpawnEnemy(enemyObjectManager);
        else if (enemyDirector == null)
            enemySpawnerPoint.SpawnEnemy(enemyObjectManager, weaponObjectManager);
        else
            enemySpawnerPoint.SpawnEnemy(enemyObjectManager,enemyDirector,weaponObjectManager);
    }
}
