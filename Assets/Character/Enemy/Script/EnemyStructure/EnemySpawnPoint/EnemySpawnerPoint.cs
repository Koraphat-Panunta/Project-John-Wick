using System;
using UnityEngine;

public class EnemySpawnerPoint : MonoBehaviour
{

    public virtual Vector3 spawnPosition { get => transform.position; protected set { } }
    public virtual Quaternion spawnRotiation { get => transform.rotation; protected set { } }

    public virtual Enemy SpawnEnemy(EnemyObjectManager enemyObjectManager, EnemyDirector enemyDirector, WeaponObjectManager weaponObjectManager)
    {
        Enemy enemy = this.SpawnEnemy(enemyObjectManager, weaponObjectManager);

        if (enemy.TryGetComponent<EnemyRoleBasedDecision>(out EnemyRoleBasedDecision enemyRoleBasedDecision))
            enemyDirector.AddEnemy(enemyRoleBasedDecision);
        else
            throw new Exception("Can not get enemyRoleBasedDicision " + enemy);

        return enemy;
    }
    public virtual Enemy SpawnEnemy(EnemyObjectManager enemyObjectManager, WeaponObjectManager weaponObjectManager)
    {

        Enemy enemy = this.SpawnEnemy(enemyObjectManager);
        Weapon weapon = weaponObjectManager.SpawnWeapon(enemy);

        return enemy;
    }
    public virtual Enemy SpawnEnemy(EnemyObjectManager enemyObjectManager)
    {
        Enemy enemy = enemyObjectManager.GetEnemy(this.spawnPosition, this.spawnRotiation);
        return enemy;

    }

    
}
