using UnityEngine;

public class EnemySpawnerPoint : MonoBehaviour
{

    public virtual Vector3 spawnPosition { get => transform.position; protected set { } }
    public virtual Quaternion spawnRotiation { get => transform.rotation; protected set { } }

    public virtual Enemy SpawnEnemy(EnemyPoolManager poolManager, EnemySpawnerData data, EnemyDirector fallbackDirector)
    {
        Enemy enemy = poolManager.GetEnemy(data.enemyData, spawnPosition, spawnRotiation);

        if (data.weaponObjectManager != null)
            data.weaponObjectManager.SpawnWeapon(enemy);

        EnemyDirector director = data.enemyDirector != null ? data.enemyDirector : fallbackDirector;
        if (director != null && enemy.TryGetComponent<IEnemyDirectedAble>(out IEnemyDirectedAble directed))
            director.AddEnemy(directed);

        EnemySpawnerExtensionData.ApplyExtensions(enemy, data.extensions);

        return enemy;
    }

}
